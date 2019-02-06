
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperButton.Models.DriverBlock;
using SuperButton.ViewModels;
using SuperButton.Helpers;
using SuperButton.Views;

namespace SuperButton.Models.ParserBlock
{

    internal delegate void PacketizerEventHandler(object sender, PacketizerEventArgs e);//Event declaration, when parser will finish operation. Rise event

    class Packetizer
    {
        public List<byte[]> StandartPacketsList = new List<byte[]>();
        public List<byte[]> PlotPacketsList = new List<byte[]>();
        public List<byte[]> StandartPacketsListNew = new List<byte[]>();
        // private  int DEBCount;
        private byte[] pack = new byte[11];
        private int plotpacketState = 0;
        private int standpacketState = 0;
        private int standpacketStateNew = 0;
        private int standpacketIndexCounter = 0;
        // private int plotpacketSize = 0;

        private int _synchAproveState;
        private int _synchAproveIndexCounter;
        // private int _synchAproveState = 0;
        byte[] readypacket = new byte[9];
        //Once created , could not be changed (READ ONLY)
        private static readonly object Synlock = new object(); //Single tone variable
        private static Packetizer _instance;               //Single tone variable

        public int length;
        public byte[] data;
        Int32 TempA = 0;

        public static readonly object Packetizerlock = new object(); //Single tone variable
        public event PacketizerEventHandler ToPacketizer;

        public static Packetizer GetInstance
        {
            get
            {
                lock (Synlock)
                {
                    if (_instance != null) return _instance;
                    _instance = new Packetizer();
                    return _instance;
                }
            }
        }

        public Packetizer()
        {
            Rs232Interface.GetInstance.Rx2Packetizer += MakePacketsBuff;
        }

        public void MakePacketsBuff(object sender, Rs232InterfaceEventArgs e)
        {
            
                if (sender is Rs232Interface) //RayonM3 Parser
                {
                    if (Rs232Interface.GetInstance.IsSynced)//Already Synchronized
                    {
                        if (e.DataChunk.Length == 0) return;

                        length = e.DataChunk.Length;
                        data = e.DataChunk;
                        for (int i = 0; i < length; i++)
                        {
                            FiilsPlotPackets(data[i]); //Plot packets
                            FiilsStandartPackets(data[i]);//Standart Packets                    
                            FiilsStandartPacketsNew(data[i]);//Standart Pavkets New Updeted
                        }
                        if (PlotPacketsList.Count > 0)
                        {
                            //EventRiser.Instance.RiseEventLedRx(RoundBoolLed.PASSED);
                            //Thread.Sleep(0);
                            ParserRayonM1.GetInstanceofParser.ParsePlot(PlotPacketsList);
                            //EventRiser.Instance.RiseEventLedRx(RoundBoolLed.IDLE);
                        }               //send to plot parser              
                        if (StandartPacketsListNew.Count > 0)
                        {
                            //EventRiser.Instance.RiseEventLedRx(RoundBoolLed.PASSED);
                            ParserRayonM1.GetInstanceofParser.ParseStandartData(StandartPacketsListNew);
                            StandartPacketsListNew.Clear(); // Joseph add
                            //EventRiser.Instance.RiseEventLedRx(RoundBoolLed.IDLE);

                        } //send to Standart parser                             
                    }
                    else
                    {
                        if (e.DataChunk.Length == 0) return;

                        PlotPacketsList.Clear();
                        StandartPacketsListNew.Clear();


                        length = e.DataChunk.Length;
                        data = e.DataChunk;
                        for (int i = 0; i < length; i++)
                        {
                            AproveSynchronization(data[i]); //Plot packets
                        }
                        if (StandartPacketsListNew.Count > 0)
                        {
                            ParserRayonM1.GetInstanceofParser.ParseSynchAcktData(StandartPacketsListNew);
                            StandartPacketsListNew.Clear();
                        }

                    }
                }
            
        }

        private void FiilsStandartPacketsNew(byte ch)
        {
            switch (standpacketStateNew)
            {
                case (0):	//First magic
                    if (ch == 0x8b)
                    { standpacketStateNew++; }
                    break;
                case (1)://Second magic
                    if (ch == 0x3c)
                    { standpacketStateNew++; }
                    else standpacketStateNew = 0;
                    break;
                case (2):
                case (3):
                case (4):
                case (5):
                case (6):
                case (7):
                case (8):
                case (9):
                    readypacket[(standpacketIndexCounter++)] = ch;
                    standpacketStateNew++;
                    break;
                case (10):
                    readypacket[standpacketIndexCounter] = ch;
                    StandartPacketsListNew.Add(readypacket);
                    standpacketStateNew = standpacketIndexCounter = 0;
                    break;
                default:
                    standpacketStateNew = standpacketIndexCounter = 0;
                    break;
            }
        }

        private void AproveSynchronization(byte ch)
        {
            switch (_synchAproveState)
            {
                case (0):	//First magic
                    if (ch == 0x8b)
                    { _synchAproveState++; }
                    break;
                case (1)://Second magic
                    if (ch == 0x3c)
                    { _synchAproveState++; }
                    else _synchAproveState = 0;
                    break;
                case (2):
                case (3):
                case (4):
                case (5):
                case (6):
                case (7):
                case (8):
                case (9):
                    readypacket[(_synchAproveIndexCounter++)] = ch;
                    _synchAproveState++;
                    break;
                case (10):
                    readypacket[_synchAproveIndexCounter] = ch;
                    StandartPacketsListNew.Add(readypacket);
                    _synchAproveState = _synchAproveIndexCounter = 0;
                    break;
                default:
                    _synchAproveState = _synchAproveIndexCounter = 0;
                    break;
            }
        }

        private void FiilsStandartPackets(byte ch)
        {

            switch (standpacketState)
            {
                case (0):	//First magic
                    if (ch == 0x8b)
                    {

                        // pack[plotpacketState] = ch;
                        standpacketState++;
                        TempA = 0;
                    }
                    break;
                case (1)://Second magic

                    if (ch == 0x3c)
                    {
                        pack[standpacketState] = ch;
                        standpacketState++;
                    }
                    else
                        standpacketState = 0;
                    break;
                case (2):
                    if (ch == 213)
                    {
                        standpacketState++;
                    }
                    else
                        standpacketState = 0;
                    break;
                case (3):
                    standpacketState++;
                    break;
                case (4):

                    standpacketState++;

                    break;
                case (5):
                    TempA = ch;

                    standpacketState++;

                    break;
                case (6):
                    TempA = TempA + Convert.ToInt16((ch << 8));

                    standpacketState++;
                    break;
                case (7):
                    TempA = TempA + Convert.ToInt16((ch << 16));

                    standpacketState++;
                    break;
                case (8):
                    TempA = TempA + Convert.ToInt16((ch << 24));
                    standpacketState = 0;
                    LeftPanelViewModel.ChankLen = TempA;
                    LeftPanelViewModel.mre.Set();
                    break;


                default:
                    standpacketState = 0;
                    break;
            }
        }

        private void FiilsPlotPackets(byte ch)
        {

            byte[] readypacket;


            switch (plotpacketState)
            {
                case (0):   //First magic
                    if (ch == 0xbb)
                    {

                        pack[plotpacketState] = ch;
                        plotpacketState++;

                    }
                    break;
                case (1)://Second magic

                    if (ch == 0xcc)
                    {
                        pack[plotpacketState] = ch;
                        plotpacketState++;
                    }
                    else
                        plotpacketState = 0;
                    break;
                case (2):
                    pack[plotpacketState] = ch;
                    plotpacketState++;
                    break;
                case (3):
                    pack[plotpacketState] = ch;
                    plotpacketState++;
                    break;
                case (4):
                    pack[plotpacketState] = ch;
                    plotpacketState++;

                    break;
                case (5):
                    pack[plotpacketState] = ch;
                    plotpacketState++;

                    break;
                case (6):

                    pack[plotpacketState] = ch;
                    plotpacketState++;
                    break;
                case (7):

                    pack[plotpacketState] = ch;
                    plotpacketState++;
                    break;
                case (8):

                    pack[plotpacketState] = ch;
                    plotpacketState++;
                    break;
                case (9):

                    pack[plotpacketState] = ch;
                    plotpacketState++;
                    break;
                case (10)://CheckSum

                    byte chechSum = 0;

                    chechSum += pack[2];
                    chechSum += pack[3];
                    chechSum += pack[4];
                    chechSum += pack[5];
                    chechSum += pack[6];
                    chechSum += pack[7];
                    chechSum += pack[8];
                    chechSum += pack[9];

                    if (chechSum == ch)
                    {
                        pack[plotpacketState] = ch;
                        readypacket = new byte[11];
                        Array.Copy(pack, 0, readypacket, 0, 11);
                        PlotPacketsList.Add(readypacket);
                    }
                    else
                    {
                        readypacket = new byte[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                        PlotPacketsList.Add(readypacket);
                    }

                    plotpacketState = 0;
                    break;

                default:
                    plotpacketState = 0;
                    break;
            }
        }




    }
}



