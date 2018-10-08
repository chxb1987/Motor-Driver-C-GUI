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
                case 4:
                    arr = new string[] { "DriverFullScale" };
                    return arr.Concat(PanelElements).ToArray();
                case 5:
                    arr = new string[] { "CalibrationCommands List" };
                    return arr.Concat(PanelElements).ToArray();
                case 6:
                    arr = new string[] { "CurrentLimit List" };
                    return arr.Concat(PanelElements).ToArray();
                case 10: // 7
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
                BuildList = new Dictionary<Tuple<int, int>, DataViewModel>();

                tab = Views.ParametarsWindow.ParametersWindowTabSelected;
                if (ParametarsWindow.WindowsOpen == false)
                    tab = -1;

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

                foreach (var command in BuildList)
                {
                    if (command.Value.CommandId == "70" || (command.Value.CommandId == "53"))
                    {
                        //int k = 0;
                    }//|| (command.Value.CommandId == "83"))

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

        internal void UpdateModel(Tuple<int, int> commandidentifier, string newPropertyValue)
        {
            EventRiser.Instance.RiseEventLedRx(RoundBoolLed.PASSED);
            //Thread.Sleep(1);
            if (commandidentifier.Item1 == 6)
            {
                switch (commandidentifier.Item2)
                {
                    case 2:
                        CalibrationViewModel.GetInstance.offsetCalVal = newPropertyValue.ToString();
                        break;
                    case 4:
                        CalibrationViewModel.GetInstance.PICurrentCalVal = newPropertyValue.ToString();
                        break;
                    case 6:
                        CalibrationViewModel.GetInstance.HallCalVal = newPropertyValue.ToString();
                        break;
                    case 8:
                        CalibrationViewModel.GetInstance.Encoder1CalVal = newPropertyValue.ToString();
                        break;
                    case 10:
                        CalibrationViewModel.GetInstance.PISpeedCalVal = newPropertyValue.ToString();
                        break;
                    case 12:
                        CalibrationViewModel.GetInstance.PIPosCalVal = newPropertyValue.ToString();
                        break;
                    default:
                        break;
                }
            }
            if (commandidentifier.Item1 == 100 && commandidentifier.Item2 == 12)
            {
                CalibrationViewModel.CalibrationProcessing = false;
                Application.Current.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    CalibrationViewModel.GetInstance.PiPositionCalCheck = false;
                }));
            }

            if (commandidentifier.Item1 == 63)
            {
                //switch (commandidentifier.Item2)
                //{
                //    case 0:
                //        MaintenanceViewModel.GetInstance.Save = (newPropertyValue == 0.ToString()) ? false : true;
                //        break;
                //    case 1:
                //        MaintenanceViewModel.GetInstance.Manufacture = (newPropertyValue == 0.ToString()) ? false : true;
                //        break;
                //    case 2:
                //        MaintenanceViewModel.GetInstance.Reboot = (newPropertyValue == 0.ToString()) ? false : true;
                //        break;
                //    case 10:
                //        MaintenanceViewModel.GetInstance.EnableWrite = (newPropertyValue == 0.ToString()) ? false : true;
                //        break;
                //    default:
                //        break;
                //}
            }
            if(commandidentifier.Item1 == 60)
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

            if (commandidentifier.Item1 == 53 || commandidentifier.Item1 == 70)
            {
                //int k=0;
                //k = 0;
            }
            if (commandidentifier.Item1 == 122 || commandidentifier.Item1 == 0x64)
            {

            }
            if (Commands.GetInstance.DataViewCommandsList.ContainsKey(new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)))
            {
                Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue =
                    newPropertyValue;
            }

            if (Commands.GetInstance.EnumViewCommandsList.ContainsKey(new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)))
            {
                Commands.GetInstance.EnumViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue =
                    newPropertyValue;

            }
            if (commandidentifier.Item1 == 1)
            {
                if (commandidentifier.Item2 == 0)
                {
                    LeftPanelViewModel.MotorOnOff_flag = true;
                    LeftPanelViewModel.GetInstance.MotorOnToggleChecked = (newPropertyValue == 0.ToString()) ? false : true;
                }
            }
            else
            {
                //OperationViewModel.GetInstance.MotorDriver = newPropertyValue;
            }
            EventRiser.Instance.RiseEventLedRx(RoundBoolLed.IDLE);

        }

    }
}
