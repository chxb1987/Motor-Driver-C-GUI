using System;
using System.Windows.Input;
using SuperButton.Models;
using SuperButton.Models.DriverBlock;
using MathNet.Numerics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SuperButton.ViewModels
{
    public class DataViewModel : ViewModelBase
    {


        private readonly BaseModel _baseModel = new BaseModel();
        ICommand _mouseLeftClickCommand;
        ICommand _mouseLeaveCommand;

        public string CommandName { get { return _baseModel.CommandName; } set { _baseModel.CommandName = value; } }

        public string CommandValue
        {
            get
            {
                return _baseModel.CommandValue;
            }
            set
            {
                //if (LeftPanelViewModel.flag == true)
                //{
                //    if (_baseModel.CommandName == "Pole Pair" && (Int32.Parse(value) < 0))
                //        _baseModel.CommandValue = "0";
                //    else
                //        _baseModel.CommandValue = value;
                //}
                //else
                _baseModel.CommandValue = value;
                OnPropertyChanged();


            }
        }

        public string CommandId { get { return _baseModel.CommandID; } set { _baseModel.CommandID = value; } }

        public string CommandSubId { get { return _baseModel.CommandSubID; } set { _baseModel.CommandSubID = value; } }

        public bool IsFloat { get { return _baseModel.IsFloat; } set { _baseModel.IsFloat = value; } }

        public bool IsSelected { get { return _baseModel.IsSelected; } set { _baseModel.IsSelected = value; OnPropertyChanged(); } }
        public virtual ICommand SendData
        {
            get
            {
                return new RelayCommand(BuildPacketTosend, CheckValue);
            }
        }
        private bool CheckValue()
        {
            return true;
        }

        private void BuildPacketTosend()
        {
            if (LeftPanelViewModel.GetInstance.ConnectButtonContent == "Disconnect")
            {
                var tmp = new PacketFields
                {
                    Data2Send = CommandValue,
                    ID = Convert.ToInt16(CommandId),
                    SubID = Convert.ToInt16(CommandSubId),
                    IsSet = true,
                    IsFloat = IsFloat,
                };
                Rs232Interface.GetInstance.SendToParser(tmp);
            }
        }

        public ICommand MouseLeftClickCommand
        {
            get
            {
                return _mouseLeftClickCommand ?? (_mouseLeftClickCommand = new RelayCommand(MouseLeftClickFunc));
            }
        }
        public ICommand MouseLeaveCommand
        {
            get
            {
                return _mouseLeaveCommand ?? (_mouseLeaveCommand = new RelayCommand(MouseLeaveCommandFunc));
            }
        }
        private void MouseLeftClickFunc()
        {
            Dictionary<Tuple<int, int>, DataViewModel> TempList = new Dictionary<Tuple<int, int>, DataViewModel>();
            bool Flag = false, Selected = false;
            string KeyStr = "";

            foreach (var group in RefreshManger.BuildGroup)
            {
                TempList = new Dictionary<Tuple<int, int>, DataViewModel>();
                foreach (var sub_list in group.Value)
                {
                    if (this.CommandName == ((DataViewModel)sub_list).CommandName)
                    {
                        Selected = true;
                        Flag = true;
                    }
                    else
                        Selected = false;
                    var data = new DataViewModel
                    {
                        CommandName = ((DataViewModel)sub_list).CommandName,
                        CommandId = ((DataViewModel)sub_list).CommandId,
                        CommandSubId = ((DataViewModel)sub_list).CommandSubId,
                        CommandValue = ((DataViewModel)sub_list).CommandValue,
                        IsFloat = ((DataViewModel)sub_list).IsFloat,
                        IsSelected = Selected,
                    };
                    try
                    {
                        TempList.Add(new Tuple<int, int>(Int32.Parse(((DataViewModel)sub_list).CommandId), Int32.Parse(((DataViewModel)sub_list).CommandSubId)), data);
                    }
                    catch (Exception e)
                    {

                    }

                }
                if (Flag)
                {
                    KeyStr = group.Key;
                    Flag = false;
                    break;
                }
            }

            RefreshManger.BuildGroup.Remove(KeyStr);
            RefreshManger.BuildGroup.Add(KeyStr, new ObservableCollection<object>());
            foreach (var sub_list in TempList.Values)
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
                RefreshManger.BuildGroup[KeyStr].Add(data);
            }
        }

        private void MouseLeaveCommandFunc()
        {
            Dictionary<Tuple<int, int>, DataViewModel> TempList = new Dictionary<Tuple<int, int>, DataViewModel>();
            bool Flag = false, Selected = false;
            string KeyStr = "";

            foreach (var group in RefreshManger.BuildGroup)
            {
                TempList = new Dictionary<Tuple<int, int>, DataViewModel>();
                foreach (var sub_list in group.Value)
                {
                    if (this.CommandName == ((DataViewModel)sub_list).CommandName)
                    {
                        Selected = false;
                        Flag = true;
                    }
                    else
                        Selected = false;
                    var data = new DataViewModel
                    {
                        CommandName = ((DataViewModel)sub_list).CommandName,
                        CommandId = ((DataViewModel)sub_list).CommandId,
                        CommandSubId = ((DataViewModel)sub_list).CommandSubId,
                        CommandValue = ((DataViewModel)sub_list).CommandValue,
                        IsFloat = ((DataViewModel)sub_list).IsFloat,
                        IsSelected = Selected,
                    };
                    try
                    {
                        TempList.Add(new Tuple<int, int>(Int32.Parse(((DataViewModel)sub_list).CommandId), Int32.Parse(((DataViewModel)sub_list).CommandSubId)), data);
                    }
                    catch (Exception e)
                    {

                    }

                }
                if (Flag)
                {
                    KeyStr = group.Key;
                    Flag = false;
                    break;
                }
            }

            RefreshManger.BuildGroup.Remove(KeyStr);
            RefreshManger.BuildGroup.Add(KeyStr, new ObservableCollection<object>());
            foreach (var sub_list in TempList.Values)
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
                RefreshManger.BuildGroup[KeyStr].Add(data);
            }
        }
    }
}
