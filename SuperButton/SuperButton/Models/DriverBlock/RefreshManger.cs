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
        public static RefreshManger GetInstance
        {
            get
            {
                lock (Synlock)
                {
                    if (_instance != null) return _instance;
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


            foreach (var list in AllEnumList)
            {
                BuildGroup.Add(list.Key, new ObservableCollection<object>());
                foreach (var sub_list in list.Value)
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

            foreach (var list in AllDataList)
            {
                BuildGroup.Add(list.Key, new ObservableCollection<object>());
                foreach (var sub_list in list.Value)
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
        private string[] GroupToExecute(int tabIndex)
        {
            string[] PanelElements = new string[] { "UpperMainPan List", "MotionCommand List", "Profiler Mode", "S.G.List", "S.G.Type", "Driver Type", "MotionStatus List" };
            // , "LPCommands List"
            switch (tabIndex)
            {
                case 0:
                    string[] arr = new string[] { "Control", "Motor", "Motion Limit" };
                    return arr.Concat(PanelElements).ToArray();
                case 1:
                    arr = new string[] { "Hall", "Qep1", "Qep2", "SSI_Feedback" };
                    return arr.Concat(PanelElements).ToArray();
                case 2:
                    arr = new string[] { "PIDCurrent", "PIDSpeed", "PIDPosition", "HallMap" };
                    return arr.Concat(PanelElements).ToArray();
                case 3:
                    arr = new string[] { "DeviceSerial" };
                    return arr.Concat(PanelElements).ToArray();
                //case 4:
                //    arr = new string[] { "DriverFullScale" };
                //    return arr.Concat(PanelElements).ToArray();
                case 4:
                    arr = new string[] { "CalibrationCommands List" };
                    return arr.Concat(PanelElements).ToArray();
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
            if (LeftPanelViewModel.GetInstance.EnRefresh)
            {
                tab = Views.ParametarsWindow.ParametersWindowTabSelected;
                if (ParametarsWindow.WindowsOpen == false)
                    tab = -1;

                if (tab != TempTab)
                {
                    BuildList = new Dictionary<Tuple<int, int>, DataViewModel>();
                    foreach (var list in BuildGroup)
                    {
                        if (GroupToExecute(tab).Contains(list.Key))
                        {
                            foreach (var sub_list in list.Value)
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
                }

                foreach (var command in BuildList)
                {
                    //if (command.Value.CommandId == "70" || (command.Value.CommandId == "53"))
                    //{
                    //    //int k = 0;
                    //}//|| (command.Value.CommandId == "83"))

                    //  if (command.Value.CommandName == "Serial Number")
                    //{
                    if (!command.Value.IsSelected)
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
                    else { }
                    // }
                    Thread.Sleep(10);
                }
            }
        }

        string CalibrationGetStatus(string returnedValue)
        {
            switch (Convert.ToInt16(returnedValue))
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
            switch (Convert.ToInt32(returnedValue))
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
                    return "no info";
            }
        }
        public static int CalibrationTimeOut = 10;
        private static int PrecedentIdx = 0;
        internal void UpdateModel(Tuple<int, int> commandidentifier, string newPropertyValue)
        {
            if (commandidentifier.Item1 == 6)
            {
                switch (commandidentifier.Item2)
                {
                    case 2:
                        CalibrationViewModel.GetInstance.OffsetCalVal = CalibrationGetStatus(newPropertyValue);
                        if (newPropertyValue != "2" && newPropertyValue != "3" && CalibrationTimeOut > 0)
                        {
                            Rs232Interface.GetInstance.SendToParser(new PacketFields
                            {
                                ID = Convert.ToInt16(6),
                                SubID = Convert.ToInt16(2),
                                IsSet = false,
                                IsFloat = false
                            });
                            Thread.Sleep(100);
                            CalibrationTimeOut--;
                        }
                        else if(CalibrationTimeOut <= 0)
                        {
                            CalibrationViewModel.CalibrationProcessing = false;
                            CalibrationViewModel.GetInstance.OffsetCalContent = "Run";
                            CalibrationViewModel.GetInstance.OffsetCalVal = "TimeOut";
                        }
                        break;
                    case 4:
                        CalibrationViewModel.GetInstance.PiCurrentCalVal = CalibrationGetStatus(newPropertyValue);
                        if (newPropertyValue != "2" && newPropertyValue != "3" && CalibrationTimeOut > 0)
                        {
                            Rs232Interface.GetInstance.SendToParser(new PacketFields
                            {
                                ID = Convert.ToInt16(6),
                                SubID = Convert.ToInt16(4),
                                IsSet = false,
                                IsFloat = false
                            });
                            Thread.Sleep(100);
                            CalibrationTimeOut--;
                        }
                        else if (CalibrationTimeOut <= 0)
                        {
                            CalibrationViewModel.CalibrationProcessing = false;
                            CalibrationViewModel.GetInstance.PiCurrentCalContent = "Run";
                            CalibrationViewModel.GetInstance.PiCurrentCalVal = "TimeOut";
                        }
                        break;
                    case 6:
                        CalibrationViewModel.GetInstance.HallMapCalVal = CalibrationGetStatus(newPropertyValue);
                        if (newPropertyValue != "2" && newPropertyValue != "3" && CalibrationTimeOut > 0)
                        {
                            Rs232Interface.GetInstance.SendToParser(new PacketFields
                            {
                                ID = Convert.ToInt16(6),
                                SubID = Convert.ToInt16(6),
                                IsSet = false,
                                IsFloat = false
                            });
                            Thread.Sleep(100);
                            CalibrationTimeOut--;
                        }
                        else if (CalibrationTimeOut <= 0)
                        {
                            CalibrationViewModel.CalibrationProcessing = false;
                            CalibrationViewModel.GetInstance.HallMapCalContent = "Run";
                            CalibrationViewModel.GetInstance.HallMapCalVal = "TimeOut";
                        }
                        break;
                    case 8:
                        CalibrationViewModel.GetInstance.Encoder1CalVal = CalibrationGetStatus(newPropertyValue);
                        if (newPropertyValue != "2" && newPropertyValue != "3" && CalibrationTimeOut > 0)
                        {
                            Rs232Interface.GetInstance.SendToParser(new PacketFields
                            {
                                ID = Convert.ToInt16(6),
                                SubID = Convert.ToInt16(8),
                                IsSet = false,
                                IsFloat = false
                            });
                            Thread.Sleep(100);
                            CalibrationTimeOut--;
                        }
                        else if (CalibrationTimeOut <= 0)
                        {
                            CalibrationViewModel.CalibrationProcessing = false;
                            CalibrationViewModel.GetInstance.Encoder1CalContent = "Run";
                            CalibrationViewModel.GetInstance.Encoder1CalVal = "TimeOut";
                        }
                        break;
                    case 10:
                        CalibrationViewModel.GetInstance.PiSpeedCalVal = CalibrationGetStatus(newPropertyValue);
                        if (newPropertyValue != "2" && newPropertyValue != "3" && CalibrationTimeOut > 0)
                        {
                            Rs232Interface.GetInstance.SendToParser(new PacketFields
                            {
                                ID = Convert.ToInt16(6),
                                SubID = Convert.ToInt16(10),
                                IsSet = false,
                                IsFloat = false
                            });
                            Thread.Sleep(100);
                            CalibrationTimeOut--;
                        }
                        else if (CalibrationTimeOut <= 0)
                        {
                            CalibrationViewModel.CalibrationProcessing = false;
                            CalibrationViewModel.GetInstance.PiSpeedCalContent = "Run";
                            CalibrationViewModel.GetInstance.PiSpeedCalVal = "TimeOut";
                        }
                        break;
                    case 12:
                        CalibrationViewModel.GetInstance.PiPosCalVal = CalibrationGetStatus(newPropertyValue);
                        if (newPropertyValue != "2" && newPropertyValue != "3" && CalibrationTimeOut > 0)
                        {
                            Rs232Interface.GetInstance.SendToParser(new PacketFields
                            {
                                ID = Convert.ToInt16(6),
                                SubID = Convert.ToInt16(12),
                                IsSet = false,
                                IsFloat = false
                            });
                            Thread.Sleep(100);
                            CalibrationTimeOut--;
                        }
                        else if (CalibrationTimeOut <= 0)
                        {
                            CalibrationViewModel.CalibrationProcessing = false;
                            CalibrationViewModel.GetInstance.PiPosCalContent = "Run";
                            CalibrationViewModel.GetInstance.PiPosCalVal = "TimeOut";
                        }
                        break;
                    default:
                        break;
                }
                if (newPropertyValue == "2")
                {
                    PrecedentIdx = commandidentifier.Item2;
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        ID = Convert.ToInt16(33),
                        SubID = Convert.ToInt16(1),
                        IsSet = false,
                        IsFloat = false
                    });
                }
            }
            if (commandidentifier.Item1 == 33)
            {
                CalibrationViewModel.CalibrationProcessing = false;
                switch (PrecedentIdx)
                {
                    case 2:
                        CalibrationViewModel.GetInstance.OffsetCalVal = CalibrationGetError(newPropertyValue);
                        CalibrationViewModel.GetInstance.OffsetCalContent = "Run";
                        break;
                    case 4:
                        CalibrationViewModel.GetInstance.PiCurrentCalVal = CalibrationGetError(newPropertyValue);
                        CalibrationViewModel.GetInstance.PiCurrentCalContent = "Run";
                        break;
                    case 6:
                        CalibrationViewModel.GetInstance.HallMapCalVal = CalibrationGetError(newPropertyValue);
                        CalibrationViewModel.GetInstance.HallMapCalContent = "Run";
                        break;
                    case 8:
                        CalibrationViewModel.GetInstance.Encoder1CalVal = CalibrationGetError(newPropertyValue);
                        CalibrationViewModel.GetInstance.Encoder1CalContent = "Run";
                        break;
                    case 10:
                        CalibrationViewModel.GetInstance.PiSpeedCalVal = CalibrationGetError(newPropertyValue);
                        CalibrationViewModel.GetInstance.PiSpeedCalContent = "Run";
                        break;
                    case 12:
                        CalibrationViewModel.GetInstance.PiPosCalVal = CalibrationGetError(newPropertyValue);
                        CalibrationViewModel.GetInstance.PiPosCalContent = "Run";
                        break;
                    default:
                        break;
                }
            }

            if (commandidentifier.Item1 == 60)
            {
                if (Int32.Parse(newPropertyValue) >= 0)
                {
                    int Sel = 0;
                    if (Int32.Parse(newPropertyValue) > 30)
                    {
                        Sel = Int32.Parse(newPropertyValue) - 3;
                    }
                    else
                        Sel = Int32.Parse(newPropertyValue);

                    if (commandidentifier.Item2 == 1)
                        OscilloscopeViewModel.GetInstance.SelectedCh1DataSource = OscilloscopeViewModel.GetInstance.Channel1SourceItems.ElementAt(Sel); //newPropertyValue;
                    if (commandidentifier.Item2 == 2)
                        OscilloscopeViewModel.GetInstance.SelectedCh2DataSource = OscilloscopeViewModel.GetInstance.Channel2SourceItems.ElementAt(Sel);
                }
            }
            if (commandidentifier.Item1 == 66)
            {
                if (commandidentifier.Item2 == 0)
                    OscilloscopeParameters.IfullScale = float.Parse(newPropertyValue);
                if (commandidentifier.Item2 == 1)
                    OscilloscopeParameters.VfullScale = float.Parse(newPropertyValue);

                OscilloscopeParameters.InitList();
            }
            if (commandidentifier.Item1 == 65 && commandidentifier.Item2 == 0)
            {
                MaintenanceViewModel.GetInstance.EnableLoder = (newPropertyValue == 0.ToString()) ? false : true;
            }
            if (Commands.GetInstance.DataViewCommandsList.ContainsKey(new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)))
            {
                Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue =
                    newPropertyValue;
            }

            if (Commands.GetInstance.EnumViewCommandsList.ContainsKey(new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)))
            {
                int index = Convert.ToInt16(newPropertyValue) - Convert.ToInt16(Commands.GetInstance.EnumViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue);
                if (index < Commands.GetInstance.EnumViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandList.Count && index >= 0)
                {
                    Commands.GetInstance.EnumViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].SelectedValue =
                    Commands.GetInstance.EnumViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandList[index];
                }
            }
            if (commandidentifier.Item1 == 1)
            {
                if (commandidentifier.Item2 == 0)
                {
                    LeftPanelViewModel.MotorOnOff_flag = true;
                    LeftPanelViewModel.GetInstance.MotorOnToggleChecked = (newPropertyValue == 0.ToString()) ? false : true;
                }
            }
            if (commandidentifier.Item1 == 62)
            {
                switch (commandidentifier.Item2)
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
        }
    }
}
