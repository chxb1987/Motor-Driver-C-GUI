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
            GenerateRPCommands();
            GenerateCalCommands();
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

            DataViewCommandsList.Add(new Tuple<int, int>(400, 0), temp);
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
                "Kp", "Ki", "Kc"
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
            DataCommandsListbySubGroup.Add("Digital", new ObservableCollection<object>());
            DataCommandsListbySubGroup.Add("Analog", new ObservableCollection<object>());


            for (var i = 0; i < names.Length; i++)
            {


                var data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "70",
                    CommandSubId = (i + 1).ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",
                    IsFloat = names[i] == "Hall Angle"
                };



                DataViewCommandsList.Add(new Tuple<int, int>(70, i + 1), data);
                //if (i != 0)
                DataCommandsListbySubGroup["Hall"].Add(data);


                if (i >= 6) continue;

                data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "71",
                    CommandSubId = i.ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",

                };

                DataViewCommandsList.Add(new Tuple<int, int>(71, i), data);
                if (i != 0)
                    DataCommandsListbySubGroup["Qep1"].Add(data);



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


                data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "75",
                    CommandSubId = i.ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",

                };
                DataViewCommandsList.Add(new Tuple<int, int>(75, i), data);
                if (i != 0)
                    DataCommandsListbySubGroup["Digital"].Add(data);


                data = new DataViewModel
                {
                    CommandName = names[i],
                    CommandId = "76",
                    CommandSubId = i.ToString(CultureInfo.InvariantCulture),
                    CommandValue = "",

                };
                DataViewCommandsList.Add(new Tuple<int, int>(76, i), data);
                if (i != 0)
                    DataCommandsListbySubGroup["Analog"].Add(data);
            }
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
                CommandId = "212",
                CommandSubId = "0",
                CommandList = Enums["Drive Mode"],
                CommandValue = "1",//first enum in list
            };

            EnumViewCommandsList.Add(new Tuple<int, int>(212, 0), enum1);


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
                CommandId = "209",
                CommandSubId = "1",
                CommandValue = "1", //first enum in list
                CommandList = Enums["Commutation Source"]
            };


            EnumViewCommandsList.Add(new Tuple<int, int>(209, 1), enum2);
            EnumCommandsListbySubGroup["Control"].Add(enum2);


            var data1 = new DataViewModel
            {
                CommandName = "Pole Pair",
                CommandId = "51",
                CommandSubId = "1",
                CommandValue = "",

            };

            DataCommandsListbySubGroup.Add("Motor", new ObservableCollection<object> { data1 });
            DataViewCommandsList.Add(new Tuple<int, int>(51, 1), data1);


            var data2 = new DataViewModel
            {
                CommandName = "Direction",
                CommandId = "51",
                CommandSubId = "2",
                CommandValue = "",

            };
            DataViewCommandsList.Add(new Tuple<int, int>(51, 2), data2);
            DataCommandsListbySubGroup["Motor"].Add(data2);



            var data3 = new DataViewModel
            {
                CommandName = "Max speed [rpm]",
                CommandId = "53",
                CommandSubId = "1",
                CommandValue = "",
                IsFloat = true
            };
            DataViewCommandsList.Add(new Tuple<int, int>(53, 1), data3);


            DataCommandsListbySubGroup.Add("Motion Limit", new ObservableCollection<object> { data3 });




            var data4 = new DataViewModel
            {
                CommandName = "Min Speed [rpm]",
                CommandId = "53",
                CommandSubId = "2",
                CommandValue = "",
                IsFloat = true
            };

            DataViewCommandsList.Add(new Tuple<int, int>(53, 2), data4);
            DataCommandsListbySubGroup["Motion Limit"].Add(data4);



            var data5 = new DataViewModel
            {

                CommandName = "Max position [Counts]",
                CommandId = "53",// "205",
                CommandSubId = "3",
                CommandValue = "",

            };
            DataViewCommandsList.Add(new Tuple<int, int>(53, 3), data5);

            DataCommandsListbySubGroup["Motion Limit"].Add(data5);

            var data6 = new DataViewModel
            {

                CommandName = "Min position [Counts]",
                CommandId = "53",
                CommandSubId = "4",
                CommandValue = "",
            };
            DataViewCommandsList.Add(new Tuple<int, int>(53, 4), data6);
            DataCommandsListbySubGroup["Motion Limit"].Add(data6);
        }
        public void GenerateRPCommands()
        {
            DataCommandsListbySubGroup.Add("RPCommands List", new ObservableCollection<object>());

            var data = new DataViewModel
            {
                CommandName = "Current [A]",
                CommandId = "6",
                CommandSubId = "7",
                CommandValue = "",
            };
            DataCommandsListbySubGroup["RPCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Speed [CPS]",
                CommandId = "4",
                CommandSubId = "0",
                CommandValue = "",
            };
            DataCommandsListbySubGroup["RPCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Speed Position [CPS]",
                CommandId = "5",
                CommandSubId = "2",
                CommandValue = "",
            };
            DataCommandsListbySubGroup["RPCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Position [Count]",
                CommandId = "5",
                CommandSubId = "0",
                CommandValue = "",
            };
            DataCommandsListbySubGroup["RPCommands List"].Add(data);

        }
        public void GenerateCalCommands()
        {
            DataCommandsListbySubGroup.Add("CalibrationCommands List", new ObservableCollection<object>());

            var data = new DataViewModel
            {
                CommandName = "Current Offset",
                CommandId = "6",
                CommandSubId = "1",
                CommandValue = "",
            };
            DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "PI Current Loop",
                CommandId = "6",
                CommandSubId = "2",
                CommandValue = "",
            };
            DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "PI Speed Loop",
                CommandId = "6",
                CommandSubId = "5",
                CommandValue = "",
            };
            DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Hall Mapping",
                CommandId = "6",
                CommandSubId = "3",
                CommandValue = "",
            };
            DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "Encoder1 Direction",
                CommandId = "6",
                CommandSubId = "4",
                CommandValue = "",
            };
            DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);

            data = new DataViewModel
            {
                CommandName = "PI Position Loop",
                CommandId = "6",
                CommandSubId = "6",
                CommandValue = "",
            };
            DataCommandsListbySubGroup["CalibrationCommands List"].Add(data);
            
        }
    }
}

