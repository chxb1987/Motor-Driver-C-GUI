using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Abt.Controls.SciChart;
using SuperButton.CommandsDB;
using SuperButton.Models.DriverBlock;
using SuperButton.Views;
using SuperButton.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace SuperButton.ViewModels
{

    public partial class LeftPanelViewModel : ViewModelBase
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
        public ActionCommand ClearLogCommand
        {
            get { return new ActionCommand(ClearLog); }
        }
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
        public bool ValueChange = false;
        public LeftPanelViewModel()
        {

            EventRiser.Instance.LoggerEvent += Instance_LoggerEvent;
            ComboBoxCOM = ComboBox.GetInstance;
            //Task task = Task.Run((Action)_comboBox.UpdateComList);
        }
        public ComboBox ComboBoxCOM
        {
            get { return _comboBox; }
            set { _comboBox = value; }
        }
        #region Connect_Button
        private String _connetButtonContent;

        //Task Background;
        //Task Connection;
        Task StarterTask;
        public String ConnectButtonContent
        {
            get { return _connetButtonContent; }
            set
            {
                if (value == "Disconnect")
                {
                    
                    //Connection = Task.Run((Action)LeftPanelViewModel.VerifyDriverCom);
                    StarterTask = Task.Run((Action)StarterOperation);
                }
                else
                {
                    if (flag)
                    {
                    }
                    LeftPanelViewModel.flag = false;
                    ConnectTextBoxContent = "Not Connected";
                }
                if (_connetButtonContent == value) return;
                _connetButtonContent = value;
                OnPropertyChanged("ConnectButtonContent");

            }

        }
        public bool StarterOperationFlag = false;
        private void StarterOperation()
        {
            #region Operations
            StarterOperationFlag = true;
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "",
                ID = Convert.ToInt16(1),
                SubID = Convert.ToInt16(0),
                IsSet = false,
                IsFloat = false
            });

            //Thread.Sleep(1000);
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "",
                ID = Convert.ToInt16(66), // IfullScale
                SubID = Convert.ToInt16(0),
                IsSet = false,
                IsFloat = true
            });
            Thread.Sleep(20);
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "",
                ID = Convert.ToInt16(66), // VfullScale
                SubID = Convert.ToInt16(1),
                IsSet = false,
                IsFloat = true
            });

            Thread.Sleep(20);
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "",
                ID = Convert.ToInt16(60),
                SubID = Convert.ToInt16(1), // SelectedCh1DataSource
                IsSet = false,
                IsFloat = false
            });
            Thread.Sleep(20);
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "",
                ID = Convert.ToInt16(60),
                SubID = Convert.ToInt16(2), // SelectedCh2DataSource
                IsSet = false,
                IsFloat = false
            });

            for (int i = 1; i < 4; i++)
            {
                Thread.Sleep(20);
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = "",
                    ID = Convert.ToInt16(62),
                    SubID = Convert.ToInt16(i),
                    IsSet = false,
                    IsFloat = false
                });
            }
            Thread.Sleep(20);
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "",
                ID = Convert.ToInt16(60),
                SubID = Convert.ToInt16(9), // Get Units Name
                IsSet = false,
                IsFloat = false
            });
            Thread.Sleep(20);
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "",
                ID = Convert.ToInt16(60),
                SubID = Convert.ToInt16(10), // Get Units Name
                IsSet = false,
                IsFloat = false
            });
            
            #endregion  Operations
            LeftPanelViewModel.flag = true;
            StarterOperationFlag = false;

            if(EnRefresh)
            {
                Thread bkgnd = new Thread(BackGroundFunc);
                bkgnd.Start();
                //BackGroundFunc();
            }
#if !DEBUG
            RefreshManger.GetInstance.VerifyConnection();
#endif

        }
        private String _connectTextBoxContent;
        public String ConnectTextBoxContent
        {
            get
            {
                //if (_connetButtonContent == "Connect")
                //{
                //    //if (ParametarsWindow.WindowsOpen)
                //    //    Close_parmeterWindow();
                //    return "Not Connected";
                //}
                //else
                //{
                    return _connectTextBoxContent;//"Connected";
                //}

            }
            set
            {
                if (_connectTextBoxContent == value) return;
                _connectTextBoxContent = value;
                OnPropertyChanged("ConnectTextBoxContent");
            }
        }

        private ObservableCollection<object> _lpCommandsList;
        public ObservableCollection<object> LPCommandsList
        {
            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["LPCommands List"];
            }
            set
            {
                _lpCommandsList = value;
                OnPropertyChanged();
            }
        }

        //public ObservableCollection<object> DriverTypeList
        //{
        //    get
        //    {
        //        return Commands.GetInstance.EnumCommandsListbySubGroup["Driver Type"];
        //    }
        //}
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
        public static bool MotorOnOff_flag = false;
        private bool _motorOnToggleChecked = false;
        private int _ledMotorStatus;

        public bool MotorOnToggleChecked
        {
            get
            {
                return _motorOnToggleChecked;
            }
            set
            {
                if (_connetButtonContent == "Disconnect")
                {
                    _motorOnToggleChecked = value;
                    //Sent
                    if (!MotorOnOff_flag)
                    {
                        Rs232Interface.GetInstance.SendToParser(new PacketFields
                        {
                            Data2Send = _motorOnToggleChecked ? 1 : 0,
                            ID = Convert.ToInt16(1),
                            SubID = Convert.ToInt16(0),
                            IsSet = true,
                            IsFloat = false
                        });
                        Rs232Interface.GetInstance.SendToParser(new PacketFields
                        {
                            Data2Send = "",
                            ID = Convert.ToInt16(1),
                            SubID = Convert.ToInt16(0),
                            IsSet = false,
                            IsFloat = false
                        });
                    }
                    MotorOnOff_flag = false;
                    OnPropertyChanged("MotorONToggleChecked");
                }
            }
        }
        public int LedMotorStatus
        {
            get
            {
                return _ledMotorStatus;
            }
            set
            {
                if(value == 0)
                {
                    MotorOnOff_flag = true;
                    MotorOnToggleChecked = false;
                }
                else if(value == 1)
                {
                    MotorOnOff_flag = true;
                    MotorOnToggleChecked = true;
                }
                _ledMotorStatus = value;
                RaisePropertyChanged("LedMotorStatus");
            }
        }

#endregion

#endregion

#region TXRXLed
        private int _led_statusTx;
        public int LedStatusTx
        {
            get
            {
                return _led_statusTx;
            }
            set
            {
                _led_statusTx = value;
                RaisePropertyChanged("LedStatusTx");
                //Debug.WriteLine("Tx1: " + DateTime.Now.ToString("h:mm:ss.fff"));
                //Thread.SpinWait(10000);
                Thread.Sleep(1);
                //Debug.WriteLine("Tx2: " + DateTime.Now.ToString("h:mm:ss.fff"));
                if(value == 1)
                {
                    _led_statusTx = 0;
                    RaisePropertyChanged("LedStatusTx");
                }
            }
        }
        public void Instance_BlinkLedTx(object sender, EventArgs e)
        {
            //LedStatusTx = ((CustomEventArgs)e).LedTx;
        }

        private int _led_statusRx;

        public int LedStatusRx
        {
            get
            {
                return _led_statusRx;
            }
            set
            {
                _led_statusRx = value;
                RaisePropertyChanged("LedStatusRx");
                //Debug.WriteLine("Rx1: " + DateTime.Now.ToString("h:mm:ss.fff"));
                //Thread.SpinWait(10000);
                Thread.Sleep(1);
                //Debug.WriteLine("Rx2: " + DateTime.Now.ToString("h:mm:ss.fff"));
                if(value == 1)
                {
                    _led_statusRx = 0;
                    RaisePropertyChanged("LedStatusRx");
                }
            }
        }
#endregion TXRXLed

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
        public void AutoConnectCommand()
        {

            if (Rs232Interface.GetInstance.IsSynced == false)
            {
                lock (ConnectLock)
                {   
                    // Erase textboxs content, reset all default.
                    foreach(var element in Commands.GetInstance.DataViewCommandsList)
                    {
                        element.Value.CommandValue = "";
                    }
                    foreach(var element in Commands.GetInstance.CalibartionCommandsList)
                    {
                        element.Value.ButtonContent = "Run";
                        element.Value.CommandValue = "";
                    }
                    LogText = "";
                    Task task = new Task(Rs232Interface.GetInstance.AutoConnect);
                    task.Start();
                }
            }
            else
            {
                lock (ConnectLock)
                {
                    Task.Run((Action)Rs232Interface.GetInstance.Disconnect);
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

        public static bool flag; // Indicate the application is running and connected to a driver

        public static ParametarsWindow win;
        private void ShowParametersWindow()
        {
            //if (_connetButtonContent == "Disconnect")
            {
                if (ParametarsWindow.WindowsOpen != true)
                {
                    
                    win = ParametarsWindow.GetInstance; // new ParametarsWindow();
                    if(win.ActualHeight != 0)
                    {
                        win.Activate();
                    }
                    else
                    {
                        win.Show();
                    }
                    
                    flag = true;
                    //Task task = Task.Run((Action)BackGroundFunc);
                }
                else
                {
                    win.Activate();
                }
            }

        }
        public void Close_parmeterWindow()
        {
            win.Close();
            //flag = false;
        }

#if !DEBUG
        private bool _enRefresh = true;
#else
        private bool _enRefresh = false;
#endif
        public bool EnRefresh
        {
            get
            {
                return _enRefresh;
            }
            set
            {
                _enRefresh = value;
                OnPropertyChanged("EnRefresh");
                if(value && flag)
                {
                    Thread bkgnd = new Thread(BackGroundFunc);
                    bkgnd.Start();
                    //BackGroundFunc();
                }
                else if(!value)
                {
                    RefreshManger.DataPressed = false;
                    foreach(var list in Commands.GetInstance.DataViewCommandsList)
                    {
                        try
                        {
                            Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(list.Value.CommandId), Convert.ToInt16(list.Value.CommandSubId))].IsSelected = false;
                            Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(list.Value.CommandId), Convert.ToInt16(list.Value.CommandSubId))].Background2 = new SolidColorBrush(Colors.White);
                            Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(list.Value.CommandId), Convert.ToInt16(list.Value.CommandSubId))].Background = new SolidColorBrush(Colors.Gray);
                        }
                        catch(Exception e)
                        {
                        }
                    }
                }
            }
        }
        private void ClearLog()
        {
            LogText = "";
        }
        private async void BackGroundFunc()//object state)
        {
            //int count = 0;
            while (flag && LeftPanelViewModel.GetInstance.EnRefresh)// && count != 1
            {
                RefreshManger.GetInstance.StartRefresh();
                Thread.Sleep(1000);
                //await Task.Delay(1000);
                //count++;
            }
        }
#endregion
    }
}
