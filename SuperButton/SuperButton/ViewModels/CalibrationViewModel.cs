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
        private string _PiCurrentCalVal;
        private string _PiSpeedCalVal;
        private string _HallMapCalVal;
        private string _Encoder1CalVal;
        private string _PiPosCalVal;

        private string _PiPosCalContent = "Run";
        private bool _PiPositionCalCheck = false;
        private bool _PiPositionCalEn = true;

        private string _OffsetCalContent = "Run";
        private bool _OffsetCalCheck = false;
        private bool _OffsetCalEn = true;

        private string _PiCurrentCalContent = "Run";
        private bool _PiCurrentCalCheck = false;
        private bool _PiCurrentCalEn = true;

        private string _HallMapCalContent = "Run";
        private bool _HallMapCalCheck = false;
        private bool _HallMapCalEn = true;
        
        private string _Encoder1CalContent = "Run";
        private bool _Encoder1CalCheck = false;
        private bool _Encoder1CalEn = true;

        
        private string _PiSpeedCalContent = "Run";
        private bool _PiSpeedCalCheck = false;
        private bool _PiSpeedCalEn = true;

        public static bool CalibrationProcessing = false;

        #endregion FIELDS


        public ActionCommand OffsetCal { get { return new ActionCommand(OffsetCalCmd); } }
        public ActionCommand PiCurrentCal { get { return new ActionCommand(PiCurrentCalCmd); } }
        public ActionCommand PiSpeedCal { get { return new ActionCommand(PiSpeedCalCmd); } }
        public ActionCommand HallMapCal { get { return new ActionCommand(HallMapCalCmd); } }
        public ActionCommand Encoder1Cal { get { return new ActionCommand(Encoder1CalCmd); } }
        public ActionCommand PiPositionCal
        {
            get
            {
                return new ActionCommand(PiPositionCalCmd);
            }
        }
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


        #region Current_Offset
        private void OffsetCalCmd()
        {
            if (OffsetCalContent == "Run")
            {
                CalibrationProcessing = true;
                OffsetCalEn = false;
                OffsetCalVal = "";
                OffsetCalContent = "Running";
                OffsetCalCheck = true;

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
                RefreshManger.CalibrationTimeOut = 20;
            }
        }
        public string OffsetCalContent
        {
            get { return _OffsetCalContent; }
            set
            {
                if (value == "Run")
                {
                    OffsetCalCheck = false;
                }
                if (value != _OffsetCalContent)
                {
                    _OffsetCalContent = value;
                    OnPropertyChanged("OffsetCalContent");
                }
            }
        }
        public bool OffsetCalCheck
        {
            get { return _OffsetCalCheck; }
            set
            {
                if (value != _OffsetCalCheck && !CalibrationProcessing)
                {
                    _OffsetCalCheck = value;
                    OnPropertyChanged("OffsetCalCheck");
                }

            }
        }
        public bool OffsetCalEn
        {
            get { return _OffsetCalEn; }
            set
            {
                if (value != _OffsetCalEn && !CalibrationProcessing)
                {
                    _OffsetCalEn = value;
                    OnPropertyChanged("OffsetCalEn");
                }
            }
        }
        public string OffsetCalVal
        {
            get { return _offsetCalValue; }
            set { _offsetCalValue = "Result: " + value; OnPropertyChanged("OffsetCalVal"); }
        }
        #endregion Current_Offset
        #region Pi_Current_Loop
        private void PiCurrentCalCmd()
        {
            if (PiCurrentCalContent == "Run")
            {
                CalibrationProcessing = true;
                PiCurrentCalEn = false;
                PiCurrentCalVal = "";
                PiCurrentCalContent = "Running";
                PiCurrentCalCheck = true;
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
                RefreshManger.CalibrationTimeOut = 20;
            }
        }
        public string PiCurrentCalContent
        {
            get { return _PiCurrentCalContent; }
            set
            {
                if (value == "Run")
                {
                    PiCurrentCalCheck = false;
                }
                if (value != _PiCurrentCalContent)
                {
                    _PiCurrentCalContent = value;
                    OnPropertyChanged("PiCurrentCalContent");
                }
            }
        }
        public bool PiCurrentCalCheck
        {
            get { return _PiCurrentCalCheck; }
            set
            {
                if (value != _PiCurrentCalCheck && !CalibrationProcessing)
                {
                    _PiCurrentCalCheck = value;
                    OnPropertyChanged("PiCurrentCalCheck");
                }

            }
        }
        public bool PiCurrentCalEn
        {
            get { return _PiCurrentCalEn; }
            set
            {
                if (value != _PiCurrentCalEn && !CalibrationProcessing)
                {
                    _PiCurrentCalEn = value;
                    OnPropertyChanged("PiCurrentCalEn");
                }
            }
        }
        public string PiCurrentCalVal
        {
            get { return _PiCurrentCalVal; }
            set {
                
                {
                    _PiCurrentCalVal = "Result: " + value;
                    OnPropertyChanged("PiCurrentCalVal");
                }
            }
        }
        #endregion Pi_Current_Loop
        #region Hall_Map
        private void HallMapCalCmd()
        {
            if (HallMapCalContent == "Run")
            {
                CalibrationProcessing = true;
                HallMapCalEn = false;
                HallMapCalVal = "";
                HallMapCalContent = "Running";
                HallMapCalCheck = true;

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
                RefreshManger.CalibrationTimeOut = 20;
            }
        }
        public string HallMapCalContent
        {
            get { return _HallMapCalContent; }
            set
            {
                if (value == "Run")
                {
                    HallMapCalCheck = false;
                }
                if (value != _HallMapCalContent)
                {
                    _HallMapCalContent = value;
                    OnPropertyChanged("HallMapCalContent");
                }
            }
        }
        public bool HallMapCalCheck
        {
            get { return _HallMapCalCheck; }
            set
            {
                if (value != _HallMapCalCheck && !CalibrationProcessing)
                {
                    _HallMapCalCheck = value;
                    OnPropertyChanged("HallMapCalCheck");
                }

            }
        }
        public bool HallMapCalEn
        {
            get { return _HallMapCalEn; }
            set
            {
                if (value != _HallMapCalEn && !CalibrationProcessing)
                {
                    _HallMapCalEn = value;
                    OnPropertyChanged("HallMapCalEn");
                }
            }
        }
        public string HallMapCalVal
        {
            get { return _HallMapCalVal; }
            set { _HallMapCalVal = "Result: " + value; OnPropertyChanged("HallMapCalVal"); }
        }
        #endregion Hall_Map
        #region Encoder1
        private void Encoder1CalCmd()
        {
            if (Encoder1CalContent == "Run")
            {
                CalibrationProcessing = true;
                Encoder1CalEn = false;
                Encoder1CalVal = "";
                Encoder1CalContent = "Running";
                Encoder1CalCheck = true;

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
                RefreshManger.CalibrationTimeOut = 20;
            }
        }
        public string Encoder1CalContent
        {
            get { return _Encoder1CalContent; }
            set
            {
                if (value == "Run")
                {
                    Encoder1CalCheck = false;
                }
                if (value != _Encoder1CalContent)
                {
                    _Encoder1CalContent = value;
                    OnPropertyChanged("Encoder1CalContent");
                }
            }
        }
        public bool Encoder1CalCheck
        {
            get { return _Encoder1CalCheck; }
            set
            {
                if (value != _Encoder1CalCheck && !CalibrationProcessing)
                {
                    _Encoder1CalCheck = value;
                    OnPropertyChanged("Encoder1CalCheck");
                }

            }
        }
        public bool Encoder1CalEn
        {
            get { return _Encoder1CalEn; }
            set
            {
                if (value != _Encoder1CalEn && !CalibrationProcessing)
                {
                    _Encoder1CalEn = value;
                    OnPropertyChanged("Encoder1CalEn");
                }
            }
        }
        public string Encoder1CalVal
        {
            get { return _Encoder1CalVal; }
            set { _Encoder1CalVal = "Result: " + value; OnPropertyChanged("Encoder1CalVal"); }
        }
        #endregion Encoder1
        #region Pi_Speed_Loop
        private void PiSpeedCalCmd()
        {
            if (PiSpeedCalContent == "Run")
            {
                CalibrationProcessing = true;
                PiSpeedCalEn = false;
                PiSpeedCalVal = "";
                PiSpeedCalContent = "Running";
                PiSpeedCalCheck = true;

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
                RefreshManger.CalibrationTimeOut = 20;
            }
        }
        public string PiSpeedCalContent
        {
            get { return _PiSpeedCalContent; }
            set
            {
                if (value == "Run")
                {
                    PiSpeedCalCheck = false;
                }
                if (value != _PiSpeedCalContent)
                {
                    _PiSpeedCalContent = value;
                    OnPropertyChanged("PiSpeedCalContent");
                }
            }
        }
        public bool PiSpeedCalCheck
        {
            get { return _PiSpeedCalCheck; }
            set
            {
                if (value != _PiSpeedCalCheck && !CalibrationProcessing)
                {
                    _PiSpeedCalCheck = value;
                    OnPropertyChanged("PiSpeedCalCheck");
                }

            }
        }
        public bool PiSpeedCalEn
        {
            get { return _PiSpeedCalEn; }
            set
            {
                if (value != _PiSpeedCalEn && !CalibrationProcessing)
                {
                    _PiSpeedCalEn = value;
                    OnPropertyChanged("PiSpeedCalEn");
                }
            }
        }
        public string PiSpeedCalVal
        {
            get { return _PiSpeedCalVal; }
            set { _PiSpeedCalVal = "Result: " + value; OnPropertyChanged("PiSpeedCalVal"); }
        }
        #endregion Pi_Speed_Loop
        #region Pi_Position_Loop
        private void PiPositionCalCmd()
        {
            if (PiPosCalContent == "Run")
            {
                CalibrationProcessing = true;
                PiPositionCalEn = false;
                PiPosCalVal = "";
                PiPosCalContent = "Running";
                PiPositionCalCheck = true;

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
                RefreshManger.CalibrationTimeOut = 20;
            }
        }
        public string PiPosCalContent
        {
            get { return _PiPosCalContent; }
            set
            {
                if (value == "Run")
                {
                    PiPositionCalCheck = false;
                }
                if (value != _PiPosCalContent)
                {
                    _PiPosCalContent = value;
                    OnPropertyChanged("PiPosCalContent");
                }
            }
        }
        public bool PiPositionCalCheck
        {
            get { return _PiPositionCalCheck; }
            set
            {
                if (value != _PiPositionCalCheck && !CalibrationProcessing)
                {
                    _PiPositionCalCheck = value;
                    OnPropertyChanged("PiPositionCalCheck");
                }

            }
        }
        public bool PiPositionCalEn
        {
            get { return _PiPositionCalEn; }
            set
            {
                if (value != _PiPositionCalEn && !CalibrationProcessing)
                {
                    _PiPositionCalEn = value;
                    OnPropertyChanged("PiPositionCalEn");
                }
            }
        }
        public string PiPosCalVal
        {
            get { return _PiPosCalVal; }
            set
            {
                _PiPosCalVal = "Result: " + value;
                OnPropertyChanged("PiPosCalVal");
                PiPositionCalEn = true;
            }
        }
        #endregion Pi_Position_Loop
        
    }
}






