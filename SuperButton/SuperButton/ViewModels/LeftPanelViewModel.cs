using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Abt.Controls.SciChart;
using SuperButton.CommandsDB;
using SuperButton.Common;
using SuperButton.Models.DriverBlock;
using SuperButton.Views;
using System.IO.Ports;
using Application = System.Windows.Application;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using SuperButton.Views.mainWindowPanels;
using SuperButton.Helpers;
using System.Drawing;
using System.ComponentModel;

namespace SuperButton.ViewModels
{

    public partial class LeftPanelViewModel : BaseViewModel
    {
        #region members
        public PacketFields RxPacket;
        private static readonly object TableLock = new object();
        private readonly object ConnectLock = new object();
        private readonly object pidLock = new object();
        private ComboBox _comboBox;

        private Thread _xlThread = new Thread(ThreadProc);
        #endregion
        #region Actions
        public ActionCommand SetAutoConnectActionCommandCommand
        {
            get { return new ActionCommand(AutoConnectCommand); }
        }
        public ActionCommand ForceStop { get { return new ActionCommand(FStop); } }
        public ActionCommand Showwindow { get { return new ActionCommand(ShowParametersWindow); } }
        private static LeftPanelViewModel _instance;
        private static readonly object Synlock = new object(); //Single tone variabl
                                                               //   public ActionCommand GetPidCurr { get { return new ActionCommand(GPC); } }
                                                               //   public ActionCommand SetPidCurr { get { return new ActionCommand(SPC); } }
        #endregion

        #region Props
        public static LeftPanelViewModel GetInstance
        {
            get
            {
                lock (Synlock)
                {
                    if (_instance != null) return _instance;
                    _instance = new LeftPanelViewModel();
                    return _instance;
                }
            }
        }
        public LeftPanelViewModel()
        {
            EventRiser.Instance.LoggerEvent += Instance_LoggerEvent;
            _comboBox = new ComboBox();

        }
        public ComboBox ComboBoxCOM
        {
            get { return _comboBox; }
            set { _comboBox = value; }
        }
        #region Connect_Button
        private String _connetButtonContent;
        public String ConnectButtonContent
        {
            get { return _connetButtonContent; }
            set
            {
                if(value == "Disconnect") {
                    LeftPanelViewModel.flag = true;
                    Task task = Task.Run((Action)LeftPanelViewModel.BackGroundFunc);
                }
                if (_connetButtonContent == value) return;
                _connetButtonContent = value;
                OnPropertyChanged("ConnectButtonContent");

            }

        }
        #endregion

        private float _setCurrentPid;
        public float SetCurrentPid
        {
            get { return _setCurrentPid; }
            set
            {
                if (_setCurrentPid == value) return;
                _setCurrentPid = value;
                OnPropertyChanged("SetCurrentPid");


            }
        }
        private float _currentPid;
        public float CurrentPid
        {
            get { return _currentPid; }
            set
            {
                if (_currentPid == value) return;

                _currentPid = value;
                OnPropertyChanged("CurrentPid");
            }
        }

        #region Send_Button
        private String _sendButtonContent;
        public String SendButtonContent
        {

            get { return _sendButtonContent; }
            set
            {
                if (_sendButtonContent == value) return;
                _sendButtonContent = value;
                OnPropertyChanged("SendButtonContent");
            }
        }
        #endregion

        #region Stop_Button
        private String _stopButtonContent;
        public String StopButtonContent
        {
            get { return _stopButtonContent; }
            set
            {
                if (_stopButtonContent == value) return;
                _stopButtonContent = value;
                OnPropertyChanged("StopButtonContent");
            }
        }
        #endregion

        #region MotorON_Switch

        private bool _motorOnToggleChecked = false;
        public bool MotorOnToggleChecked
        {
            get
            {
                return _motorOnToggleChecked;
                //Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(400, 0)].CommandValue == "1" ? true : false;               
            }
            set
            {
                _motorOnToggleChecked = value;
                //retrieve the command values
                var temp = Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(1, 1)]; // old value: 400, 0
                //init rxPacket
                RxPacket.ID = Convert.ToInt16(temp.CommandId);
                RxPacket.IsFloat = temp.IsFloat;
                RxPacket.IsSet = true;
                RxPacket.SubID = Convert.ToInt16(temp.CommandSubId);
                RxPacket.Data2Send = _motorOnToggleChecked ? 1 : 0;
                //Sent
                Rs232Interface.GetInstance.SendToParser(RxPacket);
                OnPropertyChanged("MotorONToggleChecked");
            }
        }


        #endregion

        #endregion

        #region Send_Button

        public ActionCommand SendActionCommand { get { return new ActionCommand(() => SendXLS()); } }


        public void SendXLS()
        {
            if (_xlThread.ThreadState == System.Threading.ThreadState.Running)
            {
                System.Windows.MessageBox.Show(" Wait until the end of XLS!! thread running)))");
                return;
            }

            if (_xlThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
            {
                System.Windows.MessageBox.Show(" Wait until the end of XLS!! thread Sleeping)))");
                return;
            }

            if (_xlThread.ThreadState == System.Threading.ThreadState.Unstarted)
            {

                _xlThread.Start();
            }

            if (_xlThread.ThreadState == System.Threading.ThreadState.Stopped)
            {
                _xlThread = new Thread(ThreadProc);
                _xlThread.Start();
            }

        }

        public static bool Stop = false;
        public static ManualResetEvent mre = new ManualResetEvent(false);
        public static string Exelsrootpath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + @"\Exels";
        public static string Recordsrootpath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + @"\Records\";
        static public string excelPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + @"\Exels\const_acc_N.xlsx";
        public static string name;
        private static DataSet output = new System.Data.DataSet();
        static private double mmperRev = 2.5;
        static private double countesPerRev = 1200;
        static private double offset = 0;
        public static Int32 ChankLen = 0;
        private static UInt32 DebugCount = 0;

        private static void ThreadProc()
        {
            byte[] poRefCmd;
            DataTable table;


            poRefCmd = new byte[11];

            using (OleDbConnection connection = new OleDbConnection())
            {
                connection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelPath +
                                                ";Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
                /* The connection string is used to specify how the connection would be performed. */
                connection.Open();
                OleDbCommand command = new OleDbCommand
                    ("SELECT POS " + "FROM [Feuil1$]", connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);

                // Stopwatch sw = new Stopwatch();
                adapter.Fill(output);
                table = output.Tables[0];
            }


            // ProtocolParser.GetInstance.BuildPacketToSend("1", "213" /*CommandId*/, "0" /* subid*/, true /*IsSet*/);
            if (Rs232Interface.GetInstance.IsSynced)
            {
                //Send command to the target 
                PacketFields RxPacket;
                RxPacket.ID = 213;
                RxPacket.IsFloat = false;
                RxPacket.IsSet = true;
                RxPacket.SubID = 0;
                RxPacket.Data2Send = 1;
                //rise event
                Rs232Interface.GetInstance.SendToParser(RxPacket);

            }

            mre.WaitOne();

            Stopwatch sw = new Stopwatch();

            foreach (DataRow row in table.Rows)
            {
                double temp = (double)row[0];
                object item = (double)((((double)row[0] * countesPerRev) / mmperRev) + offset);
                float var = (float)Convert.ToSingle(item);
                int var_i = (int)Convert.ToInt32(item);
                string datatosend = var_i.ToString();

                if (ChankLen > 0)
                {
                    ChankLen--;
                    //  ProtocolParser.GetInstance.BuildPacketToSend(datatosend, "403" /*CommandId*/, "0" /* subid*/, true /*IsSet*/);
                    if (Rs232Interface.GetInstance.IsSynced)
                    {
                        //Send command to the target 
                        PacketFields RxPacket;
                        RxPacket.ID = 403;
                        RxPacket.IsFloat = false;
                        RxPacket.IsSet = false;
                        RxPacket.SubID = 0;
                        RxPacket.Data2Send = var_i;
                        //rise event
                        Rs232Interface.GetInstance.SendToParser(RxPacket);
                    }
                    DebugCount++;
                    #region SW
                    sw.Start();
                    while (sw.ElapsedTicks < 50)
                    {
                    }
                    sw.Reset();
                    #endregion
                }
                else
                {
                    sw.Start();
                    while (sw.ElapsedTicks < 50)
                    {
                    }
                    sw.Reset();
                    ChankLen = 0;
                    //   ProtocolParser.GetInstance.BuildPacketToSend(datatosend, "403" /*CommandId*/, "0" /* subid*/, true /*IsSet*/);
                    if (Rs232Interface.GetInstance.IsSynced)
                    {
                        //Send command to the target 
                        PacketFields RxPacket;
                        RxPacket.ID = 403;
                        RxPacket.IsFloat = false;
                        RxPacket.IsSet = true;
                        RxPacket.SubID = 0;
                        RxPacket.Data2Send = var_i;
                        //rise event
                        Rs232Interface.GetInstance.SendToParser(RxPacket);
                    }
                    DebugCount++;
                    mre.Reset();//Suspend
                    mre.WaitOne();
                }
                lock (TableLock)
                {
                    if (Stop == true)
                    {
                        break;
                    }
                }


            }

            sw.Start();
            while (sw.ElapsedTicks < 10000)
            {
            }
            sw.Reset();
            // DebugCount = 0;
            ChankLen = 0;
            table.Dispose();
        }


        #endregion

        #region Action methods

        /// <summary>
        /// public void AutoConnectCommand()
        /// description:
        /// </summary>
        public void AutoConnectCommand()
        {

            if (Rs232Interface.GetInstance.IsSynced == false)
            {
                lock (ConnectLock)
                {

                    Task task = new Task(Rs232Interface.GetInstance.AutoConnect);

                    task.Start();
                }
            }
            else
            {
                lock (ConnectLock)
                {
                    Task taskDisconnect = new Task(Rs232Interface.GetInstance.Disconnect);
                    taskDisconnect.Start();

                }
            }
        }

        private string _logText;

        private void Instance_LoggerEvent(object sender, EventArgs e)
        {
            LogText = ((CustomEventArgs)e).Msg + Environment.NewLine + LogText;

        }
        public string LogText
        {
            get { return _logText; }

            set
            {
                _logText = value;
                RaisePropertyChanged("LogText");
            }

        }

        private string _comToolTipText;
        public string ComToolTipText
        {
            get
            {

                return
                    _comToolTipText;
            }

            set
            {
                _comToolTipText = value;
                RaisePropertyChanged("ComToolTipText");
            }

        }


        public void FStop()
        {
            lock (TableLock)
            {
                Stop = true;
            }
            Thread.Sleep(1000);

            if (_xlThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
            {
                _xlThread.Abort();
            }
        }

        /*
        public void SPC()
        {
            lock (pidLock)
            {

                if (Rs232Interface.GetInstance.IsSynced)
                {
                    var temp = Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(81, 1)];
                    ////init rxPacket
                    RxPacket.ID = Convert.ToInt16(temp.CommandId);
                    RxPacket.IsFloat = true;
                    RxPacket.IsSet = true;
                    RxPacket.SubID = Convert.ToInt16(temp.CommandSubId);
                    RxPacket.Data2Send = SetCurrentPid;
                    ////Sent
                    Rs232Interface.GetInstance.SendToParser(RxPacket);
         
                }
                else
                    SetCurrentPid = -1;
            }
            }
        public void GPC( )
        {
           lock(pidLock)
            {
               
                if (Rs232Interface.GetInstance.IsSynced)
                {
                    ////init rxPacke

                    var temp = Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(81, 1)];
                    RxPacket.ID = Convert.ToInt16(temp.CommandId);
                    RxPacket.IsFloat = true;
                    RxPacket.IsSet = false;
                    RxPacket.SubID = Convert.ToInt16(temp.CommandSubId);
                    RxPacket.Data2Send = "";
                    ////Sent
                    Rs232Interface.GetInstance.SendToParser(RxPacket);
                    Thread.Sleep(20);
                    CurrentPid = Commands.GetInstance.Pidcurr;
              

                }
                else
                    CurrentPid = -1;
            
            }

        }
        */

        public static bool flag;
        private void ShowParametersWindow()
        {
            if (_connetButtonContent == "Disconnect")
            {
                ParametarsWindow win = new ParametarsWindow();
                win.Show();
                flag = true;
                Task task = Task.Run((Action)BackGroundFunc);
            }

        }
        public void Close_parmeterWindow()
        {
            //flag = false;
        }
        public static void BackGroundFunc()
        {
            while (flag)
            {
                RefreshManger.GetInstance.StartRefresh();
                Thread.Sleep(1000);
                //flag = false; // Joseph added
            }
        }
        #endregion
    }
}
