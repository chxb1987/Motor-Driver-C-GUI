using System;
using System.Windows.Input;
using SuperButton.Models;
using SuperButton.Models.DriverBlock;
using MathNet.Numerics;

namespace SuperButton.ViewModels
{
    public class DataViewModel : ViewModelBase
    {


        private readonly BaseModel _baseModel = new BaseModel();
        ICommand _mouseLeftClickCommand;
        public event System.Windows.Input.MouseButtonEventHandler MouseLeftButtonDown;

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

        public bool IsSelected { get { return _baseModel.IsSelected; } set { _baseModel.IsSelected = value; } }
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
       
            var tmp = new PacketFields
            {
                Data2Send = CommandValue ,
                ID = Convert.ToInt16(CommandId),
                SubID = Convert.ToInt16(CommandSubId),
                IsSet = true,
                IsFloat = IsFloat,
            };
            Rs232Interface.GetInstance.SendToParser(tmp);
        }

        public ICommand MouseLeftClickCommand
        {
            get
            {
                return _mouseLeftClickCommand ?? (_mouseLeftClickCommand = new RelayCommand(MouseLeftClickFunc));
            }
        }

        private void MouseLeftClickFunc()
        {
            IsSelected = true;
        }
    }
}
