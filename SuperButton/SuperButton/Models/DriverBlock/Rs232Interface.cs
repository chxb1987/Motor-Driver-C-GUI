using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Example.Common;
using Abt.Controls.SciChart.Example.Data;
using SuperButton.CommandsDB;
using SuperButton.Data;
using SuperButton.Models.DriverBlock;
using SuperButton.Models.ParserBlock;
using SuperButton.Models.SataticClaass;
using SuperButton.ViewModels;
using SuperButton.Views;
using System.Globalization;
using SuperButton.Common;
using System.Windows;
using SuperButton.Helpers;
using System.ComponentModel;
using SuperButton.Annotations;

namespace SuperButton.Models.DriverBlock
{

    internal delegate void Rs232RxHandler(object sender, Rs232InterfaceEventArgs e);


    internal class Rs232Interface : ConnectionBase
    {
        //TOdo move global members to base clase, and create there ctor (lesson interfase)
        //MEMBERS
        public event Rs232RxHandler RxtoParser;
        public event Rs232RxHandler TxtoParser;
        public event Rs232RxHandler Driver2Mainmodel;

        public event Rs232RxHandler Rx2Packetizer;
        // public  Task TsakRec;
        // public Task Child;

        public PacketFields RxPacket;
        public static SerialPort _comPort;         //Serial Port

        //Once created , could not be changed (READ ONLY)
        private static readonly object Synlock = new object(); //Single tone variable
        private static readonly object ConnectLock = new object(); //Single tone variable
        private static readonly object DisonnectLock = new object(); //Single tone variable
        private static Rs232Interface _instance;               //Single tone variable


        private static bool _isSynced = false;                    //Sincronization flag
        private static readonly object Sendlock = new object();   //Semapophor
        private static List<ConnectionBase.ComDevice> _comDevicesList = new List<ComDevice>();


        //private int _bytes2Read;
        //private DoubleSeries datasource1 = new DoubleSeries();
        // XYPoint[] xyPointBuff = new XYPoint[200];
        //private UInt16 Counter;
        // private byte[] buffer = new byte[8192];  



        //TODO make this property abstract
        #region Properties
        //public int Bytes2Read
        //{
        //    get
        //    {
        //        return _bytes2Read;
        //    }
        //    set
        //    {
        //        _bytes2Read = value;
        //    }
        //}

        public bool IsSynced
        {
            get
            {
                return _isSynced;
            }
            set
            {
                _isSynced = value;
            }
        }

        #endregion



        public delegate void DataRecived(byte[] dataBytes);
        //Defining event based on the above delegate
        public event DataRecived DataRecivedEvent;

        //Contsractor

        public Rs232Interface()
        {
            //Create queue object and set queue size
            GuiUpdateQueue stundartUpdateQueue = new GuiUpdateQueue();

            //for (int i = 0; i < 200; i++)
            //{
            //    xyPointBuff[i]=new XYPoint();
            //}

            //Counter = 0;


            //ParserRayonM1.GetInstanceofParser.Parser2Send += SendDataHendler;
        }
        public override void Disconnect()
        {
            if (_comPort.IsOpen)
            {
                if (RxtoParser != null)
                {
                    _isSynced = false;
                    Thread.Sleep(100);
                    DataViewModel temp = (DataViewModel)Commands.GetInstance.DataCommandsListbySubGroup["DeviceSynchCommand"][0];
                    Commands.AssemblePacket(out RxPacket, Int16.Parse(temp.CommandId), Int16.Parse(temp.CommandSubId), true, false, 0);
                    RxtoParser(this, new Rs232InterfaceEventArgs(RxPacket));

                    ParserRayonM1.mre.WaitOne(1000);

                    if (Rs232Interface.GetInstance.IsSynced == false)
                    {

                        _comPort.DataReceived -= DataReceived;
                        _comPort.Close();
                        _comPort.Dispose();

                        if (Driver2Mainmodel != null)
                        {
                            Driver2Mainmodel(this, new Rs232InterfaceEventArgs("Connect"));
                        }
                        else
                        {
                            throw new NullReferenceException("No Listeners to this event");
                        }
                    }
                }

            }
            else
            {
                _isSynced = false;
                _comPort.DataReceived -= DataReceived;
                _comPort.Close();
                _comPort.Dispose();
                Driver2Mainmodel(this, new Rs232InterfaceEventArgs("Connect"));

            }
            LeftPanelViewModel.GetInstance.EnRefresh = false;
        }

        #region Auto_Connect

        //This method auto detects baud rate, and open connection
        //
        //
        //
        //
        //
        //********************************************************
        string msg = "";
        public override void AutoConnect()
        {
            if (_isSynced == false) //Driver is not synchronized
            {
                //Gets aviable ports list and initates them
                _comDevicesList =
                    (SerialPort.GetPortNames()).Select(o => new ComDevice { Portname = o, Baudrate = 921600 }).ToList();

                //Iterates though  the ports,Looks for apropriate Com Port ( where the driver connected)
                if (Configuration.SelectedCom != null && Configuration.SelectedCom != "")//foreach (var comDevice in _comDevicesList)
                {
                    // Add text to logger panel
                    EventRiser.Instance.RiseEevent(string.Format($"Try to establish connection with the driver at {Configuration.SelectedCom}"));
                    var tmpcom = new SerialPort
                    {
                        PortName = Configuration.SelectedCom,
                        DataBits = 0x00000008,
                        StopBits = System.IO.Ports.StopBits.One,
                        ReadBufferSize = 8192,
                        ReadTimeout = 10
                    };
                    try
                    {
                        tmpcom.Open(); //Try to open

                        if (tmpcom.IsOpen)
                        {
                            foreach (var baudRate in BaudRates) //Iterate though baud rates
                            {

                                if (_isSynced)
                                {
                                    if (Driver2Mainmodel != null)
                                    {
                                        Driver2Mainmodel(this, new Rs232InterfaceEventArgs("Disconnect"));
                                    }
                                    else
                                    {
                                        throw new NullReferenceException("No Listeners on this event");
                                    }
                                    _comPort.DiscardInBuffer();        //Reset internal rx buffer
                                    return;

                                }
                                else if (_isSynced == false)  //open task
                                {
                                    tmpcom.BaudRate = baudRate;

                                    tmpcom.DataReceived -= DataReceived;
                                    tmpcom.DataReceived += DataReceived;

                                    ParserRayonM1.GetInstanceofParser.Parser2Send -= SendDataHendler;
                                    ParserRayonM1.GetInstanceofParser.Parser2Send += SendDataHendler;
                                    _comPort = tmpcom;

                                    //Init synchronization packet, and rises event for parser
                                    if (RxtoParser != null)
                                    {
                                        DataViewModel temp = (DataViewModel)Commands.GetInstance.DataCommandsListbySubGroup["DeviceSynchCommand"][0];
                                        Commands.AssemblePacket(out RxPacket, Int16.Parse(temp.CommandId), Int16.Parse(temp.CommandSubId), true, false, 1);
                                        RxtoParser(this, new Rs232InterfaceEventArgs(RxPacket));
                                    }
                                    Thread.Sleep(100);// while with timeout of 1 second

                                    var Cleaner = tmpcom.ReadExisting();
                                }

                            }

                        }
                        tmpcom.Close();
                    }
                    catch (Exception)
                    {
                        tmpcom.Close();
                        tmpcom.Dispose();
                        return;// false;
                    }
                }
                else
                {
                    if (!MessageBoxWrapper.IsOpen)
                    {
                        msg = string.Format("No COM Port Selected!");
                        MessageBoxWrapper.Show(msg, "");
                    }
                }
            }
            else if (_isSynced == true)
                return;// true;

            return;// false;
        }

        #endregion

        #region Manual_Connect

        //This method manually and open connection
        //
        //
        //
        //
        //
        //********************************************************

        public virtual bool ManualConnect()
        {

            if (_isSynced == false) //Driver is not synchronized
            {
                //Gets aviable ports list and initates them
                _comDevicesList =
                    (SerialPort.GetPortNames()).Select(o => new ComDevice { Portname = o, Baudrate = 921600 }).ToList();

                //Iterates though  the ports,Looks for apropriate Com Port ( where the driver connected)
                foreach (var comDevice in _comDevicesList)
                {
                    var tmpcom = new SerialPort
                    {
                        PortName = comDevice.Portname,
                        DataBits = comDevice.DataBits,
                        StopBits = comDevice.StopBits
                    };
                    try
                    {
                        tmpcom.Open(); //Try to open
                        if (tmpcom.IsOpen)
                        {
                            foreach (var baudRate in BaudRates) //Iterate though baud rates
                            {
                                tmpcom.BaudRate = baudRate;
                                //tmpcom.DataReceived += SyncDataReceived;

                                //Moves to parser block
                                //ProtocolParser.GetInstance.BuildPacketToSend("0", "400" /*CommandId*/, "0" /* subid*/,
                                //    true /*IsSet*/);

                                Thread.Sleep(50);
                                // tmpcom.DataReceived -= SyncDataReceived;

                                if (_isSynced)
                                {
                                    return true;
                                }
                            }
                        }
                        tmpcom.Close();
                    }
                    catch (Exception)
                    {
                        tmpcom.Close();
                        tmpcom.Dispose();
                        return false;
                    }
                }
            }
            return _isSynced;
        }
        #endregion

        #region Send_Mechanism

        //     public void SendData(byte[] packetToSend)
        //     SendDataHendler(object sender, Parser2SendEventArgs parser2SendEventArgs)
        //
        //
        //
        //
        //********************************************************
        public void SendDataHendler(object sender, Parser2SendEventArgs parser2SendEventArgs)
        {
            //Task.Factory.StartNew(action: () =>
            //{
            SendData(parser2SendEventArgs.BytesTosend, _comPort);
            //});
        }


        private void SendData(byte[] packetToSend, object comPort)
        {
            lock (Sendlock)
            {
                //EventRiser.Instance.RiseEventLedTx(RoundBoolLed.PASSED);
                var serialPort = comPort as SerialPort;
                if (serialPort != null && serialPort.IsOpen)
                    serialPort.Write(packetToSend, 0, packetToSend.Length); // Send through RS232 cable
                var port = comPort as SerialPort;
                if (port != null) port.DiscardOutBuffer();
                //EventRiser.Instance.RiseEventLedTx(RoundBoolLed.IDLE);
                LeftPanelViewModel.GetInstance.LedStatusTx = RoundBoolLed.PASSED;
            }
            LeftPanelViewModel.GetInstance.LedStatusTx = RoundBoolLed.IDLE;
        }

        public void SendToParser(PacketFields messege)
        {
            //if (RxtoParser != null)
            //{
            //    //rise event
            //    //if (messege.ID == 82 && messege.SubID == 1 && messege.IsSet == true)
            //    //{ /*int i = 0; */}
            //    // messege.Data2Send = "0.5";


            //    RxtoParser(this, new Rs232InterfaceEventArgs(messege));
            //    //Debug.WriteLine("SendToDriver=> Data: {0}, ID: {1}, isFloat: {2}, isSet: {3}, SubID: {4}.", messege.Data2Send, messege.ID, messege.IsFloat, messege.IsSet, messege.SubID);
            //    //Debug.WriteLine("{0} {1}[{2}]={3} {4}.", messege.IsSet ? "Set" : "Get", messege.ID, messege.SubID, messege.Data2Send, messege.IsFloat ? "F" : "I");
            //    //EventRiser.Instance.RiseEevent(String.Format("{0} {1}[{2}]={3} {4}", messege.IsSet ? "Set" : "Get", messege.ID, messege.SubID, messege.Data2Send, messege.IsFloat ? "F" : "I"));
            //    //try
            //    //{
            //    //    foreach (var List in RefreshManger.BuildList)
            //    //    {
            //    //        if (List.Value.CommandId.ToString() == messege.ID.ToString())
            //    //        {
            //    //            if (messege.IsSet)
            //    //                EventRiser.Instance.RiseEevent("Tx: Set " + List.Value.CommandName.ToString() + ' ' + messege.ID + '[' + messege.SubID + "]=" + messege.Data2Send + " (" + (messege.IsFloat ? ".0" : "") + ')');
            //    //            else
            //    //                EventRiser.Instance.RiseEevent("Tx: Get " + List.Value.CommandName.ToString() + ' ' + messege.ID + '[' + messege.SubID + "]");
            //    //        }
            //    //    }
            //    //}
            //    //catch { }

            //}
            RxtoParser?.Invoke(this, new Rs232InterfaceEventArgs(messege));
            Debug.WriteLine("{0} {1}[{2}]={3} {4}.", messege.IsSet ? "Set" : "Get", messege.ID, messege.SubID, messege.Data2Send, messege.IsFloat ? "F" : "I");
        }

        #endregion

        #region Read_Mechanism


        // public void ReadDataEventHandler(byte[] packetToSend)
        //     
        //
        // read Data will read always two byte and send to parser 
        // parcer will check preemble and know how mane data fields it should get
        //
        //
        //
        //********************************************************

        public void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Create new parent Task
            // Task.Factory.StartNew(() => ProcessRecievedData( sender,e));
            SerialPort port = (SerialPort)sender;
            if (port != null)
            {
                byte[] buffer = new byte[port.BytesToRead];
                port.Read(buffer, 0, buffer.Length);

                if (Rx2Packetizer != null && buffer.Length > 0)
                {
                    Rx2Packetizer(this, new Rs232InterfaceEventArgs(buffer));
                    LeftPanelViewModel.GetInstance.LedStatusRx = RoundBoolLed.PASSED;
                }
                LeftPanelViewModel.GetInstance.LedStatusRx = RoundBoolLed.IDLE;
            }
        }
        public void Connect()
        {
            _comPort.DataReceived += DataReceived;
        }

        #endregion

        //STATIC METHODS  STATIC METHODS   STATIC METHODS   STATIC METHODS   STATIC METHODS   STATIC METHODS   STATIC METHODS   STATIC METHODS   STATIC METHODS   STATIC METHODS    

        public static Rs232Interface GetInstance
        {
            get
            {
                lock (Synlock)
                {
                    if (_instance != null) return _instance;
                    _instance = new Rs232Interface();
                    return _instance;
                }
            }
        }
    }
}

