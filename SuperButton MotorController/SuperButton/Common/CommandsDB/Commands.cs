using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using SuperButton.ViewModels;
using System.Windows.Media;

namespace SuperButton.CommandsDB
{
    class Commands
    {
        private Commands()
        {
            GenerateMotionCommands();
            GenerateFeedBakcCommands();
            GeneratePidCommands();
            GenerateDeviceCommands();
            //GenerateDriverCommands();
            GenerateMainWindowCommands();
            GenerateBPCommands();
            //GenerateCalCommands();
            GenerateLPCommands();
            GenerateMotionTabCommands();
            GenerateMaintenanceList();
            UpperMainPannelList();
            CalibrationCmd();
            BuildErrorList();
            GenerateGain();
            GenerateDebugListCommands();
        }

        static public void AssemblePacket(out PacketFields rxPacket, Int16 id, Int16 subId, bool isSet, bool isFloat, object data2Send)
        {
            if(id == 81 && subId == 1 && isSet == true)
            { /*int i = 0;*/ }
            rxPacket.ID = id;
            rxPacket.IsFloat = isFloat;
            rxPacket.IsSet = isSet;
            rxPacket.SubID = subId;
            rxPacket.Data2Send = data2Send;
        }

        public Dictionary<Tuple<int, int>, DataViewModel> DataViewCommandsList = new Dictionary<Tuple<int, int>, DataViewModel>();
        public Dictionary<Tuple<int, int>, DataViewModel> DataViewCommandsListLP = new Dictionary<Tuple<int, int>, DataViewModel>();
        public Dictionary<Tuple<int, int>, EnumViewModel> EnumViewCommandsList = new Dictionary<Tuple<int, int>, EnumViewModel>();
        public Dictionary<Tuple<int, int>, CalibrationButtonModel> CalibartionCommandsList = new Dictionary<Tuple<int, int>, CalibrationButtonModel>();
        public Dictionary<Tuple<int, int>, DebugObjViewModel> DebugList = new Dictionary<Tuple<int, int>, DebugObjViewModel>();


        public Dictionary<string, List<string>> Enums = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> EnumsQep1 = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> EnumsQep2 = new Dictionary<string, List<string>>();

        public Dictionary<string, ObservableCollection<object>> EnumCommandsListbySubGroup = new Dictionary<string, ObservableCollection<object>>();
        public Dictionary<string, ObservableCollection<object>> DataCommandsListbySubGroup = new Dictionary<string, ObservableCollection<object>>();
        public Dictionary<string, ObservableCollection<object>> CalibartionCommandsListbySubGroup = new Dictionary<string, ObservableCollection<object>>();
        public Dictionary<string, ObservableCollection<object>> DebugListbySubGroup = new Dictionary<string, ObservableCollection<object>>();
       
        public Dictionary<int, string> ErrorList = new Dictionary<int, string>();
        public Dictionary<string, NumericTextboxModel> Gain = new Dictionary<string, NumericTextboxModel>();
        public Dictionary<string, ObservableCollection<object>> GainList = new Dictionary<string, ObservableCollection<object>>();
        public Dictionary<Tuple<int, int>, BoolViewIndModel> DigitalInputList = new Dictionary<Tuple<int, int>, BoolViewIndModel>();
        public Dictionary<string, ObservableCollection<object>> DigitalInputListbySubGroup = new Dictionary<string, ObservableCollection<object>>();

        private static readonly object Synlock = new object(); //Single tone variable
        private static Commands _instance;
        public float _pidcurr;
        public float Pidcurr
        {
            get { return _pidcurr; }
            set
            {
                if(_pidcurr == value)
                    return;
                _pidcurr = value;

            }
        }
        public static Commands GetInstance
        {
            get
            {
                lock(Synlock)
                {
                    if(_instance != null)
                        return _instance;
                    _instance = new Commands();
                    return _instance;
                }
            }
        }

        private void GenerateMainWindowCommands()
        {
            DataViewModel temp = new DataViewModel
            {
                CommandId = "100",
                CommandSubId = "0",
                CommandName = "DriveOn",
                IsFloat = false,
                CommandValue = "0"
            };

            //DataViewCommandsList.Add(new Tuple<int, int>(400, 0), temp);
        }
        private void GenerateDriverCommands()
        {
            DataCommandsListbySubGroup.Add("DriverFullScale", new ObservableCollection<object>());

            var names = new[]
            {
                "Current", "V-Bus", "BEMF"
            };


            for(int i = 0; i < names.Length; i++)
            {
                var data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "130",
                    CommandSubId = i.ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = true,
                };

                DataViewCommandsList.Add(new Tuple<int, int>(130, i), data);
                DataCommandsListbySubGroup["DriverFullScale"].Add(data);
            }
        }
        private void GenerateDeviceCommands()
        {
            DataCommandsListbySubGroup.Add("DeviceSerial", new ObservableCollection<object>());

            var names = new[]
            {
                "Serial Number", "Hardware Rev", "CanNode ID"//,"System ID"
            };
            for(int i = 0, k = 1; i < names.Length; i++, k++)
            {
                var data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "62",
                    CommandSubId = k.ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                };

                DataViewCommandsList.Add(new Tuple<int, int>(62, k), data);
                DataCommandsListbySubGroup["DeviceSerial"].Add(data);
                if(i == 1)
                    k = 7;
            }

            var BR = new List<string>
              {
                  "4800",
                  "9600",
                  "19200",
                  "38400",
                  "57600",
                  "115200",
                  "230400",
                  "460800",
                  "921600"
              };
            Enums.Add("Baudrate", BR);

            var BR_Enum = new EnumViewModel
            {
                CommandName = "Baudrate",
                CommandId = "61",
                CommandSubId = "1",
                CommandList = Enums["Baudrate"],
                CommandValue = "0",//first enum in list
            };

            EnumViewCommandsList.Add(new Tuple<int, int>(61, 1), BR_Enum);
            EnumCommandsListbySubGroup.Add("BaudrateList", new ObservableCollection<object> { BR_Enum });
            #region Synch Command cmdID 64 cmdSubID 0

            DataCommandsListbySubGroup.Add("DeviceSynchCommand", new ObservableCollection<object>());
            DataViewModel temp = new DataViewModel
            {
                CommandId = "64",
                CommandSubId = "0",
                CommandName = "Synchcmd",
                IsFloat = false,
                CommandValue = "0"
            };
            DataCommandsListbySubGroup["DeviceSynchCommand"].Add(temp);

            #endregion

        }
        private void GeneratePidCommands()
        {
            var names = new[]
            {
                "Kp", "Ki"//, "Kc"
            };

            DataCommandsListbySubGroup.Add("PIDCurrent", new ObservableCollection<object>());
            DataCommandsListbySubGroup.Add("PIDSpeed", new ObservableCollection<object>());
            DataCommandsListbySubGroup.Add("PIDPosition", new ObservableCollection<object>());

            for(int i = 0; i < names.Length; i++)
            {
                var data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "81",
                    CommandSubId = (i + 1).ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = true
                };
                DataViewCommandsList.Add(new Tuple<int, int>(81, (i + 1)), data);
                DataCommandsListbySubGroup["PIDCurrent"].Add(data);



                data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "82",
                    CommandSubId = (i + 1).ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = true
                };
                DataViewCommandsList.Add(new Tuple<int, int>(82, (i + 1)), data);
                DataCommandsListbySubGroup["PIDSpeed"].Add(data);


                data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "83",
                    CommandSubId = (i + 1).ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = true
                };
                DataViewCommandsList.Add(new Tuple<int, int>(83, (i + 1)), data);
                DataCommandsListbySubGroup["PIDPosition"].Add(data);
            }
        }
        private void GenerateFeedBakcCommands()
        {
            #region Hall_SSI
            var names = new[]
            {
                "Enable", "Roll UP", "Sample Period",
                "Direction", "Counts Per Rev", "Speed LPF Cut-Off", "Index Mode"
            };

            DataCommandsListbySubGroup.Add("Hall", new ObservableCollection<object>());
            DataCommandsListbySubGroup.Add("Qep1", new ObservableCollection<object>());
            DataCommandsListbySubGroup.Add("Qep2", new ObservableCollection<object>());
            DataCommandsListbySubGroup.Add("SSI_Feedback", new ObservableCollection<object>());

            for(var i = 0; i < names.Length; i++)
            {
                var data = new DataViewModel();
                if(i != 6)
                {
                    data = new DataViewModel
                    {
                        CommandName = names[i],
                        CommandId = "70",
                        CommandSubId = (i + 1).ToString(CultureInfo.InvariantCulture),
                        CommandValue = "",
                        IsFloat = names[i] == "Speed LPF Cut-Off"
                    };
                    DataViewCommandsList.Add(new Tuple<int, int>(70, i + 1), data);
                    DataCommandsListbySubGroup["Hall"].Add(data);
                }

                if(i >= 6)
                    continue;

                data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "73",
                    CommandSubId = i.ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                };
                DataViewCommandsList.Add(new Tuple<int, int>(73, i), data);
                if(i != 0)
                    DataCommandsListbySubGroup["SSI_Feedback"].Add(data);
            }

            for(int i = 1; i < 5; i++)
            {
                var data = new DataViewModel
                {
                    CommandName = "Hall Angle " + i.ToString(),
                    CommandId = "70",
                    CommandSubId = (i + 6).ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = false
                };
                DataViewCommandsList.Add(new Tuple<int, int>(70, i + 6), data);
                DataCommandsListbySubGroup["Hall"].Add(data);
            }

            for(int i = 0; i < 6; i++)
            {
                var data = new DataViewModel
                {
                    CommandName = "Hall Map" + i.ToString(),
                    CommandId = "84",
                    CommandSubId = (i + 1).ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = false
                };
                DataViewCommandsList.Add(new Tuple<int, int>(84, (i + 1)), data);
                DataCommandsListbySubGroup["Hall"].Add(data);
            }

            #endregion Hall_SSI
            #region Qep1Qep2
            names = new[]
            {
                "Enable", "Roll High", "Roll Low", "Direction", "Counts Per Rev",
                "Speed LPF", "Index Mode", "Reset Value", "Set Position Value"
            };
            bool[] IsFloat = new[] { false, false, false, false, false, true, false, false, false };
            for(int i = 0, k = 1; i < names.Length; i++, k++)
            {
                var data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "71",
                    CommandSubId = k.ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = IsFloat[i],
                };
                DataViewCommandsList.Add(new Tuple<int, int>(71, k), data);
                DataCommandsListbySubGroup["Qep1"].Add(data);

                data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "72",
                    CommandSubId = k.ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = IsFloat[i],
                };
                DataViewCommandsList.Add(new Tuple<int, int>(72, k), data);
                DataCommandsListbySubGroup["Qep2"].Add(data);

                if(k == 7)
                    k = 8;
                if(k == 9)
                    k = 12;
            }

            var dataB = new DataViewModel
            {
                CommandName = "Resolution Sin/Cos",
                CommandId = "71",
                CommandSubId = 14.ToString(CultureInfo.InvariantCulture),
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(71, 14), dataB);
            DataCommandsListbySubGroup["Qep1"].Add(dataB);

            //Qep1FdBckList Qep1Bis
            var tmp1 = new List<string>
              {
                  "Index Disabled",
                  "One Shot",
                  "Continuous Refresh"
              };
            EnumsQep1.Add("Index Reset", tmp1);

            var enum1 = new EnumViewModel
            {
                CommandName = "Index Reset",
                CommandId = "71",
                CommandSubId = "8",
                CommandList = EnumsQep1["Index Reset"],
                CommandValue = "1",//first enum in list
                IsFloat = false,
                //SelectedValue = "0",
            };

            EnumViewCommandsList.Add(new Tuple<int, int>(71, 8), enum1);

            EnumCommandsListbySubGroup.Add("Qep1Bis", new ObservableCollection<object>
            {
              enum1
            });

            var tmp2 = new List<string>
              {
                  "Index Disabled",
                  "One Shot",
                  "Continuous Refresh"
              };
            EnumsQep2.Add("Index Reset", tmp2);

            var enum2 = new EnumViewModel
            {
                CommandName = "Index Reset",
                CommandId = "72",
                CommandSubId = "8",
                CommandList = EnumsQep2["Index Reset"],
                CommandValue = "1",//first enum in list
                IsFloat = false,
                //SelectedValue = "0",
            };

            EnumViewCommandsList.Add(new Tuple<int, int>(72, 8), enum2);

            EnumCommandsListbySubGroup.Add("Qep2Bis", new ObservableCollection<object>
            {
              enum2
            });
            #endregion  Qep1Qep2
        }
        public void GenerateMotionCommands()
        {
            var tmp1 = new List<string>
              {
                  "Current Control",
                  "Speed Control",
                  "Position Control",
                  "ST Speed Control",
                  "ST Position Time Control",
                  "ST Position Control"
              };
            Enums.Add("Drive Mode", tmp1);

            var enum1 = new EnumViewModel
            {
                CommandName = "Drive Mode",
                CommandId = "50",
                CommandSubId = "1",
                CommandList = Enums["Drive Mode"],
                CommandValue = "1",//first enum in list
            };

            EnumViewCommandsList.Add(new Tuple<int, int>(50, 1), enum1);

            EnumCommandsListbySubGroup.Add("Control", new ObservableCollection<object>
            {
              enum1
            });

            var tmp2 = new List<string>
             {
                 //"Cmtn Hall",
                 //"Cmtn Qep1",
                 //"Cmtn Qep2",
                 //"Cmtn Hall And Qep1",
                 //"Cmtn Hall And Qep2",
                 //"Cmtn SSI Abs",
                 //"Cmtn Forced",
                 //"Cmtn SSI Abs",
                 //"Cmtn DC Brushed",
                 //"Cmtn SSI Inc",
                 //"Cmtn Src Analog",
                 //"Cmtn Src Sin Cos",
                 //"Cmtn SensorLess"

                 "Cmtn_Forced_Rotate",
                 "Cmtn_Hall",
                 "Cmtn_Enc1",
                 "Cmtn_Abs_Enc1",
                 "Cmtn_Hall_Inc_Enc1",
                 "Cmtn_Enc2",
                 "Cmtn_Abs_Enc2",
                 "Cmtn_Hall_Inc_Enc2",
                 "Cmtn_DC_Brushed",
                 "Cmtn_SensorLess",
                 "Cmtn_Set_angle"
             };
            Enums.Add("Commutation Source", tmp2);


            var enum2 = new EnumViewModel
            {
                CommandName = "Commutation Source",
                CommandId = "50",
                CommandSubId = "2",
                CommandValue = "1", //first enum in list
                CommandList = Enums["Commutation Source"]
            };
            EnumViewCommandsList.Add(new Tuple<int, int>(50, 2), enum2);
            EnumCommandsListbySubGroup["Control"].Add(enum2);

            var tmp3 = new List<string>
             {
                 "Cla_Fdb_Hall",
                 "Cla_Fdb_Enc1",
                 "Cla_Fdb_Enc2"
             };

            Enums.Add("Speed Fdb Source", tmp3);
            var enum3 = new EnumViewModel
            {
                CommandName = "Speed Fdb Source",
                CommandId = "50",
                CommandSubId = "3",
                CommandValue = "0", //first enum in list
                CommandList = Enums["Speed Fdb Source"]
            };
            EnumViewCommandsList.Add(new Tuple<int, int>(50, 3), enum3);
            EnumCommandsListbySubGroup["Control"].Add(enum3);

            var tmp4 = new List<string>
             {
                 "Cla_Fdb_Hall",
                 "Cla_Fdb_Enc1",
                 "Cla_Fdb_Enc2"
             };
            Enums.Add("Position Fdb Source", tmp4);
            var enum4 = new EnumViewModel
            {
                CommandName = "Position Fdb Source",
                CommandId = "50",
                CommandSubId = "4",
                CommandValue = "0", //first enum in list
                CommandList = Enums["Position Fdb Source"]
            };
            EnumViewCommandsList.Add(new Tuple<int, int>(50, 4), enum4);
            EnumCommandsListbySubGroup["Control"].Add(enum4);

            var tmp5 = new List<string>
             {
                "Digital_Cmd",
                "Analog_Cmd",
                "PWM_Cmd",
                "Buffer_Cmd",
                "Spi_Cmd",
                "Signal_gen_Cmd"
            };
            Enums.Add("Command Source", tmp5);
            var enum5 = new EnumViewModel
            {
                CommandName = "Command Source",
                CommandId = "50",
                CommandSubId = "5",
                CommandValue = "1", //first enum in list
                CommandList = Enums["Command Source"]
            };
            EnumViewCommandsList.Add(new Tuple<int, int>(50, 5), enum5);
            EnumCommandsListbySubGroup["Control"].Add(enum5);

            var data1 = new DataViewModel
            {
                CommandName = "Pole Pair",
                CommandId = "51",
                CommandSubId = "1",
                CommandValue = "",
                IsFloat = false,
            };
            DataCommandsListbySubGroup.Add("Motor", new ObservableCollection<object> { data1 });
            DataViewCommandsList.Add(new Tuple<int, int>(51, 1), data1);


            var data2 = new DataViewModel
            {
                CommandName = "Direction",
                CommandId = "51",
                CommandSubId = "2",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(51, 2), data2);
            DataCommandsListbySubGroup["Motor"].Add(data2);

            var data3 = new DataViewModel
            {
                CommandName = "Max speed [CPS]",
                CommandId = "53",
                CommandSubId = "1",
                CommandValue = "",
                IsFloat = false
            };
            DataViewCommandsList.Add(new Tuple<int, int>(53, 1), data3);
            DataCommandsListbySubGroup.Add("Motion Limit", new ObservableCollection<object> { data3 });

            var data4 = new DataViewModel
            {
                CommandName = "Min Speed [CPS]",
                CommandId = "53",
                CommandSubId = "2",
                CommandValue = "",
                IsFloat = false
            };

            DataViewCommandsList.Add(new Tuple<int, int>(53, 2), data4);
            DataCommandsListbySubGroup["Motion Limit"].Add(data4);

            var data5 = new DataViewModel
            {
                CommandName = "Max position [C]",
                CommandId = "53",// "205",
                CommandSubId = "3",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(53, 3), data5);
            DataCommandsListbySubGroup["Motion Limit"].Add(data5);

            var data6 = new DataViewModel
            {
                CommandName = "Min position [C]",
                CommandId = "53",
                CommandSubId = "4",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(53, 4), data6);
            DataCommandsListbySubGroup["Motion Limit"].Add(data6);
        }
        public void GenerateBPCommands()
        {
            #region Commands1
            DataCommandsListbySubGroup.Add("MotionCommand List", new ObservableCollection<object>());
            DataCommandsListbySubGroup.Add("MotionCommand List2", new ObservableCollection<object>());

            var data = new DataViewModel
            {
                CommandName = "Current [A]",
                CommandId = "3",
                CommandSubId = "0",
                CommandValue = "",
                IsFloat = true,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(3, 0), data);
            DataCommandsListbySubGroup["MotionCommand List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Speed [C/S]",
                CommandId = "4",
                CommandSubId = "0",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(4, 0), data);
            DataCommandsListbySubGroup["MotionCommand List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "RPM",
                CommandId = "4",
                CommandSubId = "10",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(4, 10), data);
            DataCommandsListbySubGroup["MotionCommand List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Speed Position [C/S]",
                CommandId = "5",
                CommandSubId = "2",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(5, 2), data);
            DataCommandsListbySubGroup["MotionCommand List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Position Absolute [C]",
                CommandId = "5",
                CommandSubId = "0",
                CommandValue = "",
                IsFloat = false,
                IsSelected = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(5, 0), data);
            DataCommandsListbySubGroup["MotionCommand List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Position Relative [C]",
                CommandId = "5",
                CommandSubId = "1",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(5, 1), data);
            DataCommandsListbySubGroup["MotionCommand List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Accelaration [C/S^2]",
                CommandId = "54",
                CommandSubId = "3",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(54, 3), data);
            DataCommandsListbySubGroup["MotionCommand List2"].Add(data);

            data = new DataViewModel
            {
                CommandName = "PTP Speed [C/S]",
                CommandId = "54",
                CommandSubId = "2",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(54, 2), data);
            DataCommandsListbySubGroup["MotionCommand List2"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Max Tracking Err [C]",
                CommandId = "54",
                CommandSubId = "6",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(54, 6), data);
            DataCommandsListbySubGroup["MotionCommand List2"].Add(data);
            #endregion Commands1
            #region Commands2
            var ProfilerModeEnum = new List<string>
              {
                  "PID",
                  "Trapezoid"
              };
            Enums.Add("Profiler Mode", ProfilerModeEnum);

            var ProfilerModeCmd = new EnumViewModel
            {
                CommandName = "Profiler Mode",
                CommandId = "54",
                CommandSubId = "1",
                CommandList = Enums["Profiler Mode"],
                CommandValue = "1",//first enum in list
            };
            //DataViewCommandsList.Add(new Tuple<int, int>(54, 1), ProfilerModeCmd);
            EnumViewCommandsList.Add(new Tuple<int, int>(54, 1), ProfilerModeCmd);
            EnumCommandsListbySubGroup.Add("Profiler Mode", new ObservableCollection<object>
            {
              ProfilerModeCmd
            });
            #endregion Commands2
            #region Commands3
            var SignalgeneratorTypeEnum = new List<string>
              {
                "GenDisabled",
                "RampUpDown",
                "SquareWave",
                "SinWave"

            };
            Enums.Add("S.G.Type", SignalgeneratorTypeEnum);

            var SignalgeneratorTypeCmd = new EnumViewModel
            {
                CommandName = "Type",
                CommandId = "7",
                CommandSubId = "1",
                CommandList = Enums["S.G.Type"],
                CommandValue = "1",//first enum in list start at 0
            };
            //DataViewCommandsList.Add(new Tuple<int, int>(7, 1), SignalgeneratorTypeCmd);
            EnumViewCommandsList.Add(new Tuple<int, int>(7, 1), SignalgeneratorTypeCmd);
            EnumCommandsListbySubGroup.Add("S.G.Type", new ObservableCollection<object>
            {
              SignalgeneratorTypeCmd
            });
            #endregion Commands3
            #region Commands4
            DataCommandsListbySubGroup.Add("S.G.List", new ObservableCollection<object>());

            data = new DataViewModel
            {
                CommandName = "Offset",
                CommandId = "7",
                CommandSubId = "5",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(7, 5), data);
            DataCommandsListbySubGroup["S.G.List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Frequency [Hz]",
                CommandId = "7",
                CommandSubId = "6",
                CommandValue = "",
                IsFloat = true,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(7, 6), data);
            DataCommandsListbySubGroup["S.G.List"].Add(data);


            #endregion Commands4
            #region Commands5
            DataCommandsListbySubGroup.Add("PowerOut List", new ObservableCollection<object>());
            data = new DataViewModel
            {
                CommandName = "PowerOut",
                CommandId = "12",
                CommandSubId = "1",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(12, 1), data);
            DataCommandsListbySubGroup["PowerOut List"].Add(data);
            #endregion Commands5
            #region Status_1
            DataCommandsListbySubGroup.Add("MotionStatus List", new ObservableCollection<object>());
            
            data = new DataViewModel
            {
                CommandName = "PWM %",
                CommandId = "30",
                CommandSubId = "2",
                CommandValue = "",
                IsFloat = true,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(30, 2), data);
            DataCommandsListbySubGroup["MotionStatus List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Speed Fdb",
                CommandId = "25",
                CommandSubId = "0",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(25, 0), data);
            DataCommandsListbySubGroup["MotionStatus List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "RPM",
                CommandId = "25",
                CommandSubId = "10",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(25, 10), data);
            DataCommandsListbySubGroup["MotionStatus List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "IQ Current [A]",
                CommandId = "30",
                CommandSubId = "0",
                CommandValue = "",
                IsFloat = true,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(30, 0), data);
            DataCommandsListbySubGroup["MotionStatus List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "ID Current [A]",
                CommandId = "30",
                CommandSubId = "1",
                CommandValue = "",
                IsFloat = true,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(30, 1), data);
            DataCommandsListbySubGroup["MotionStatus List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Ia",
                CommandId = "30",
                CommandSubId = "10",
                CommandValue = "",
                IsFloat = true,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(30, 10), data);
            DataCommandsListbySubGroup["MotionStatus List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Ib",
                CommandId = "30",
                CommandSubId = "11",
                CommandValue = "",
                IsFloat = true,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(30, 11), data);
            DataCommandsListbySubGroup["MotionStatus List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Ic",
                CommandId = "30",
                CommandSubId = "12",
                CommandValue = "",
                IsFloat = true,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(30, 12), data);
            DataCommandsListbySubGroup["MotionStatus List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Temperature [C]",
                CommandId = "32",
                CommandSubId = "1",
                CommandValue = "",
                IsFloat = true,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(32, 1), data);
            DataCommandsListbySubGroup["MotionStatus List"].Add(data);
            #endregion Status_1
            #region Status2

            DigitalInputListbySubGroup.Add("Digital Input List", new ObservableCollection<object>());
            var names = new[]
            {
                "Input 1", "Input 2", "Input 3", "Input 4"
            };
            for(int i = 1; i < 5; i++)
            {
                var input = new BoolViewIndModel
                {
                    CommandName = names[i - 1],
                    CommandValue = 0,
                    CommandId = "29",
                    CommandSubId = i.ToString(),
                    IsFloat = false
                };
                DigitalInputList.Add(new Tuple<int, int>(29, i), input);
                DigitalInputListbySubGroup["Digital Input List"].Add(input);
            }
            #endregion Status2

            #region Status_3
            DataCommandsListbySubGroup.Add("Position counters List", new ObservableCollection<object>());
            data = new DataViewModel
            {
                CommandName = "Main",
                CommandId = "26",
                CommandSubId = "0",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(26, 0), data);
            DataCommandsListbySubGroup["Position counters List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Hall",
                CommandId = "26",
                CommandSubId = "1",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(26, 1), data);
            DataCommandsListbySubGroup["Position counters List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "QEP",
                CommandId = "26",
                CommandSubId = "2",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(26, 2), data);
            DataCommandsListbySubGroup["Position counters List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "SSI",
                CommandId = "26",
                CommandSubId = "3",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(26, 3), data);
            DataCommandsListbySubGroup["Position counters List"].Add(data);
            #endregion Status_3

        }
        //public void GenerateCalCommands()
        //{
        //    DataCommandsListbySubGroup.Add("CalibrationCommands List", new ObservableCollection<object>());

        //    var data = new DataViewModel
        //    {
        //        CommandName = "Current Offset",
        //        CommandId = "6",
        //        CommandSubId = "2",
        //        CommandValue = "",
        //    };
        //    DataViewCommandsList.Add(new Tuple<int, int>(6, 2), data);
        //    DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

        //    data = new DataViewModel
        //    {
        //        CommandName = "PI Current Loop",
        //        CommandId = "6",
        //        CommandSubId = "4",
        //        CommandValue = "",
        //    };
        //    DataViewCommandsList.Add(new Tuple<int, int>(6, 4), data);
        //    DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

        //    data = new DataViewModel
        //    {
        //        CommandName = "Hall Mapping",
        //        CommandId = "6",
        //        CommandSubId = "6",
        //        CommandValue = "",
        //    };
        //    DataViewCommandsList.Add(new Tuple<int, int>(6, 6), data);
        //    DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

        //    data = new DataViewModel
        //    {
        //        CommandName = "Encoder1 Direction",
        //        CommandId = "6",
        //        CommandSubId = "8",
        //        CommandValue = "",
        //    };
        //    DataViewCommandsList.Add(new Tuple<int, int>(6, 8), data);
        //    DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

        //    data = new DataViewModel
        //    {
        //        CommandName = "PI Speed Loop",
        //        CommandId = "6",
        //        CommandSubId = "10",
        //        CommandValue = "",
        //    };
        //    DataViewCommandsList.Add(new Tuple<int, int>(6, 10), data);
        //    DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

        //    data = new DataViewModel
        //    {
        //        CommandName = "PI Position Loop",
        //        CommandId = "6",
        //        CommandSubId = "12",
        //        CommandValue = "",
        //    };
        //    DataViewCommandsList.Add(new Tuple<int, int>(6, 12), data);
        //    DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

        //}
        public void GenerateLPCommands()
        {
            #region Commands1
            DataCommandsListbySubGroup.Add("LPCommands List", new ObservableCollection<object>());

            var data = new DataViewModel
            {
                CommandName = "SN",
                CommandId = "62",
                CommandSubId = "1",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsListLP.Add(new Tuple<int, int>(62, 1), data);
            DataCommandsListbySubGroup["LPCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "HW Rev",
                CommandId = "62",
                CommandSubId = "2",
                CommandValue = "",
                IsFloat = true,
            };
            DataViewCommandsListLP.Add(new Tuple<int, int>(62, 2), data);
            DataCommandsListbySubGroup["LPCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "FW Rev",
                CommandId = "62",
                CommandSubId = "3",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsListLP.Add(new Tuple<int, int>(62, 3), data);
            DataCommandsListbySubGroup["LPCommands List"].Add(data);
            #endregion Commands1

            #region Commands2
            //var ProfilerModeEnum = new List<string>
            //  {
            //    "uRayon",
            //    "Rayon 30A",
            //    "uRayon SB",
            //    "Rayon HP",
            //    "Rayon MK6",
            //    "Rayon 70A"

            //  };
            //Enums.Add("Driver Type", ProfilerModeEnum);

            //var ProfilerModeCmd = new EnumViewModel
            //{
            //    CommandName = "Driver Type",
            //    CommandId = "62",
            //    CommandSubId = "0",
            //    CommandList = Enums["Driver Type"],
            //    CommandValue = "1",//first enum in list
            //};
            //DataViewCommandsListLP.Add(new Tuple<int, int>(62, 0), ProfilerModeCmd);
            //EnumViewCommandsList.Add(new Tuple<int, int>(62, 0), ProfilerModeCmd);
            //EnumCommandsListbySubGroup.Add("Driver Type", new ObservableCollection<object>
            //{
            //  ProfilerModeCmd
            //});
            #endregion Commands2
            #region Command3
            DataCommandsListbySubGroup.Add("DriverStatus List", new ObservableCollection<object>());
            data = new DataViewModel
            {
                CommandName = "Driver Status",
                CommandId = "33",
                CommandSubId = "1",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(33, 1), data);
            DataCommandsListbySubGroup["DriverStatus List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "MotorStatus",
                CommandId = "1",
                CommandSubId = "0",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(1, 0), data);
            DataCommandsListbySubGroup["MotionStatus List"].Add(data);
            #endregion Command3
        }
        private void GenerateMotionTabCommands()
        {
            DataCommandsListbySubGroup.Add("CurrentLimit List", new ObservableCollection<object>());

            var names = new[]
            {
                "Continuous Current Limit [A]", "Peak Current Limit [A]", "Peak Time [sec]", "PWM limit [%]"
            };


            for(int i = 0; i < names.Length; i++)
            {
                var data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "52",
                    CommandSubId = (i + 1).ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = true,
                };

                DataViewCommandsList.Add(new Tuple<int, int>(52, i + 1), data);
                DataCommandsListbySubGroup["CurrentLimit List"].Add(data);
            }
        }
        private void GenerateMaintenanceList()
        {
            DataCommandsListbySubGroup.Add("Maintenance List", new ObservableCollection<object>());

            var data = new DataViewModel
            {
                CommandName = "Flash Checksum",
                CommandId = "63",
                CommandSubId = "11",
                CommandValue = "",
                IsFloat = false,
            };
            //DataViewCommandsList.Add(new Tuple<int, int>(63, 11), data);
            //DataCommandsListbySubGroup["Maintenance List"].Add(data);

            //DataCommandsListbySubGroup.Add("MaintenanceBool List", new ObservableCollection<object>());

            var names = new[] { "Save", "Load Manufacture defaults" }; //, "Reboot Driver", "Enable Protected Write", "Enable Loader"};
            var ID = new[] { "63", "63" }; //, "63", "63", "65" };
            var subID = new[] { "0", "1" }; //, "2", "10", "0" };
            for(int i = 0; i < names.Length; i++)
            {
                data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = ID[i],
                    CommandSubId = subID[i],
                    CommandValue = "",
                    IsFloat = true,
                };
                //DataViewCommandsList.Add(new Tuple<int, int>(Convert.ToInt16(ID[i]), Convert.ToInt16(subID[i])), data);
                //DataCommandsListbySubGroup["MaintenanceBool List"].Add(data);
            }

            data = new DataViewModel
            {
                CommandName = "Protected Params",
                CommandId = "63",
                CommandSubId = "10",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(63, 10), data);
            DataCommandsListbySubGroup["Maintenance List"].Add(data);

        }
        private void UpperMainPannelList()
        {
            DataCommandsListbySubGroup.Add("UpperMainPan List", new ObservableCollection<object>());

            var data = new DataViewModel
            {
                CommandName = "CH1",
                CommandId = "60",
                CommandSubId = "1",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(60, 1), data);
            DataCommandsListbySubGroup["UpperMainPan List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "CH2",
                CommandId = "60",
                CommandSubId = "2",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(60, 2), data);
            DataCommandsListbySubGroup["UpperMainPan List"].Add(data);

        }
        private void CalibrationCmd()
        {
            CalibartionCommandsListbySubGroup.Add("Calibration List", new ObservableCollection<object>());
            CalibartionCommandsListbySubGroup.Add("Calibration Result List", new ObservableCollection<object>());

            var names = new[]
            {
                "Current Offset", "PI Current Loop", "Hall Mapping", "Encoder1 Direction", "PI Speed Loop", "PI Position Loop", "Abs. Enc."
            };
            for(int i = 0; i < names.Length; i++) // Calibration Button
            {
                var data = new CalibrationButtonModel
                {
                    CommandName = names[i],
                    CommandId = "6",
                    CommandSubId = (i * 2 + 1).ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = false,
                };
                CalibartionCommandsList.Add(new Tuple<int, int>(6, Convert.ToInt16(i * 2 + 1)), data);
                CalibartionCommandsListbySubGroup["Calibration List"].Add(data);

                var TextBoxResult = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "6",
                    CommandSubId = (i * 2 + 2).ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = false,
                };
                DataViewCommandsList.Add(new Tuple<int, int>(6, Convert.ToInt16(i * 2 + 2)), TextBoxResult);
                CalibartionCommandsListbySubGroup["Calibration Result List"].Add(TextBoxResult);
            }
        }
        private void BuildErrorList()
        {
            // Com. Error: 
            ErrorList.Add(2, "BAD_COMMAND");
            ErrorList.Add(3, "BAD_INDEX");
            ErrorList.Add(5, "NO_INTERPRETER_MEANING");
            ErrorList.Add(6, "PROGRAM_NOT_RUNNING");
            ErrorList.Add(7, "MODE_NOT_STARTED");
            ErrorList.Add(11, "CANNOT_WRITE_TO_FLASH");
            ErrorList.Add(12, "COMMAND_NOT_AVAILABLE");
            ErrorList.Add(13, "UART_BUSY");
            ErrorList.Add(18, "EMPTY_ASSIGN");
            ErrorList.Add(19, "BAD_COMMAND_FORMAT");
            ErrorList.Add(21, "OPERAND_OUT_OF_RANGE");
            ErrorList.Add(22, "ZERO_DIVISION");
            ErrorList.Add(23, "COMMAND_NOT_ASSIGNED");
            ErrorList.Add(24, "BAD_OPERAND");
            ErrorList.Add(25, "COMMAND_NOT_VALID");
            ErrorList.Add(26, "MOTION_MODE_NOT_VALID");
            ErrorList.Add(28, "OUT_OF_LIMIT_RANGE");
            ErrorList.Add(30, "NO_PROGRAM_TO_CONTINUE");
            ErrorList.Add(32, "COMMUNICATION_ERROR");
            ErrorList.Add(37, "HALL_DEFINED_SAME_LOCATION");
            ErrorList.Add(38, "HALL_READING_ERROR");
            ErrorList.Add(39, "MOTION_START_PAST");
            ErrorList.Add(41, "COMMAND_NOT_SUPPORTED");
            ErrorList.Add(42, "NO_SUCH_LABEL");
            ErrorList.Add(57, "MOTOR_MUST_BE_OFF");
            ErrorList.Add(58, "MOTOR_MUST_BE_ON");
            ErrorList.Add(60, "BAD_UNIT_MODE");
            ErrorList.Add(66, "DRIVE_NOT_READY");
            ErrorList.Add(71, "HOMING_BUSY");
            ErrorList.Add(72, "MODULO_MUST_EVEN");
            ErrorList.Add(73, "SET_POSITION");
            ErrorList.Add(127, "MODULO_RANGE_MUST_POSITIVE");
            ErrorList.Add(166, "OUT_OF_MODULO_RANGE");
            ErrorList.Add(200, "Reset_Driver_occured");
        }

        private void GenerateGain()
        {
            GainList.Add("Gain1 List", new ObservableCollection<object>());
            var data = new NumericTextboxModel
            {
                Channel = "1",
                Name = "Gain",
                CommandValue = "1.0",
                BackGround = (SolidColorBrush)(new BrushConverter().ConvertFrom("#82F7E31D")),
            };
            Gain.Add("Ch1 Gain", data);
            GainList["Gain1 List"].Add(data);

            GainList.Add("Gain2 List", new ObservableCollection<object>());
            data = new NumericTextboxModel
            {
                Channel = "2",
                Name = "Gain",
                CommandValue = "1.0",
                BackGround = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7F1810D4")),
            };
            Gain.Add("Ch2 Gain", data);
            GainList["Gain2 List"].Add(data);
        }

        private void GenerateDebugListCommands()
        {
            DebugListbySubGroup.Add("Debug List", new ObservableCollection<object>());

            var data = new DebugObjViewModel
            {
                ID = "62",
                Index = "3",
                IntFloat = true,
                GetData = "",
                SetData = "",
            };
            DebugList.Add(new Tuple<int, int>(62, 3), data);
            DebugListbySubGroup["Debug List"].Add(data);
        }
    }
}

