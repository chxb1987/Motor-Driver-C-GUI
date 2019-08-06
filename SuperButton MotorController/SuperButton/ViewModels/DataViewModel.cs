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
using System.Linq;
using SuperButton.CommandsDB;

namespace SuperButton.ViewModels
{
    public class DataViewModel : ViewModelBase
    {


        private readonly BaseModel _baseModel = new BaseModel();
        ICommand _mouseLeftClickCommand;
        ICommand _mouseLeaveCommand;
        private SolidColorBrush _backgroundSmallFontSelected = new SolidColorBrush(Colors.Gray);

        public string CommandName { get { return _baseModel.CommandName; } set { _baseModel.CommandName = value; } }

        public string CommandValue
        {
            get
            {
                return _baseModel.CommandValue;
            }
            set
            {
                _baseModel.CommandValue = value;
                OnPropertyChanged();
            }
        }

        public string CommandId { get { return _baseModel.CommandID; } set { _baseModel.CommandID = value; } }

        public string CommandSubId { get { return _baseModel.CommandSubID; } set { _baseModel.CommandSubID = value; } }

        public bool IsFloat { get { return _baseModel.IsFloat; } set { _baseModel.IsFloat = value; } }

        public bool IsSelected
        {
            get { return _baseModel.IsSelected; }
            set
            {
                _baseModel.IsSelected = value;
                OnPropertyChanged();
            }
        }
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

            if(LeftPanelViewModel.GetInstance.ConnectButtonContent == "Disconnect")
            {

                Debug.WriteLine("Enter to Send");

                RefreshManger.DataPressed = false;
                if(Commands.GetInstance.DataViewCommandsList.ContainsKey(new Tuple<int, int>(Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId))))
                {
                    Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId))].IsSelected = false;
                    Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId))].BackgroundStd = new SolidColorBrush(Colors.White);
                    Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId))].BackgroundSmallFont = new SolidColorBrush(Colors.Gray);
                    this.ReadOnly = true;
                    this.EnableTextBox = false;
                }
                if(CommandValue != "")
                {
                    //bool _escPressed = true;
                    var tmp = new PacketFields
                    {
                        Data2Send = CommandValue,
                        ID = Convert.ToInt16(CommandId),
                        SubID = Convert.ToInt16(CommandSubId),
                        IsSet = true,
                        IsFloat = IsFloat,
                    };
                    Task.Factory.StartNew(action: () => { Rs232Interface.GetInstance.SendToParser(tmp); });
                    //_escPressed = false;
                    MouseLeaveCommandFunc();
                    //_escPressed = true;
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

        //static string cmdClicked = "";

        //int cmdID = 0, cmdSubID = 0;

        private void MouseLeftClickFunc()
        {
            if(LeftPanelViewModel.GetInstance.ConnectButtonContent == "Disconnect")
            {
                RefreshManger.DataPressed = true;
                foreach(var list in Commands.GetInstance.DataViewCommandsList)
                {
                    try
                    {
                        Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(list.Value.CommandId), Convert.ToInt16(list.Value.CommandSubId))].IsSelected = false;
                        Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(list.Value.CommandId), Convert.ToInt16(list.Value.CommandSubId))].BackgroundStd = new SolidColorBrush(Colors.White);
                        Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(list.Value.CommandId), Convert.ToInt16(list.Value.CommandSubId))].BackgroundSmallFont = new SolidColorBrush(Colors.Gray);
                    }
                    catch(Exception)
                    {
                    }
                }
                if(Commands.GetInstance.DataViewCommandsList.ContainsKey(new Tuple<int, int>(Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId))))
                {
                    //SuperButton.Models.DriverBlock.RefreshManger.GetInstance._oneSelected = true;
                    Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId))].IsSelected = true;
                    Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId))].BackgroundStd = new SolidColorBrush(Colors.Red);
                    Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId))].BackgroundSmallFont = new SolidColorBrush(Colors.Red);
                    this.ReadOnly = false;
                    this.EnableTextBox = true;
                }
            }
            #region Not_Enabled
            //int i = 0;
            //if(i == 1)
            //{
            //    if(LeftPanelViewModel.GetInstance.ConnectButtonContent == "Disconnect")
            //    {
            //        //int tab = Views.ParametarsWindow.ParametersWindowTabSelected;
            //        //RefreshManger.GroupToExecute(tab);
            //        //var result = RefreshManger.BuildGroup.
            //        Dictionary<Tuple<int, int>, DataViewModel> TempList = new Dictionary<Tuple<int, int>, DataViewModel>();
            //        bool Flag = false, Selected = false;
            //        string KeyStr = "";
            //        this.IsSelected = true;
            //        this.EnableTextBox = false;
            //        this.ReadOnly = true;
            //        _commandvalueTemp = this.CommandValue;
            //        bool selectExist = false;
            //        //RefreshManger.BuildGroup.Where(x => x.Key.Where(x => x.).ToList().ForEach(b => ((DataViewModel)sub_list).Background2 = new SolidColorBrush(Colors.Red));

            //        foreach(var group in RefreshManger.BuildGroup)
            //        {
            //            foreach(var sub_list in group.Value)
            //            {
            //                if(this.CommandName != ((DataViewModel)sub_list).CommandName)
            //                {
            //                    if(((DataViewModel)sub_list).IsSelected)
            //                    {
            //                        selectExist = true;
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //        if(!selectExist)
            //        {
            //            this.Background = this.Background2 = new SolidColorBrush(Colors.Red);
            //            this.ReadOnly = false;
            //            this.EnableTextBox = true;


            //            foreach(var group in RefreshManger.BuildGroup)
            //            {
            //                TempList = new Dictionary<Tuple<int, int>, DataViewModel>();
            //                foreach(var sub_list in group.Value)
            //                {
            //                    if(this.CommandName == ((DataViewModel)sub_list).CommandName)
            //                    {
            //                        Selected = true;
            //                        Flag = true;
            //                    }
            //                    else
            //                    {
            //                        Selected = false;
            //                    }
            //                    var data = new DataViewModel
            //                    {
            //                        CommandName = ((DataViewModel)sub_list).CommandName,
            //                        CommandId = ((DataViewModel)sub_list).CommandId,
            //                        CommandSubId = ((DataViewModel)sub_list).CommandSubId,
            //                        CommandValue = ((DataViewModel)sub_list).CommandValue,
            //                        IsFloat = ((DataViewModel)sub_list).IsFloat,
            //                        IsSelected = Selected,
            //                    };
            //                    try
            //                    {
            //                        TempList.Add(new Tuple<int, int>(Int32.Parse(((DataViewModel)sub_list).CommandId), Int32.Parse(((DataViewModel)sub_list).CommandSubId)), data);
            //                    }
            //                    catch(Exception e)
            //                    {

            //                    }

            //                }
            //                if(Flag)
            //                {
            //                    KeyStr = group.Key;
            //                    Flag = false;
            //                    break;
            //                }
            //            }

            //            RefreshManger.BuildGroup.Remove(KeyStr);
            //            RefreshManger.BuildGroup.Add(KeyStr, new ObservableCollection<object>());
            //            foreach(var sub_list in TempList.Values)
            //            {
            //                var data = new DataViewModel
            //                {
            //                    CommandName = ((DataViewModel)sub_list).CommandName,
            //                    CommandId = ((DataViewModel)sub_list).CommandId,
            //                    CommandSubId = ((DataViewModel)sub_list).CommandSubId,
            //                    CommandValue = ((DataViewModel)sub_list).CommandValue,
            //                    IsFloat = ((DataViewModel)sub_list).IsFloat,
            //                    IsSelected = ((DataViewModel)sub_list).IsSelected,
            //                };
            //                RefreshManger.BuildGroup[KeyStr].Add(data);
            //            }
            //        }
            //    }
            //}
            #endregion Not_Enabled
        }

        private void MouseLeaveCommandFunc()
        {
            RefreshManger.DataPressed = false;
            //if(Commands.GetInstance.DataViewCommandsList.ContainsKey(new Tuple<int, int>(Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId))))
            //{
            //    Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId))].IsSelected = false;
            //    Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId))].Background2 = new SolidColorBrush(Colors.White);
            //}
            foreach(var list in Commands.GetInstance.DataViewCommandsList)
            {
                try
                {
                    Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(list.Value.CommandId), Convert.ToInt16(list.Value.CommandSubId))].IsSelected = false;
                    Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(list.Value.CommandId), Convert.ToInt16(list.Value.CommandSubId))].BackgroundStd = new SolidColorBrush(Colors.White);
                    Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(list.Value.CommandId), Convert.ToInt16(list.Value.CommandSubId))].BackgroundSmallFont = new SolidColorBrush(Colors.Gray);
                }
                catch(Exception)
                {

                }
            }
            //SuperButton.Models.DriverBlock.RefreshManger.GetInstance._oneSelected = false;
            //Dictionary<Tuple<int, int>, DataViewModel> TempList = new Dictionary<Tuple<int, int>, DataViewModel>();
            //bool Flag = false, Selected = false;
            //string KeyStr = "";
            //this.IsSelected = false;
            //if(_escPressed)
            //    this.CommandValue = _commandvalueTemp;
            //this.Background = new SolidColorBrush(Colors.Gray);
            //this.Background2 = new SolidColorBrush(Colors.White);


            //foreach(var group in RefreshManger.BuildGroup)
            //{
            //    TempList = new Dictionary<Tuple<int, int>, DataViewModel>();
            //    foreach(var sub_list in group.Value)
            //    {
            //        if(this.CommandName == ((DataViewModel)sub_list).CommandName)
            //        {
            //            Selected = false;
            //            Flag = true;
            //        }
            //        else
            //            Selected = false;
            //        var data = new DataViewModel
            //        {
            //            CommandName = ((DataViewModel)sub_list).CommandName,
            //            CommandId = ((DataViewModel)sub_list).CommandId,
            //            CommandSubId = ((DataViewModel)sub_list).CommandSubId,
            //            CommandValue = ((DataViewModel)sub_list).CommandValue,
            //            IsFloat = ((DataViewModel)sub_list).IsFloat,
            //            IsSelected = Selected,

            //        };
            //        try
            //        {
            //            TempList.Add(new Tuple<int, int>(Int32.Parse(((DataViewModel)sub_list).CommandId), Int32.Parse(((DataViewModel)sub_list).CommandSubId)), data);
            //        }
            //        catch(Exception e)
            //        {

            //        }

            //    }
            //    if(Flag)
            //    {
            //        KeyStr = group.Key;
            //        Flag = false;
            //        break;
            //    }
            //}

            //RefreshManger.BuildGroup.Remove(KeyStr);
            //RefreshManger.BuildGroup.Add(KeyStr, new ObservableCollection<object>());
            //foreach(var sub_list in TempList.Values)
            //{
            //    var data = new DataViewModel
            //    {
            //        CommandName = ((DataViewModel)sub_list).CommandName,
            //        CommandId = ((DataViewModel)sub_list).CommandId,
            //        CommandSubId = ((DataViewModel)sub_list).CommandSubId,
            //        CommandValue = ((DataViewModel)sub_list).CommandValue,
            //        IsFloat = ((DataViewModel)sub_list).IsFloat,
            //        IsSelected = ((DataViewModel)sub_list).IsSelected,
            //    };
            //    RefreshManger.BuildGroup[KeyStr].Add(data);
            //}
        }
        public SolidColorBrush BackgroundSmallFont
        {
            get
            {
                if(!IsSelected)
                    return new SolidColorBrush(Colors.Gray);
                else
                    return new SolidColorBrush(Colors.Red);
            }
            set
            {
                if(IsSelected)
                    _baseModel.Background = new SolidColorBrush(Colors.Red);
                else
                    _baseModel.Background = new SolidColorBrush(Colors.Gray);

                OnPropertyChanged("BackgroundSmallFont");
            }

        }
        public SolidColorBrush BackgroundStd
        {
            get
            {
                if(!IsSelected)
                    return new SolidColorBrush(Colors.White);
                else
                    return new SolidColorBrush(Colors.Red);
            }
            set
            {
                if(IsSelected)
                    _baseModel.Background = new SolidColorBrush(Colors.Red);
                else
                    _baseModel.Background = new SolidColorBrush(Colors.White);

                OnPropertyChanged("BackgroundStd");
            }

        }

        public bool ReadOnly
        {
            get { if(!IsSelected) return true; else return false; }
            set { if(!IsSelected) _baseModel.ReadOnly = true; else _baseModel.ReadOnly = false; OnPropertyChanged("ReadOnly"); }
        }
        public bool EnableTextBox { get { return true; } set { if(IsSelected) _baseModel.EnableTextBox = true; else _baseModel.EnableTextBox = false; OnPropertyChanged("EnableTextBox"); } }

        public object ColorConvert(object value,
                     Type targetType,
                     object parameter,
                     System.Globalization.CultureInfo culture)
        {
            var temp = (int)value;

            if(temp < 100 && temp > 50)
                return new SolidColorBrush(
                    (Color)ColorConverter
                        .ConvertFromString("#FFFF23D3"));

            if(temp > 200)
                return new SolidColorBrush(
                    (Color)ColorConverter
                        .ConvertFromString("#FFDA2367"));

            return new SolidColorBrush(Colors.Green);
        }

    }
}