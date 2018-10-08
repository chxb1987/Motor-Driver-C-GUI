using Abt.Controls.SciChart;
using SuperButton.Models.DriverBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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

        private string _PIPosCalContent = "Run";

        private SolidColorBrush _PIPosCalContentCol = new SolidColorBrush(Colors.White);

        private bool _PiPositionCalCheck = false;

        public static bool CalibrationProcessing = false;

        #endregion FIELDS


        public ActionCommand OffsetCal { get { return new ActionCommand(OffsetCalCmd); } }
        public ActionCommand PiCurrentCal { get { return new ActionCommand(PiCurrentCalCmd); } }
        public ActionCommand PiSpeedCal { get { return new ActionCommand(PiSpeedCalCmd); } }
        public ActionCommand HallMapCal { get { return new ActionCommand(HallMapCalCmd); } }
        public ActionCommand EncoderCal { get { return new ActionCommand(EncoderCalCmd); } }
        public ActionCommand PiPositionCal { get { return new ActionCommand(PiPositionCalCmd); } }
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
            });
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(2),
                IsSet = false,
                IsFloat = false
            });
        }
        private void PiCurrentCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = 1,
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(3),
                IsSet = true,
                IsFloat = false
            });
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(4),
                IsSet = false,
                IsFloat = false
            });
        }
        private void HallMapCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = 1,
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(5),
                IsSet = true,
                IsFloat = false
            });
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(6),
                IsSet = false,
                IsFloat = false
            });
        }
        private void EncoderCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = 1,
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(7),
                IsSet = true,
                IsFloat = false
            });
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(8),
                IsSet = false,
                IsFloat = false
            });
        }
        private void PiSpeedCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = 1,
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(9),
                IsSet = true,
                IsFloat = false
            });
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                ID = Convert.ToInt16(6),
                SubID = Convert.ToInt16(10),
                IsSet = false,
                IsFloat = false
            });
        }
        private void PiPositionCalCmd()
        {
            if (PiPositionCalCheck && !CalibrationProcessing)
            {
                CalibrationProcessing = true;
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = 1,
                    ID = Convert.ToInt16(6),
                    SubID = Convert.ToInt16(11),
                    IsSet = true,
                    IsFloat = false
                });
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    ID = Convert.ToInt16(6),
                    SubID = Convert.ToInt16(12),
                    IsSet = false,
                    IsFloat = false
                });
            }
        }
        public string PIPosCalContent
        {
            get { return _PIPosCalContent; }
            set
            {
                _PIPosCalContent = PIPosCalContent;
                OnPropertyChanged("PIPosCalContent");
            }
        }
        public SolidColorBrush PIPosCalContentCol
        {
            get { return _PIPosCalContentCol; }
            set
            {
                _PIPosCalContentCol = PIPosCalContentCol;
                OnPropertyChanged("PIPosCalContentCol");
            }
        }

        public bool PiPositionCalCheck
        {
            get { return _PiPositionCalCheck; }
            set
            {
                if (!CalibrationProcessing)
                {
                    if (value)
                    {
                        PIPosCalContent = _PIPosCalContent = "Running";
                        PIPosCalContentCol = _PIPosCalContentCol = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        PIPosCalContent = _PIPosCalContent = "Run";
                        PIPosCalContentCol = _PIPosCalContentCol = new SolidColorBrush(Colors.White);
                    }
                    _PiPositionCalCheck = value;
                    //_PiPositionCalCheck = !_PiPositionCalCheck;
                    OnPropertyChanged("PiPositionCalCheck");
                }
            }
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






