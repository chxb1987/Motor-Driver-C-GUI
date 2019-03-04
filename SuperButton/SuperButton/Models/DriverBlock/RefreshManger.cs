using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SuperButton.CommandsDB;
using SuperButton.ViewModels;
using System.Collections.ObjectModel;
using SuperButton.Views;
using System.Collections;
using SuperButton.Helpers;
using System.Diagnostics;
using System.Windows.Threading; // For Dispatcher.
using System.Windows;

namespace SuperButton.Models.DriverBlock
{

    public class RefreshManger
    {
        public static int tab = Views.ParametarsWindow.ParametersWindowTabSelected;
        private static readonly object Synlock = new object();
        private static RefreshManger _instance;

        public static Dictionary<string, ObservableCollection<object>> BuildGroup = new Dictionary<string, ObservableCollection<object>>();
        public static Dictionary<Tuple<int, int>, DataViewModel> BuildList;
        public static int TempTab = 0;
        private static bool _dataPressed = false;
        public static bool DataPressed { get { return _dataPressed; } set { _dataPressed = value; } }
        public static RefreshManger GetInstance
        {
            get
            {
                lock(Synlock)
                {
                    if(_instance != null)
                        return _instance;
                    _instance = new RefreshManger();
                    buildGroup();
                    return _instance;
                }
            }
        }

        private static void buildGroup()
        {
            //var DataViewlist = CommandsDB.Commands.GetInstance.DataViewCommandsList;
            //var EnumViewlist = CommandsDB.Commands.GetInstance.EnumViewCommandsList;
            var AllDataList = CommandsDB.Commands.GetInstance.DataCommandsListbySubGroup;
            var AllEnumList = CommandsDB.Commands.GetInstance.EnumCommandsListbySubGroup;
            var AllCalList = CommandsDB.Commands.GetInstance.CalibartionCommandsListbySubGroup;

            foreach(var list in AllEnumList)
            {
                BuildGroup.Add(list.Key, new ObservableCollection<object>());
                foreach(var sub_list in list.Value)
                {
                    var data = new DataViewModel
                    {
                        CommandName = ((DataViewModel)sub_list).CommandName,
                        CommandId = ((DataViewModel)sub_list).CommandId,
                        CommandSubId = ((DataViewModel)sub_list).CommandSubId,
                        CommandValue = ((DataViewModel)sub_list).CommandValue,
                        IsFloat = ((DataViewModel)sub_list).IsFloat,
                    };
                    BuildGroup[list.Key].Add(data);
                }
            }

            foreach(var list in AllDataList)
            {
                BuildGroup.Add(list.Key, new ObservableCollection<object>());
                foreach(var sub_list in list.Value)
                {
                    var data = new DataViewModel
                    {
                        CommandName = ((DataViewModel)sub_list).CommandName,
                        CommandId = ((DataViewModel)sub_list).CommandId,
                        CommandSubId = ((DataViewModel)sub_list).CommandSubId,
                        CommandValue = ((DataViewModel)sub_list).CommandValue,
                        IsFloat = ((DataViewModel)sub_list).IsFloat,
                    };
                    BuildGroup[list.Key].Add(data);
                }
            }
            foreach(var list in AllCalList)
            {
                BuildGroup.Add(list.Key, new ObservableCollection<object>());
                foreach(var sub_list in list.Value)
                {
                    if(list.Key == "Calibration List")
                    {
                        CalibrationButtonModel temp = sub_list as CalibrationButtonModel;
                        string CommandName = temp.CommandName;
                        string CommandId = temp.CommandId;
                        string CommandSubId = temp.CommandSubId;
                        bool IsFloat = temp.IsFloat;
                        var data = new DataViewModel
                        {
                            CommandName = CommandName,
                            CommandId = CommandId,
                            CommandSubId = CommandSubId,
                            IsFloat = IsFloat,
                        };
                        BuildGroup[list.Key].Add(data);
                    }
                    else
                    {
                        var data = new DataViewModel
                        {
                            CommandName = ((DataViewModel)sub_list).CommandName,
                            CommandId = ((DataViewModel)sub_list).CommandId,
                            CommandSubId = ((DataViewModel)sub_list).CommandSubId,
                            CommandValue = ((DataViewModel)sub_list).CommandValue,
                            IsFloat = ((DataViewModel)sub_list).IsFloat,
                        };
                        BuildGroup[list.Key].Add(data);
                    }
                }
            }
            //foreach (var list in AllEnumList)
            //{
            //    foreach (var sub_list in list.Value)
            //    {
            //        var data = new DataViewModel
            //        {
            //            CommandName = ((DataViewModel)sub_list).CommandName,
            //            CommandId = ((DataViewModel)sub_list).CommandId,
            //            CommandSubId = ((DataViewModel)sub_list).CommandSubId,
            //            CommandValue = ((DataViewModel)sub_list).CommandValue,
            //            IsFloat = ((DataViewModel)sub_list).IsFloat,
            //        };
            //        BuildList.Add(new Tuple<int, int>(Int32.Parse(((DataViewModel)sub_list).CommandId), Int32.Parse(((DataViewModel)sub_list).CommandSubId)), data);
            //    }
            //}
            //foreach (var list in AllDataList)
            //{
            //    foreach (var sub_list in list.Value)
            //    {
            //        var data = new DataViewModel
            //        {
            //            CommandName = ((DataViewModel)sub_list).CommandName,
            //            CommandId = ((DataViewModel)sub_list).CommandId,
            //            CommandSubId = ((DataViewModel)sub_list).CommandSubId,
            //            CommandValue = ((DataViewModel)sub_list).CommandValue,
            //            IsFloat = ((DataViewModel)sub_list).IsFloat,
            //        };
            //        BuildList.Add(new Tuple<int, int>(Int32.Parse(((DataViewModel)sub_list).CommandId), Int32.Parse(((DataViewModel)sub_list).CommandSubId)), data);
            //    }
            //}
        }
        public string[] GroupToExecute(int tabIndex)
        {
            string[] PanelElements = new string[] { "MotionCommand List", "Profiler Mode", "S.G.List", "S.G.Type", "MotionStatus List", "Control", "Motor" };// , "Driver Type" "UpperMainPan List",
            // , "LPCommands List"
            switch(tabIndex)
            {
                case 0:
                    string[] arr = new string[] { "Motion Limit" }; // "Control", "Motor", already in PanelElements array
                    return arr.Concat(PanelElements).ToArray();
                case 1:
                    arr = new string[] { "Hall", "Qep1", "Qep2", "SSI_Feedback" };
                    return arr.Concat(PanelElements).ToArray();
                case 2:
                    arr = new string[] { "PIDCurrent", "PIDSpeed", "PIDPosition" };
                    return arr.Concat(PanelElements).ToArray();
                case 3:
                    arr = new string[] { "DeviceSerial" };
                    return arr.Concat(PanelElements).ToArray();
                //case 4:
                //    arr = new string[] { "DriverFullScale" };
                //    return arr.Concat(PanelElements).ToArray();
                case 4:
                    arr = new string[] { "Calibration Result List", "Calibration List" };
                    return arr.Concat(PanelElements).ToArray();
                //return PanelElements;
                case 5:
                    arr = new string[] { "CurrentLimit List" };
                    return arr.Concat(PanelElements).ToArray();
                case 6: // 7
                    arr = new string[] { "Maintenance List", "MaintenanceBool List" };
                    return arr.Concat(PanelElements).ToArray();
                case -1:
                    return PanelElements;
                default:
                    return new string[] { };
            }
        }
        public void StartRefresh()
        {
            if(!RefreshManger.DataPressed)//LeftPanelViewModel.GetInstance.EnRefresh && 
            {
                tab = Views.ParametarsWindow.ParametersWindowTabSelected;
                if(ParametarsWindow.WindowsOpen == false)
                    tab = -1;

                Debug.WriteLine("1: " + DateTime.Now.ToString("h:mm:ss.fff"));
                if(tab != TempTab)
                {
                    BuildList = new Dictionary<Tuple<int, int>, DataViewModel>();
                    foreach(var list in BuildGroup)
                    {
                        if(GroupToExecute(tab).Contains(list.Key))
                        {
                            foreach(var sub_list in list.Value)
                            {
                                var data = new DataViewModel
                                {
                                    CommandName = ((DataViewModel)sub_list).CommandName,
                                    CommandId = ((DataViewModel)sub_list).CommandId,
                                    CommandSubId = ((DataViewModel)sub_list).CommandSubId,
                                    CommandValue = ((DataViewModel)sub_list).CommandValue,
                                    IsFloat = ((DataViewModel)sub_list).IsFloat,
                                    IsSelected = ((DataViewModel)sub_list).IsSelected,
                                };
                                BuildList.Add(new Tuple<int, int>(Int32.Parse(((DataViewModel)sub_list).CommandId), Int32.Parse(((DataViewModel)sub_list).CommandSubId)), data);
                            }
                        }
                    }
                    TempTab = tab;
                    Debug.WriteLine(" --- Tab --- ");
                }

                Debug.WriteLine("2: " + DateTime.Now.ToString("h:mm:ss.fff"));
                foreach(var command in BuildList)
                {
                    if(!command.Value.IsSelected)
                    {
                        Rs232Interface.GetInstance.SendToParser(new PacketFields
                        {
                            Data2Send = command.Value.CommandValue,
                            ID = Convert.ToInt16(command.Value.CommandId),
                            SubID = Convert.ToInt16(command.Value.CommandSubId),
                            IsSet = false,
                            IsFloat = command.Value.IsFloat
                        });
                    }
                    else
                    {
                    }
                    Thread.Sleep(3);
                }
                Debug.WriteLine("3: " + DateTime.Now.ToString("h:mm:ss.fff"));
            }
        }

        string CalibrationGetStatus(string returnedValue)
        {
            switch(Convert.ToInt16(returnedValue))
            {
                case 0:
                    return "Idle";
                case 1:
                    return "in process";
                case 2:
                    return "failure";
                case 3:
                    return "success";
                default:
                    return "no info(" + returnedValue + ")";
            }
        }
        string CalibrationGetError(string returnedValue)
        {
            switch(Convert.ToInt32(returnedValue))
            {
                //uint16_t hallFeedlErr:1;         // 0x01 - 1
                //uint16_t encPhaseErr:1;          // 0x02 - 2
                //uint16_t encoderHallMismach:1;  // 0x04  - 4
                //uint16_t overTemperature:1;     // 0x08  - 8
                //uint16_t overVoltage:1;         // 0x010 - 16
                //uint16_t underVoltage:1;        //0x20   - 32
                //uint16_t speedRangeErr:1;       //0x40   - 64
                //uint16_t positionErr:1;         //0x80   - 128
                //uint16_t gateDriverFault:1;     //0x0100 - 256
                //uint16_t nOCTW:1;               //0x0200 - 512
                //uint16_t gateDriverInit:1;      //0x0400 - 1024
                //uint16_t motorStall:1;           //0x0800 - 2048
                //uint16_t Reserved3:1;           //0x1000 - 4096
                //uint16_t Reserved4:1;           //0x2000 - 8192
                //uint16_t ADCoffset:1;           //0x4000 - 16384
                //uint16_t FetShort:1;            //0x8000 - 32768
                case 0:
                    return "All OK !";
                case 1:
                    return "hallFeedlErr";
                case 2:
                    return "encPhaseErr";
                case 4:
                    return "encoderHallMismach";
                case 8:
                    return "overTemperature";
                case 16:
                    return "overVoltage";
                case 32:
                    return "underVoltage";
                case 64:
                    return "speedRangeErr";
                case 128:
                    return "positionErr";
                case 256:
                    return "gateDriverFault";
                case 512:
                    return "nOCTW";
                case 1024:
                    return "gateDriverInit";
                case 2048:
                    return "motorStall";
                case 4096:
                    return "Reserved3";
                case 8192:
                    return "Reserved4";
                case 16384:
                    return "ADCoffset";
                case 32768:
                    return "FetShort";
                default:
                    return "no info(" + returnedValue + ")";
            }
        }
        //public static int CalibrationTimeOut = 10;
        private static int PrecedentIdx = 0;
        internal void UpdateModel(Tuple<int, int> commandidentifier, string newPropertyValue)
        {
            #region Calibration_old
            //if(Commands.GetInstance.CalibartionCommandsList.ContainsKey(new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)))
            //{
            //    Commands.GetInstance.CalibartionCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2 - 1)].CommandValue = CalibrationGetStatus(newPropertyValue);
            //    if(LeftPanelViewModel.GetInstance.EnRefresh == false && Commands.GetInstance.CalibartionCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2 - 1)].ButtonContent == "Running")//newPropertyValue != "2" && newPropertyValue != "3"&& CalibrationTimeOut > 0
            //    {
            //        Rs232Interface.GetInstance.SendToParser(new PacketFields
            //        {
            //            ID = Convert.ToInt16(commandidentifier.Item1),
            //            SubID = Convert.ToInt16(commandidentifier.Item2),
            //            IsSet = false,
            //            IsFloat = false
            //        });
            //        //Thread.Sleep(100);
            //        //CalibrationTimeOut--;
            //    }
            //    //else if(CalibrationTimeOut <= 0)
            //    //{
            //    //    Commands.GetInstance.CalibartionCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2 - 1)].ButtonContent = "Run";
            //    //    Commands.GetInstance.CalibartionCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2 - 1)].CommandValue = "TimeOut";
            //    //}
            //    //else
            //    if(newPropertyValue == "2")
            //    {
            //        PrecedentIdx = commandidentifier.Item2;
            //        Rs232Interface.GetInstance.SendToParser(new PacketFields
            //        {
            //            ID = Convert.ToInt16(33),
            //            SubID = Convert.ToInt16(1),
            //            IsSet = false,
            //            IsFloat = false
            //        });
            //    }
            //    else if(newPropertyValue == "3")
            //        Commands.GetInstance.CalibartionCommandsList[new Tuple<int, int>(6, commandidentifier.Item2 - 1)].ButtonContent = "Run";
            //}
            //else if(commandidentifier.Item1 == 33) // Calibraion Status
            //{
            //    if(PrecedentIdx != 0)
            //    {
            //        Commands.GetInstance.CalibartionCommandsList[new Tuple<int, int>(6, PrecedentIdx - 1)].CommandValue = CalibrationGetError(newPropertyValue);
            //        Commands.GetInstance.CalibartionCommandsList[new Tuple<int, int>(6, PrecedentIdx - 1)].ButtonContent = "Run";
            //        PrecedentIdx = 0;
            //    }
            //    else
            //    {
            //        Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue = CalibrationGetError(newPropertyValue);
            //    }
            //}
            #endregion Calibration_old
            #region Calibration_new
            if(commandidentifier.Item1 == 6)
            {
                if(Commands.GetInstance.CalibartionCommandsList.ContainsKey(new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)))
                {
                    if(Convert.ToInt16(newPropertyValue) == 0)
                        Commands.GetInstance.CalibartionCommandsList[new Tuple<int, int>(6, commandidentifier.Item2)].ButtonContent = "Run";
                    else if(Convert.ToInt16(newPropertyValue) == 1)
                        Commands.GetInstance.CalibartionCommandsList[new Tuple<int, int>(6, commandidentifier.Item2)].ButtonContent = "Running";
                }
                else if(Commands.GetInstance.DataViewCommandsList.ContainsKey(new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)))
                {
                    //if(Convert.ToInt16(newPropertyValue) < 2)
                    Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue = CalibrationGetStatus(newPropertyValue);
                }

            }
            #endregion Calibration_new
            #region Plot_Channels
            else if(commandidentifier.Item1 == 60)
            {
                if(Int32.Parse(newPropertyValue) >= 0)
                {
                    int Sel = 0;
                    if(Int32.Parse(newPropertyValue) > 30)
                    {
                        Sel = Int32.Parse(newPropertyValue) - 3;
                    }
                    else
                        Sel = Int32.Parse(newPropertyValue);

                    if(commandidentifier.Item2 == 1)
                        OscilloscopeViewModel.GetInstance.SelectedCh1DataSource = OscilloscopeViewModel.GetInstance.Channel1SourceItems.ElementAt(Sel);
                    if(commandidentifier.Item2 == 2)
                        OscilloscopeViewModel.GetInstance.SelectedCh2DataSource = OscilloscopeViewModel.GetInstance.Channel2SourceItems.ElementAt(Sel);
                }
            }
            else if(commandidentifier.Item1 == 66)
            {
                if(commandidentifier.Item2 == 0)
                    OscilloscopeParameters.IfullScale = float.Parse(newPropertyValue);
                if(commandidentifier.Item2 == 1)
                    OscilloscopeParameters.VfullScale = float.Parse(newPropertyValue);

                OscilloscopeParameters.InitList();
            }
            #endregion Plot_Channels
            #region DataView_EnumView
            else if(commandidentifier.Item1 == 33)
            {
                Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue = CalibrationGetError(newPropertyValue);
            }
            else if(commandidentifier.Item1 == 1 && commandidentifier.Item2 == 0) // MotorStatus
            {
                if(newPropertyValue == "1" || newPropertyValue == "0")
                    LeftPanelViewModel.GetInstance.LedMotorStatus = Convert.ToInt16(newPropertyValue);
            }
            else if(commandidentifier.Item1 == 65 && commandidentifier.Item2 == 0) // EnableLoader
            {
                MaintenanceViewModel.GetInstance.EnableLoder = (newPropertyValue == 0.ToString()) ? false : true;
            }
            else if(Commands.GetInstance.DataViewCommandsList.ContainsKey(new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)))
            {
                Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue = newPropertyValue;
                if(commandidentifier.Item1 == 62 && commandidentifier.Item2 < 3)
                    Commands.GetInstance.DataViewCommandsListLP[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue = newPropertyValue;
            }
            else if(Commands.GetInstance.EnumViewCommandsList.ContainsKey(new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)))
            {
                int index = Convert.ToInt16(newPropertyValue) - Convert.ToInt16(Commands.GetInstance.EnumViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue);
                if(index < Commands.GetInstance.EnumViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandList.Count && index >= 0)
                {
                    Commands.GetInstance.EnumViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].SelectedValue =
                    Commands.GetInstance.EnumViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandList[index];
                }
            }
            #endregion DataView_EnumView
            #region Motor_ON/OFF
            //else if(commandidentifier.Item1 == 1)
            //{
            //    if(commandidentifier.Item2 == 0)
            //    {
            //        LeftPanelViewModel.MotorOnOff_flag = true;
            //        LeftPanelViewModel.GetInstance.MotorOnToggleChecked = (newPropertyValue == 0.ToString()) ? false : true;
            //    }
            //}
            #endregion Motor_ON/OFF
            #region StartUpAPP
            else if(commandidentifier.Item1 == 62)
            {
                switch(commandidentifier.Item2)
                {
                    case 0:
                        Commands.GetInstance.DataViewCommandsListLP[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue = newPropertyValue;
                        break;
                    case 1:
                        Commands.GetInstance.DataViewCommandsListLP[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue = newPropertyValue;
                        break;
                    case 2:
                        Commands.GetInstance.DataViewCommandsListLP[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue = newPropertyValue;
                        break;
                    case 3:
                        Commands.GetInstance.DataViewCommandsListLP[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue = newPropertyValue;
                        break;
                }
            }
            #endregion StartUpAPP
        }
    }
}
