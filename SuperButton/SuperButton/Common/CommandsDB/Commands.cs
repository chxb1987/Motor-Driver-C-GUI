using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using SuperButton.ViewModels;


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
            GenerateDriverCommands();
            GenerateMainWindowCommands();
            GenerateBPCommands();
            GenerateCalCommands();
            GenerateLPCommands();
            GenerateMotionTabCommands();
            GenerateMaintenanceList();
        }

        static public void AssemblePacket(out PacketFields rxPacket, Int16 id, Int16 subId, bool isSet, bool isFloat, object data2Send)
        {
            if (id == 81 && subId == 1 && isSet == true)
            { /*int i = 0;*/ }
            rxPacket.ID = id;
            rxPacket.IsFloat = isFloat;
            rxPacket.IsSet = isSet;
            rxPacket.SubID = subId;
            rxPacket.Data2Send = data2Send;
        }
        
        public Dictionary<Tuple<int, int>, DataViewModel> DataViewCommandsList = new Dictionary<Tuple<int, int>, DataViewModel>();
        public Dictionary<Tuple<int, int>, EnumViewModel> EnumViewCommandsList = new Dictionary<Tuple<int, int>, EnumViewModel>();
        public Dictionary<string, List<string>> Enums = new Dictionary<string, List<string>>();

        public Dictionary<string, ObservableCollection<object>> EnumCommandsListbySubGroup = new Dictionary<string, ObservableCollection<object>>();
        public Dictionary<string, ObservableCollection<object>> DataCommandsListbySubGroup = new Dictionary<string, ObservableCollection<object>>();
        private static readonly object Synlock = new object(); //Single tone variable
        private static Commands _instance;
        public float _pidcurr;
        public float Pidcurr
        {
            get { return _pidcurr; }
            set
            {
                if (_pidcurr == value) return;
                _pidcurr = value;

            }
        }
        public static Commands GetInstance
        {

            get
            {
                lock (Synlock)
                {
                    if (_instance != null) return _instance;
                    _instance = new Commands();
                    return _instance;
                }
            }
        }

        private void GenerateMainWindowCommands()
        {
            DataViewModel temp = new DataViewModel
            {
                CommandId = "400",
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


            for (int i = 0; i < names.Length; i++)
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
            for (int i = 0; i < names.Length; i++)
            {
                var data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "122",
                    CommandSubId = i.ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",

                };

                DataViewCommandsList.Add(new Tuple<int, int>(122, i), data);
                DataCommandsListbySubGroup["DeviceSerial"].Add(data);

            }

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
                "Kp [A]", "Ki [A]", "Kc [A]"
            };

            DataCommandsListbySubGroup.Add("PIDCurrent", new ObservableCollection<object>());
            DataCommandsListbySubGroup.Add("PIDSpeed", new ObservableCollection<object>());
            DataCommandsListbySubGroup.Add("PIDPosition", new ObservableCollection<object>());

            for (int i = 0; i < names.Length; i++)
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
            var names = new[]
            {
                "Enable", "Roll UP", "Sample Period",
                "Direction", "Counts Per Rev", "Speed LPF Cut-Off", "Hall Angle"
            };

            DataCommandsListbySubGroup.Add("Hall", new ObservableCollection<object>());
            DataCommandsListbySubGroup.Add("Qep1", new ObservableCollection<object>());
            DataCommandsListbySubGroup.Add("Qep2", new ObservableCollection<object>());
            DataCommandsListbySubGroup.Add("SSI_Feedback", new ObservableCollection<object>());
            //DataCommandsListbySubGroup.Add("Digital", new ObservableCollection<object>());
            //DataCommandsListbySubGroup.Add("Analog", new ObservableCollection<object>());


            for (var i = 0; i < names.Length; i++)
            {
                var data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "70",
                    CommandSubId = (i + 1).ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = names[i] == "Speed LPF Cut-Off"
                };
                DataViewCommandsList.Add(new Tuple<int, int>(70, i + 1), data);
                DataCommandsListbySubGroup["Hall"].Add(data);

                if (i >= 6) continue;

                data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "72",
                    CommandSubId = i.ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                };
                DataViewCommandsList.Add(new Tuple<int, int>(72, i), data);
                if (i != 0)
                    DataCommandsListbySubGroup["Qep2"].Add(data);

                data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "73",
                    CommandSubId = i.ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                };
                DataViewCommandsList.Add(new Tuple<int, int>(73, i), data);
                if (i != 0)
                    DataCommandsListbySubGroup["SSI_Feedback"].Add(data);

                //data = new DataViewModel
                //{
                //    CommandName = names[i],
                //    CommandId = "75",
                //    CommandSubId = i.ToString(CultureInfo.InvariantCulture),
                //    CommandValue = "",
                //};
                //DataViewCommandsList.Add(new Tuple<int, int>(75, i), data);
                //if (i != 0)
                    //DataCommandsListbySubGroup["Digital"].Add(data);

                //data = new DataViewModel
                //{
                //    CommandName = names[i],
                //    CommandId = "76",
                //    CommandSubId = i.ToString(CultureInfo.InvariantCulture),
                //    CommandValue = "",
                //};
                //DataViewCommandsList.Add(new Tuple<int, int>(76, i), data);
                //if (i != 0)
                    //DataCommandsListbySubGroup["Analog"].Add(data);
            }

            names = new[]
            {
                "Enable", "Roll High", "Roll Low", "Direction", "Counts Per Rev",
                "Speed LPF", "Reset Value", "Set Position Value"
            };
            bool[] IsFloat = new[] {false, false, false, false, false, true, false, false, };
            for (int i = 0, k = 1; i < names.Length; i++, k++)
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

                if (k == 3)
                    k = 4;
                if (k == 8)
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
            DataViewCommandsList.Add(new Tuple<int, int>(73, 14), dataB);
            DataCommandsListbySubGroup["Qep1"].Add(dataB);

            //Qep1FdBckList Qep1Bis
            var tmp1 = new List<string>
              {
                  "Index Disabled",
                  "One Shot",
                  "Continuous Refresh"
              };
            Enums.Add("Index Reset", tmp1);

            var enum1 = new EnumViewModel
            {
                CommandName = "Index Reset",
                CommandId = "71",
                CommandSubId = "8",
                CommandList = Enums["Index Reset"],
                CommandValue = "1",//first enum in list
                IsFloat = false,
            };

            EnumViewCommandsList.Add(new Tuple<int, int>(71, 8), enum1);

            EnumCommandsListbySubGroup.Add("Qep1Bis", new ObservableCollection<object>
            {
              enum1
            });

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
                 "Cmtn Hall",
                 "Cmtn Qep1",
                 "Cmtn Qep2",
                 "Cmtn Hall And Qep1",
                 "Cmtn Hall And Qep2",
                 "Cmtn SSI Abs",
                 "Cmtn Forced",
                 "Cmtn SSI Abs",
                 "Cmtn DC Brushed",
                 "Cmtn SSI Inc",
                 "Cmtn Src Analog",
                 "Cmtn Src Sin Cos",
                 "Cmtn SensorLess"
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
                 "Fdb Hall",
                 "Fdb Enc1",
                 "Fdb Enc2"
             };

            Enums.Add("Speed fdbck Source", tmp3);
            var enum3 = new EnumViewModel
            {
                CommandName = "Speed fdbck Source",
                CommandId = "50",
                CommandSubId = "3",
                CommandValue = "1", //first enum in list
                CommandList = Enums["Speed fdbck Source"]
            };
            EnumViewCommandsList.Add(new Tuple<int, int>(50, 3), enum3);
            EnumCommandsListbySubGroup["Control"].Add(enum3);

            var tmp4 = new List<string>
             {
                 "Fdb Hall",
                 "Fdb Enc1",
                 "Fdb Enc2"
             };
            Enums.Add("Position fdbck Source", tmp4);
            var enum4 = new EnumViewModel
            {
                CommandName = "Position fdbck Source",
                CommandId = "50",
                CommandSubId = "4",
                CommandValue = "1", //first enum in list
                CommandList = Enums["Position fdbck Source"]
            };
            EnumViewCommandsList.Add(new Tuple<int, int>(50, 4), enum4);
            EnumCommandsListbySubGroup["Control"].Add(enum4);

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

            var data = new DataViewModel
            {
                CommandName = "Current [A]",
                CommandId = "3",
                CommandSubId = "0",
                CommandValue = "",
                IsFloat = true,
                IsSelected = false,
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
            DataCommandsListbySubGroup["MotionCommand List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "PTP Speed [C/S]",
                CommandId = "54",
                CommandSubId = "2",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(54, 2), data);
            DataCommandsListbySubGroup["MotionCommand List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Max Tracking Err [C]",
                CommandId = "54",
                CommandSubId = "6",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(54, 6), data);
            DataCommandsListbySubGroup["MotionCommand List"].Add(data);
            #endregion Commands1
            #region Commands2
            var ProfilerModeEnum = new List<string>
              {
                  "PID",
                  "Trapezoid",
                  "Test1",
                  "Test2"
              };
            Enums.Add("Profiler Mode", ProfilerModeEnum);

            var ProfilerModeCmd = new EnumViewModel
            {
                CommandName = "Profiler Mode",
                CommandId = "54",
                CommandSubId = "1",
                CommandList = Enums["Profiler Mode"],
                CommandValue = "",//first enum in list
            };
            DataViewCommandsList.Add(new Tuple<int, int>(54, 1), ProfilerModeCmd);
            EnumViewCommandsList.Add(new Tuple<int, int>(54, 1), ProfilerModeCmd);
            EnumCommandsListbySubGroup.Add("Profiler Mode", new ObservableCollection<object>
            {
              ProfilerModeCmd
            });
            #endregion Commands2
            #region Status
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

            #endregion Status
        }
        public void GenerateCalCommands()
        {
            DataCommandsListbySubGroup.Add("CalibrationCommands List", new ObservableCollection<object>());

            var data = new DataViewModel
            {
                CommandName = "Current Offset",
                CommandId = "6",
                CommandSubId = "2",
                CommandValue = "",
            };
            DataViewCommandsList.Add(new Tuple<int, int>(6, 2), data);
            DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "PI Current Loop",
                CommandId = "6",
                CommandSubId = "4",
                CommandValue = "",
            };
            DataViewCommandsList.Add(new Tuple<int, int>(6, 4), data);
            DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Hall Mapping",
                CommandId = "6",
                CommandSubId = "6",
                CommandValue = "",
            };
            DataViewCommandsList.Add(new Tuple<int, int>(6, 6), data);
            DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Encoder1 Direction",
                CommandId = "6",
                CommandSubId = "8",
                CommandValue = "",
            };
            DataViewCommandsList.Add(new Tuple<int, int>(6, 8), data);
            DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "PI Speed Loop",
                CommandId = "6",
                CommandSubId = "10",
                CommandValue = "",
            };
            DataViewCommandsList.Add(new Tuple<int, int>(6, 10), data);
            DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "PI Position Loop",
                CommandId = "6",
                CommandSubId = "12",
                CommandValue = "",
            };
            DataViewCommandsList.Add(new Tuple<int, int>(6, 12), data);
            DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

        }
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
            DataViewCommandsList.Add(new Tuple<int, int>(62, 1), data);
            DataCommandsListbySubGroup["LPCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "HW Rev",
                CommandId = "62",
                CommandSubId = "2",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(62, 2), data);
            DataCommandsListbySubGroup["LPCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "FW Rev",
                CommandId = "62",
                CommandSubId = "3",
                CommandValue = "",
                IsFloat = false,
            };
            DataViewCommandsList.Add(new Tuple<int, int>(62, 3), data);
            DataCommandsListbySubGroup["LPCommands List"].Add(data);
            #endregion Commands1

            #region Commands2
            var ProfilerModeEnum = new List<string>
              {
                "uRayon",
                "Rayon 30A",
                "uRayon SB",
                "Rayon HP",
                "Rayon MK6",
                "Rayon 70A"

              };
            Enums.Add("Driver Type", ProfilerModeEnum);

            var ProfilerModeCmd = new EnumViewModel
            {
                CommandName = "Driver Type",
                CommandId = "62",
                CommandSubId = "0",
                CommandList = Enums["Driver Type"],
                CommandValue = "1",//first enum in list
            };
            DataViewCommandsList.Add(new Tuple<int, int>(62, 0), ProfilerModeCmd);
            EnumViewCommandsList.Add(new Tuple<int, int>(62, 0), ProfilerModeCmd);
            EnumCommandsListbySubGroup.Add("Driver Type", new ObservableCollection<object>
            {
              ProfilerModeCmd
            });
            #endregion Commands2

        }
        private void GenerateMotionTabCommands()
        {
            DataCommandsListbySubGroup.Add("CurrentLimit List", new ObservableCollection<object>());

            var names = new[]
            {
                "Continuous Current Limit [A]", "Peak Current Limit [A]", "Peak Time [sec]", "PWM limit [%]" 
            };


            for (int i = 0; i < names.Length; i++)
            {
                var data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "52",
                    CommandSubId = (i+1).ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = true,
                };

                DataViewCommandsList.Add(new Tuple<int, int>(52, i+1), data);
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

            DataCommandsListbySubGroup.Add("MaintenanceBool List", new ObservableCollection<object>());

            var names = new[] { "Save", "Load Manufacture defaults" }; //, "Reboot Driver", "Enable Protected Write", "Enable Loader"};
            var ID = new[] {"63", "63" }; //, "63", "63", "65" };
            var subID = new[] {"0", "1" }; //, "2", "10", "0" };
            for (int i = 0; i < names.Length; i++)
            {
                data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = ID[i],
                    CommandSubId = subID[i],
                    CommandValue = "",
                    IsFloat = true,
                };
                DataViewCommandsList.Add(new Tuple<int, int>(Convert.ToInt16(ID[i]), Convert.ToInt16(subID[i])), data);
                DataCommandsListbySubGroup["MaintenanceBool List"].Add(data);
            }
        }

    }
}

