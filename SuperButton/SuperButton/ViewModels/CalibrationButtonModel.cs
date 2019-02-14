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
using Abt.Controls.SciChart;
using SuperButton.Views;

namespace SuperButton.ViewModels
{
    public class CalibrationButtonModel : ViewModelBase
    {
        public static bool CalibrationProcessing = false;
        private readonly BaseModel _baseModel = new BaseModel();
        private bool EnRefChg = false;
        private string _buttonContent = "Run";
        private bool _buttonCheck = false;
        private bool _buttonEn = true;

        public string ButtonContent
        {
            get { return _buttonContent; }
            set
            {
                if(value == "Run")
                {
                    ButtonCheck = false;
                    CalibrationProcessing = false;
                    if(EnRefChg)
                    {
                        LeftPanelViewModel.GetInstance.EnRefresh = true;
                        EnRefChg = false;
                    }
                    if(OscilloscopeParameters.ChanTotalCounter > 0)
                    {
                        OscilloscopeViewModel.GetInstance.IsFreeze = false;
                    }
                }
                if(value != _buttonContent)
                { _buttonContent = value; OnPropertyChanged("ButtonContent"); }
            }
        }
        public bool ButtonCheck { get { return _buttonCheck; } set { if(value != _buttonCheck && !CalibrationProcessing) { _buttonCheck = value; OnPropertyChanged("ButtonCheck"); } } }
        public bool ButtonEn { get { return _buttonEn; } set { if(value != _buttonEn && !CalibrationProcessing) { _buttonEn = value; OnPropertyChanged("ButtonEn"); } } }
        public string CommandName { get { return _baseModel.CommandName; } set { _baseModel.CommandName = value; } }
        public string CommandValue { get { return _baseModel.CommandValue; } set { _baseModel.CommandValue = "Result: " + value; OnPropertyChanged(); } }
        public string CommandId { get { return _baseModel.CommandID; } set { _baseModel.CommandID = value; } }
        public string CommandSubId { get { return _baseModel.CommandSubID; } set { _baseModel.CommandSubID = value; } }
        public bool IsFloat { get { return _baseModel.IsFloat; } set { _baseModel.IsFloat = value; } }
        public bool IsSelected { get { return _baseModel.IsSelected; } set { _baseModel.IsSelected = value; OnPropertyChanged(); } }

        public ActionCommand ButtonCal { get { return new ActionCommand(ButtonCmd); } }
        private void ButtonCmd()
        {
            if(ButtonContent == "Run" && CalibrationProcessing == false)
            {
                if(LeftPanelViewModel.GetInstance.EnRefresh)
                {
                    LeftPanelViewModel.GetInstance.EnRefresh = false;
                    EnRefChg = true;
                }
                if(OscilloscopeParameters.ChanTotalCounter > 0)
                {
                    OscilloscopeViewModel.GetInstance.IsFreeze = true;
                }
                ButtonCheck = true;
                CalibrationProcessing = true;
                ButtonEn = false;
                CommandValue = "";
                ButtonContent = "Running";

                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = 1,
                    ID = Convert.ToInt16(CommandId),
                    SubID = Convert.ToInt16(CommandSubId),
                    IsSet = true,
                    IsFloat = false
                });
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    ID = Convert.ToInt16(CommandId),
                    SubID = Convert.ToInt16(Convert.ToInt16(CommandSubId) + 1),
                    IsSet = false,
                    IsFloat = false
                });
                RefreshManger.CalibrationTimeOut = 20;
            }
        }
    }
}
