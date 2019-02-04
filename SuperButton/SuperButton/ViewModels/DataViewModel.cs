using System;
using System.Windows.Input;
using SuperButton.Models;
using SuperButton.Models.DriverBlock;
using MathNet.Numerics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Threading.Tasks;

namespace SuperButton.ViewModels
{
    public class DataViewModel : ViewModelBase
    {


        private readonly BaseModel _baseModel = new BaseModel();
        ICommand _mouseLeftClickCommand;
        ICommand _mouseLeaveCommand;
        private SolidColorBrush _backgroundSelected = new SolidColorBrush(Colors.Gray);
        private SolidColorBrush _backgroundSelected2 = new SolidColorBrush(Colors.White);

        private string _commandvalueTemp;
        private bool _escPressed = true;

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
        public virtual ICommand ResetValue
        {
            get
            {
                return _mouseLeaveCommand ?? (_mouseLeaveCommand = new RelayCommand(MouseLeaveCommandFunc));
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
                if (CommandValue != "")
                {
                    Task.Factory.StartNew(action: () =>
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
                        _escPressed = false;
                        MouseLeaveCommandFunc();
                        _escPressed = true;
                    Debug.WriteLine("{0} {1}[{2}]={3} {4}.", "Set", Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId), CommandValue, IsFloat ? "F":"I");
                });
                }
                else
                {
                    MouseLeaveCommandFunc();
                }
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

        static string cmdClicked = "";
        private void MouseLeftClickFunc()
        {
            if (LeftPanelViewModel.GetInstance.ConnectButtonContent == "Disconnect")
            {
                Dictionary<Tuple<int, int>, DataViewModel> TempList = new Dictionary<Tuple<int, int>, DataViewModel>();
                bool Flag = false, Selected = false;
                string KeyStr = "";
                this.IsSelected = true;
                this.EnableTextBox = false;
                this.ReadOnly = true;
                _commandvalueTemp = this.CommandValue;
                bool selectExist = false;
                foreach (var group in RefreshManger.BuildGroup)
                {
                    foreach (var sub_list in group.Value)
                    {
                        if (this.CommandName != ((DataViewModel)sub_list).CommandName)
                        {
                            if (((DataViewModel)sub_list).IsSelected)
                            {
                                selectExist = true;
                                break;
                            }
                        }
                    }
                }
                if (!selectExist)
                {
                    this.Background = this.Background2 = new SolidColorBrush(Colors.Red);
                    this.ReadOnly = false;
                    this.EnableTextBox = true;


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
                            {
                                Selected = false;
                            }
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

        private void MouseLeaveCommandFunc()
        {
            Dictionary<Tuple<int, int>, DataViewModel> TempList = new Dictionary<Tuple<int, int>, DataViewModel>();
            bool Flag = false, Selected = false;
            string KeyStr = "";
            this.IsSelected = false;
            if (_escPressed)
                this.CommandValue = _commandvalueTemp;
            this.Background = new SolidColorBrush(Colors.Gray);
            this.Background2 = new SolidColorBrush(Colors.White);


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
        public SolidColorBrush Background
        {
            get
            {
                if (!IsSelected)
                    return new SolidColorBrush(Colors.Gray);
                else return new SolidColorBrush(Colors.Red);
            }
            set
            {
                if (IsSelected)
                    _baseModel.Background = new SolidColorBrush(Colors.Red);
                else
                    _baseModel.Background = new SolidColorBrush(Colors.Gray);

                OnPropertyChanged("Background");
            }

        }
        public SolidColorBrush Background2
        {
            get
            {
                if (!IsSelected)
                    return new SolidColorBrush(Colors.White);
                else return new SolidColorBrush(Colors.Red);
            }
            set
            {
                if (IsSelected)
                    _baseModel.Background = new SolidColorBrush(Colors.Red);
                else
                    _baseModel.Background = new SolidColorBrush(Colors.White);

                OnPropertyChanged("Background2");
            }

        }
        public bool ReadOnly
        {
            get { if (!IsSelected) return true; else return false; }
            set { if (!IsSelected) _baseModel.ReadOnly = true; else _baseModel.ReadOnly = false; OnPropertyChanged("ReadOnly"); }
        }
        public bool EnableTextBox { get { return true; } set { if (IsSelected) _baseModel.EnableTextBox = true; else _baseModel.EnableTextBox = false; OnPropertyChanged("EnableTextBox"); } }
    }
}
