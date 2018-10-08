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

namespace SuperButton.Models.DriverBlock
{

    public class RefreshManger
    {
        public static int tab = Views.ParametarsWindow.ParametersWindowTabSelected;
        private static readonly object Synlock = new object();
        private static RefreshManger _instance;

        private static Dictionary<string, ObservableCollection<object>> BuildGroup = new Dictionary<string, ObservableCollection<object>>();

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
            switch (tabIndex)
            {
                case 0:
                    return new string[] { "Control", "Motor", "Motion Limit" };
                case 1:
                    return new string[] { "Hall", "Qep1", "Qep2", "SSI_Feedback", "Digital", "Analog" };
                case 2:
                    return new string[] { "PIDCurrent", "PIDSpeed", "PIDPosition" };
                case 3:
                    return new string[] { "DeviceSerial" };
                case 4:
                    return new string[] { "DriverFullScale" };
                case 5:
                    return new string[] { "CalibrationCommands List" };
                case -1:
                    return new string[] { "RPCommands List" };
                default:
                    return new string[] { };
            }
        }
        public void StartRefresh()
        {
            Dictionary<Tuple<int, int>, DataViewModel> BuildList = new Dictionary<Tuple<int, int>, DataViewModel>();
            tab = Views.ParametarsWindow.ParametersWindowTabSelected;
            if (ParametarsWindow.WindowsOpen == false)
                tab = -1;

            foreach (var list in BuildGroup)
            {
                if (GroupToExecute(tab).Contains(list.Key)) // list.Key == "Control" || list.Key == "Motor" || list.Key == "Motion Limit"
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
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = command.Value.CommandValue,
                    ID = Convert.ToInt16(command.Value.CommandId),
                    SubID = Convert.ToInt16(command.Value.CommandSubId),
                    IsSet = false,
                    IsFloat = command.Value.IsFloat
                }
                );
                // }

                Thread.Sleep(500);
            }

        }

        internal void UpdateModel(Tuple<int, int> commandidentifier, string newPropertyValue)
        {

            if (commandidentifier.Item1 == 6)
            {
                switch (commandidentifier.Item2)
                {
                    case 1:
                        CalibrationViewModel.GetInstance.offsetCalVal = newPropertyValue.ToString();
                        break;
                    case 2:
                        CalibrationViewModel.GetInstance.PICurrentCalVal = newPropertyValue.ToString();
                        break;
                    case 3:
                        CalibrationViewModel.GetInstance.PISpeedCalVal = newPropertyValue.ToString();
                        break;
                    case 4:
                        CalibrationViewModel.GetInstance.HallCalVal = newPropertyValue.ToString();
                        break;
                    case 5:
                        CalibrationViewModel.GetInstance.Encoder1CalVal = newPropertyValue.ToString();
                        break;
                    case 6:
                        CalibrationViewModel.GetInstance.PIPosCalVal = newPropertyValue.ToString();
                        break;
                    default:
                        break;
                }
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
                Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue =
                    newPropertyValue;

            }
            else
            {
                //OperationViewModel.GetInstance.MotorDriver = newPropertyValue;
            }
        }

    }
}
