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
    public class NumericTextboxModel : ViewModelBase
    {


        private readonly BaseModel _baseModel = new BaseModel();

        public string Name { get { return _baseModel.CommandName; } set { _baseModel.CommandName = value; } }

        private string _commandValue = "1";
        public string CommandValue
        {
            get
            {
                return _commandValue;
            }
            set
            {
                _commandValue = value;
                OnPropertyChanged();
            }
        }

        public virtual ICommand SendData
        {
            get
            {
                return new RelayCommand(ParseValue);
            }
        }
        public virtual ICommand ResetValue
        {
            get
            {
                return new RelayCommand(Esc);
            }
        }
        private void ParseValue()
        {
            Commands.GetInstance.Gain[Name].CommandValue = _commandValue;
        }
        private void Esc()
        {
        }
    }
}