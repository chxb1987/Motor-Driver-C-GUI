using Abt.Controls.SciChart;
using SuperButton.Models.DriverBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperButton.ViewModels
{
    internal class CalibrationViewModel : ViewModelBase
    {
        #region FIELDS
        private static readonly object Synlock = new object();
        private static CalibrationViewModel _instance;

        private string _offsetCalValue;
        private string _PICurrentCalVal;
        private string _PISpeedCalVal;
        private string _HallCalVal;
        private string _Encoder1CalVal;
        private string _PIPosCalVal;

        #endregion FIELDS


        public ActionCommand OffsetCal { get { return new ActionCommand(OffsetCalCmd); } }
        public ActionCommand PiCurrentCal { get { return new ActionCommand(PiCurrentCalCmd); } }
        public ActionCommand PiSpeedCal { get { return new ActionCommand(PiSpeedCalCmd); } }
        public ActionCommand HallMapCal { get { return new ActionCommand(HallMapCalCmd); } }
        public ActionCommand EncoderCal { get { return new ActionCommand(EncoderCalCmd); } }
        public ActionCommand P2PositionCal { get { return new ActionCommand(P2PositionCalCmd); } }
        public static CalibrationViewModel GetInstance
        {
            get
            {
                lock (Synlock)
                {
                    if (_instance != null) return _instance;
                    _instance = new CalibrationViewModel();
                    return _instance;
                }
            }
        }

        private CalibrationViewModel()
        {

        }
        private void OffsetCalCmd()
        {

            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = 1,
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(1),
                IsSet = true,
                IsFloat = false
            }
        );

        }
        private void PiCurrentCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "1",
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(2),
                IsSet = true,
                IsFloat = false
            }
        );
        }
        private void PiSpeedCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "1",
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(5),
                IsSet = true,
                IsFloat = false
            }
        );

        }
        private void HallMapCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "1",
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(3),
                IsSet = true,
                IsFloat = false
            }
        );
        }
        private void EncoderCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "1",
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(4),
                IsSet = true,
                IsFloat = false
            }
        );

        }
        private void P2PositionCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                //Data2Send = "1",
                //ID = Convert.ToInt16(6),
                //SubID = Convert.ToInt16(6),
                //IsSet = true,
                //IsFloat = false
                Data2Send = 7,
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(7),
                IsSet = true,
                IsFloat = false
            }
        );
        }

        public string offsetCalVal
        {
            get { return _offsetCalValue; }
            set { _offsetCalValue = "Result: " + value; OnPropertyChanged("offsetCalVal"); }
        }
        public string PICurrentCalVal
        {
            get { return _PICurrentCalVal; }
            set { _PICurrentCalVal = "Result: " + value; OnPropertyChanged("PICurrentCalVal"); }
        }
        public string PISpeedCalVal
        {
            get { return _PISpeedCalVal; }
            set { _PISpeedCalVal = "Result: " + value; OnPropertyChanged("PISpeedCalVal"); }
        }
        public string HallCalVal
        {
            get { return _HallCalVal; }
            set { _HallCalVal = "Result: " + value; OnPropertyChanged("HallCalVal"); }
        }
        public string Encoder1CalVal
        {
            get { return _Encoder1CalVal; }
            set { _Encoder1CalVal = "Result: " + value; OnPropertyChanged("Encoder1CalVal"); }
        }
        public string PIPosCalVal
        {
            get { return _PIPosCalVal; }
            set { _PIPosCalVal = "Result: " + value; OnPropertyChanged("PIPosCalVal"); }
        }
    }
}






