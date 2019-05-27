using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Example.Common;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Numerics;
using SuperButton.Common;
using SuperButton.Models.DriverBlock;
using SuperButton.ViewModels;
using Timer = System.Timers.Timer;
using SuperButton.Models.ParserBlock;
using System.Diagnostics;
using SuperButton.Helpers;
using System.Collections.ObjectModel;
using SuperButton.CommandsDB;

//Cntl+M and Control+O for close regions
namespace SuperButton.Views
{

    public class OscilloscopeViewModel : BaseViewModel
    {
        #region members
        //    private const float _singleChanelFreq = 6600;
        //Y axle title
        //   private float step = (float) 0.151515;

        // private float delta = (float) (1.0/_singleChanelFreq);


        private string _yaxeTitle;
        readonly Dictionary<string, string> ChannelYtitles = new Dictionary<string, string>();

        //CH1 ComboBox
        int ch1;
        private string _ch1Title;
        public List<string> _channel1SourceItems = new List<string>();
        public List<string> Channel1SourceItems
        {
            get { return _channel1SourceItems; }
            set
            {
                _channel1SourceItems = value;
                OnPropertyChanged("Channel1SourceItems");
            }
        }
        private string _selectedCh1DataSource;

        //CH2 ComboBox
        int ch2;
        private string _ch2Title;
        public List<string> Channel2SourceItems
        {
            get { return _channel1SourceItems; }
            set
            {
                _channel1SourceItems = value;
                OnPropertyChanged("Channel2SourceItems");
            }
        }
        private string _selectedCh2DataSource;

        private UInt16 plotActivationstate;


        // private float dtx;

        // private int count;
        // private int countlimit;


        //13.01
        private bool _isFull = false;
        private float[] xData;


        // private int countert = 0;
        // private int Aountert = 0;

        // private static readonly object PointstoplotLOCK = new object();   //Semapophor;
        // private int PlotFifoLenth = 0;
        private int pivot = 0;

        readonly List<float> AllYData = new List<float>(500000);//500000
        readonly List<float> AllYData2 = new List<float>(500000);

        //Debug Vars
        //private float Averadge = 0;

        private DoubleRange _xVisibleRange;
        private DoubleRange _yVisibleRange;

        private ModifierType _chartModifier;
        private bool _isDigitalLine = true;

        private Timer _timer;

        private ResamplingMode _resamplingMode;
        private bool _canExecuteRollover;

        //private Thread SciThread;
        //  private int FlowControl = 500;

        //  private int MinimumChank = 3300/10 ;
        //  private int MinimumFillChank = 3300 / 5;
        //  private double _duration = 500;
        //  private int POintstoPlot = 3300; //0.5 sec - 1 channel

        //private int MinimumChank = 33000;
        //private int MinimumFillChank = 3300 / 5;

        private int POintstoPlot = 33000; //5 sec min


        public List<float> RecList = new List<float>();
        public List<float> RecList2 = new List<float>();


        private int _undesample = 1;
        private uint _undesampleCounter = 0;

        private const double TimerIntervalMs = 1;

        private int ucarry;
        //private int ustate = 0;


        //Umprove
        private int State = 0;

        List<float> utemp3L = new List<float>();
        float[] utemp3;


        #endregion
        ~OscilloscopeViewModel()
        {

        }
        #region Yzoom
        private double _yzoom = 0;
        public ActionCommand YPlus
        {
            get { return new ActionCommand(YDirPlus); }
        }
        public ActionCommand YMinus
        {
            get { return new ActionCommand(YDirMinus); }
        }
        public void YDirMinus()
        {
            if(_yzoom > 0.0002)
            {
                _yzoom = _yzoom / 2;
                YLimit = new DoubleRange(-_yzoom, _yzoom); //ubdate visible limits        
                YVisibleRange = YLimit;
            }
        }
        public void YDirPlus()
        {
            if(_yzoom > 0 &&_yzoom < 1000)
            {
                _yzoom = _yzoom * 2;
                YLimit = new DoubleRange(-_yzoom, _yzoom); //ubdate visible limits        
                YVisibleRange = YLimit;
            }
        }
        #endregion
        #region Duration
        private float _duration = 5000;
        public ActionCommand DirectionPlus
        {
            get { return new ActionCommand(DirPlus); }
        }
        public ActionCommand DirectionMinus
        {
            get { return new ActionCommand(DirMinus); }
        }
        public void DirPlus()
        {
            switch((int)_duration)
            {
                case (10)://Initial duration is 1 sec [1000ms]
                    {
                        if(_timer != null)
                        {
                            lock(_timer)
                            {
                                //  MinimumChank = 33000;
                                _duration = 100; //update x axe
                                POintstoPlot = (int)(OscilloscopeParameters.ChanelFreq * 0.1);  //update resolution
                            }

                        }
                        else
                        {
                            _duration = 100; //update x axe
                            POintstoPlot = (int)(OscilloscopeParameters.ChanelFreq * 0.1);  //update resolution
                            _yFloats = new float[0];
                            _yFloats2 = new float[0];
                        }

                        break;
                    }

                case (100)://Initial duration is 1 sec [1000ms]
                    {
                        if(_timer != null)
                        {
                            lock(_timer)
                            {
                                //  MinimumChank = 33000;
                                _duration = 1000; //update x axe
                                POintstoPlot = (int)(OscilloscopeParameters.ChanelFreq);  //update resolution
                            }

                        }
                        else
                        {
                            _duration = 1000; //update x axe
                            POintstoPlot = (int)(OscilloscopeParameters.ChanelFreq);  //update resolution
                            _yFloats = new float[0];
                            _yFloats2 = new float[0];
                        }

                        break;
                    }
                case (1000)://Initial duration is 1 sec [1000ms]
                    {
                        if(_timer != null)
                        {
                            lock(_timer)
                            {
                                //  MinimumChank = 33000;
                                _duration = 5000; //update x axe
                                POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 5;  //update resolution

                                //XLimit = new DoubleRange(0, _duration); //ubdate visible limits        
                                //XVisibleRange = XLimit;
                                //_isFull = false;
                            }

                        }
                        else
                        {
                            _duration = 5000; //update x axe
                            POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 5;  //update resolution
                            _yFloats = new float[0];
                            _yFloats2 = new float[0];

                            //XLimit = new DoubleRange(0, _duration); //ubdate visible limits        
                            //XVisibleRange = XLimit;
                            //// pivot = (int) (_singleChanelFreq*5); //update pivote and move to initial state
                            //_isFull = false;
                        }

                        break;
                    }

                case (5000)://Initial duration is 5 sec [5000ms]
                    {
                        if(_timer != null)
                        {
                            lock(_timer)
                            {
                                _duration = _duration * 2; //update x axe           
                                POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 10;  //update resolution

                                //XLimit = new DoubleRange(0, _duration); //ubdate visible limits        
                                //XVisibleRange = XLimit;
                                //// pivot = (int) (_singleChanelFreq*5); //update pivote and move to initial state
                                //_isFull = false;
                            }

                        }
                        else
                        {
                            _duration = _duration * 2; //update x axe           
                            POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 10;  //update resolution
                            _yFloats = new float[0];
                            _yFloats2 = new float[0];
                            //XLimit = new DoubleRange(0, _duration); //ubdate visible limits        
                            //XVisibleRange = XLimit;
                            //// pivot = (int) (_singleChanelFreq*5); //update pivote and move to initial state
                            //_isFull = false;
                        }

                        break;
                    }
                case (10000)://is 10 seconds, goes to be 30 seconds
                    {

                        if(_timer != null)
                        {
                            lock(_timer)
                            {
                                POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 30;  //update resolution                           
                                _duration = _duration * 3; //update x axe

                                //XLimit = new DoubleRange(0, _duration); //ubdate visible limits        
                                //XVisibleRange = XLimit;                    
                                //_isFull = false;

                            }
                        }
                        else
                        {
                            {
                                POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 30;  //update resolution   
                                _duration = _duration * 3; //update x axe
                                _yFloats = new float[0];
                                _yFloats2 = new float[0];
                                //XLimit = new DoubleRange(0, _duration); //ubdate visible limits        
                                //XVisibleRange = XLimit;
                                //_undesampleCounter = 0;
                                //_isFull = false;

                            }
                        }


                        break;
                    }
                case (30000):///is 30 seconds, goes to be 90 seconds
                    {
                        if(_timer != null)
                        {
                            lock(_timer)
                            {
                                POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 90;  //update resolution   
                                _duration = _duration * 3; //update x axe

                                //XLimit = new DoubleRange(0, _duration); //ubdate visible limits        
                                //XVisibleRange = XLimit;
                                //_isFull = false;

                            }
                        }
                        else
                        {
                            POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 90;  //update resolution   
                            _duration = _duration * 3; //update x axe
                            _yFloats = new float[0];
                            _yFloats2 = new float[0];

                        }
                        break;

                    }
            }

            XLimit = new DoubleRange(0, _duration); //ubdate visible limits        
            XVisibleRange = XLimit;
            _isFull = false;


        }
        public void DirMinus()
        {
            switch((int)_duration)
            {
                case (100):
                    {
                        if(_timer != null)
                        {
                            lock(_timer)
                            {
                                _duration = 10; //update x axe
                                POintstoPlot = (int)(OscilloscopeParameters.ChanelFreq * 0.01);
                            } //update resolution                   
                        }
                        else
                        {
                            _duration = 10; //update x axe
                            POintstoPlot = (int)(OscilloscopeParameters.ChanelFreq * 0.01);  //update resolution             
                        }
                        break;
                    }

                case (1000):
                    {
                        if(_timer != null)
                        {
                            lock(_timer)
                            {
                                _duration = 100; //update x axe
                                POintstoPlot = (int)(OscilloscopeParameters.ChanelFreq * 0.1);
                            } //update resolution                   
                        }
                        else
                        {
                            _duration = 100; //update x axe
                            POintstoPlot = (int)(OscilloscopeParameters.ChanelFreq * 0.1);  //update resolution             
                        }
                        break;
                    }

                case (5000):
                    {
                        if(_timer != null)
                        {
                            lock(_timer)
                            {
                                _duration = 1000; //update x axe
                                POintstoPlot = (int)OscilloscopeParameters.ChanelFreq;
                            } //update resolution                   
                        }
                        else
                        {
                            _duration = 1000; //update x axe
                            POintstoPlot = (int)OscilloscopeParameters.ChanelFreq;  //update resolution             
                        }
                        break;
                    }

                case (10000):
                    {
                        if(_timer != null)
                        {
                            lock(_timer)
                            {
                                POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 5;  //update resolution    
                                _duration = _duration / 2; //update x axe
                            }
                        }
                        else
                        {
                            POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 5;  //update resolution    
                            _duration = _duration / 2; //update x axe
                        }

                        break;
                    }
                case (30000)://Initial duration is 30 sec [30000ms], will be 10
                    {
                        if(_timer != null)
                        {
                            lock(_timer)
                            {
                                POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 10;  //update resolution  
                                _duration = _duration / 3; //update x axe
                            }
                        }
                        else
                        {
                            {
                                POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 10;  //update resolution  
                                _duration = _duration / 3; //update x axe
                            }
                        }
                        break;
                    }
                case (90000)://Initial will be 30 sec [15000ms]
                    {
                        if(_timer != null)
                        {
                            lock(_timer)
                            {
                                POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 30;  //update resolution  
                                _duration = _duration / 3; //update x axe
                            }
                        }
                        else
                        {
                            POintstoPlot = (int)OscilloscopeParameters.ChanelFreq * 30;  //update resolution  
                            _duration = _duration / 3; //update x axe
                        }
                        break;
                    }
                default:
                    return;

            }
            XLimit = new DoubleRange(0, _duration); //ubdate visible limits        
            XVisibleRange = XLimit;
            _undesample = 1;
            pivot = (int)0; //update pivote and move to initial state
            _isFull = false;
            if(this.ChartData != null)
            {
                using(this.ChartData.SuspendUpdates())
                {
                    _series0.Clear();
                }
                _yFloats = new float[0];
            }

        }

        #endregion

        #region Constractor
        private static OscilloscopeViewModel _instance;
        private static readonly object Synlock = new object(); //Single tone variable
        public static OscilloscopeViewModel GetInstance
        {
            get
            {
                lock(Synlock)
                {
                    if(_instance != null)
                        return _instance;
                    _instance = new OscilloscopeViewModel();
                    return _instance;
                }
            }
        }

        private ObservableCollection<object> _ch1Gain;
        public ObservableCollection<object> Ch1Gain
        {
            get
            {
                return Commands.GetInstance.GainList["Gain1 List"];
            }
            set
            {
                _ch1Gain = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<object> _ch2Gain;
        public ObservableCollection<object> Ch2Gain
        {
            get
            {
                return Commands.GetInstance.GainList["Gain2 List"];
            }
            set
            {
                _ch2Gain = value;
                OnPropertyChanged();
            }
        }

        public OscilloscopeViewModel()
        {
            IsFreeze = RecFlag = false;
            //Initial frame duration is 5 seconds
            POintstoPlot = (int)(OscilloscopeParameters.SingleChanelFreqC * 5);//20Khz/3*5Seconds

            xData = new float[0];
            _yFloats = new float[0];
            _yFloats2 = new float[0];

            FillDictionary();
            Thread.Sleep(100);
            ResetZoom();
        }
        private void FillDictionary()
        {
            _channel1SourceItems.Add("Pause:");
            _channel1SourceItems.Add("IqFeedback");
            _channel1SourceItems.Add("I_PhaseA");
            _channel1SourceItems.Add("I_PhaseB");
            _channel1SourceItems.Add("I_PhaseC");
            _channel1SourceItems.Add("VDC_Motor");
            _channel1SourceItems.Add("BEMF_PhaseA");
            _channel1SourceItems.Add("BEMF_PhaseB");
            _channel1SourceItems.Add("BEMF_PhaseC");
            _channel1SourceItems.Add("HALL_LPF_Speed");
            _channel1SourceItems.Add("HALL_Elect_Angle");
            _channel1SourceItems.Add("QEP1_LPF_Speed");
            _channel1SourceItems.Add("QEP1_Elect_Angle");
            _channel1SourceItems.Add("QEP2_LPF_Speed");
            _channel1SourceItems.Add("QEP2_Elect_Angle");
            _channel1SourceItems.Add("SSI_LPF_Speed");
            _channel1SourceItems.Add("SSI_Elect_Angle");
            _channel1SourceItems.Add("SL_Elect_Angle");
            _channel1SourceItems.Add("IRms");
            _channel1SourceItems.Add("IRms(Filtered)");
            _channel1SourceItems.Add("SL_LPF_Speed");
            _channel1SourceItems.Add("CommutationAngle");
            _channel1SourceItems.Add("SpeedFdbRPM");
            _channel1SourceItems.Add("SpeedRefRPM");
            _channel1SourceItems.Add("Test_Signal");
            _channel1SourceItems.Add("Cla_filt0");
            _channel1SourceItems.Add("Cmd_Ref");
            _channel1SourceItems.Add("Cmd_Ref_filt");
            _channel1SourceItems.Add("SinEnc");
            _channel1SourceItems.Add("CosEnc");
            _channel1SourceItems.Add("InterAngle");
            _channel1SourceItems.Add("SpeedRefPI");
            _channel1SourceItems.Add("SpeedFdb");
            _channel1SourceItems.Add("CurrentRefPI");

            //update Ch1 ComboBox
            //Channel1SourceItems = _channel1SourceItems;
            //update Ch2 ComboBox
            //Channel2SourceItems = Channel1SourceItems;// _channel1SourceItems;


            ChannelYtitles.Add("Pause", "");
            ChannelYtitles.Add("IqFeedback", "Current [A]");
            ChannelYtitles.Add("I_PhaseA", "Current [A]");
            ChannelYtitles.Add("I_PhaseB", "Current [A]");
            ChannelYtitles.Add("I_PhaseC", "Voltage [V]");
            ChannelYtitles.Add("VDC_Motor", "Voltage [V]");
            ChannelYtitles.Add("BEMF_PhaseA", "Voltage [V]");
            ChannelYtitles.Add("BEMF_PhaseB", "Voltage [V]");
            ChannelYtitles.Add("BEMF_PhaseC", "BEMF_PhaseC");
            ChannelYtitles.Add("HALL_LPF_Speed", "Velocity [KRPM]");
            ChannelYtitles.Add("HALL_Elect_Angle", "Angle [Deg]");
            ChannelYtitles.Add("QEP1_LPF_Speed", "Velocity [KRPM]");
            ChannelYtitles.Add("QEP2_Elect_Angle", "Angle [Deg]");
            ChannelYtitles.Add("QEP2_LPF_Speed", "Velocity [KRPM]");
            ChannelYtitles.Add("SSI_LPF_Speed", "Velocity [KRPM]");
            ChannelYtitles.Add("SSI_Elect_Angle", "Angle [Deg]");
            ChannelYtitles.Add("SL_Elect_Angle", "Angle [Deg]");
            ChannelYtitles.Add("IRms", "Current [A]");
            ChannelYtitles.Add("SL_LPF_Speed", "Velocity [KRPM]");
            ChannelYtitles.Add("CommutationAngle", "Angle [Deg]");
            ChannelYtitles.Add("PositionFdb", "Position [Counts]");
            ChannelYtitles.Add("PositionRef", "Position [Counts]");
            ChannelYtitles.Add("Test_Signal", "");
            ChannelYtitles.Add("Cla_filt0", "");
            ChannelYtitles.Add("Cmd_Ref", "");
            ChannelYtitles.Add("Cmd_Ref_filt", "");//28
            ChannelYtitles.Add("SinEnc", ""); //31
            ChannelYtitles.Add("CosEnc", ""); //32
            ChannelYtitles.Add("InterAngle", ""); //33
            ChannelYtitles.Add("SpeedRefPI", ""); //34
            ChannelYtitles.Add("SpeedFdb", ""); //35
            ChannelYtitles.Add("CurrentRefPI", ""); //36
        }
        #endregion
        #region ActionCommnds
        public ActionCommand SetRolloverModifierCommand
        {
            get { return new ActionCommand(() => SetModifier(ModifierType.Rollover)); }
        }
        public ActionCommand SetCursorModifierCommand
        {
            get { return new ActionCommand(() => SetModifier(ModifierType.CrosshairsCursor)); }
        }
        public ActionCommand SetRubberBandZoomModifierCommand
        {
            get { return new ActionCommand(() => SetModifier(ModifierType.RubberBandZoom)); }
        }
        public ActionCommand SetZoomPanModifierCommand
        {
            get { return new ActionCommand(() => SetModifier(ModifierType.ZoomPan)); }
        }
        public ActionCommand SetNullModifierCommand
        {
            get { return new ActionCommand(() => SetModifier(ModifierType.Null)); }
        }
        public ActionCommand SetDigitalLineCommand
        {
            get { return new ActionCommand(() => IsDigitalLine = !IsDigitalLine); }
        }
        public ActionCommand ResetZoomCommand
        {
            get { return new ActionCommand(ResetZoom); }
        }
        public ActionCommand ClearGraphCommand
        {
            get { return new ActionCommand(ClearGraph); }
        }
        public ActionCommand PlotReset
        {
            get { return new ActionCommand(() => isReset = true); }
        }
        #endregion

        public bool IsRolloverSelected
        {
            get { return ChartModifier == ModifierType.Rollover; }
        }
        public bool IsCursorSelected
        {
            get { return ChartModifier == ModifierType.CrosshairsCursor; }
        }
        public bool CanExecuteRollover
        {
            get { return _canExecuteRollover; }
            set
            {
                if(_canExecuteRollover == value)
                    return;
                _canExecuteRollover = value;
                OnPropertyChanged("CanExecuteRollover");
            }
        }

        //public new EventHandler doubleClick;
        private object isReset = false;
        public object IsReset
        {
            get
            {
                // System.Windows.Forms.Button temp = new System.Windows.Forms.Button();
                return isReset;
            }
            set
            {
                isReset = value;
                SetModifier(ModifierType.ZoomPan);
                OnPropertyChanged("IsReset");
            }
        }

        private int ActChenCount = 0;

        #region Channels
        private int _chan1Counter = 0;
        private int _chan2Counter = 0;

        private IXyDataSeries<float, float> _series0;
        private IXyDataSeries<float, float> _series1;

        private int _ch1Index = 0, _ch2Index = 0;
        public int Ch1SelectedIndex
        {
            get { return _ch1Index; }
            set
            {
                if(_ch1Index == value)
                    return;
                _ch1Index = value;
                OnPropertyChanged("Ch1SelectedIndex");
            }
        }
        public int Ch2SelectedIndex
        {
            get { return _ch2Index; }
            set
            {
                if(_ch2Index == value)
                    return;
                _ch2Index = value;
                OnPropertyChanged("Ch2SelectedIndex");
            }
        }
        public string SelectedCh1DataSource
        {
            get { return _selectedCh1DataSource; }
            set
            {
                //if(_selectedCh1DataSource == value)
                //    return;
                //if(!_is_freeze)
                {
                    _selectedCh1DataSource = value;

                    lock(ParserRayonM1.PlotListLock)
                    {
                        ch1 = _channel1SourceItems.FindIndex(x => x.Equals(_selectedCh1DataSource));
                        ChannelsYaxeMerge(ch1, 1);

                        //y axle update
                        if(Rs232Interface.GetInstance.IsSynced)
                        {
                            //Send command to the target 
                            PacketFields RxPacket;
                            RxPacket.ID = 60;
                            RxPacket.IsFloat = false;
                            RxPacket.IsSet = true;
                            RxPacket.SubID = 1;
                            RxPacket.Data2Send = (ch1 >= 28) ? ch1 + 3 : ch1;
                            //rise event
                            Rs232Interface.GetInstance.SendToParser(RxPacket);
                            //if(ch1 != 0)
                            //{
                            Rs232Interface.GetInstance.SendToParser(new PacketFields
                            {
                                Data2Send = "",
                                ID = Convert.ToInt16(60),
                                SubID = Convert.ToInt16(9), // Get Units Name
                                IsSet = false,
                                IsFloat = false
                            });
                            //}
                            ChannelsplotActivationMerge();
                        }
                        else
                        {
                            if(OscilloscopeParameters.ScaleAndGainList.Count != 0)
                            {
                                _yzoom = OscilloscopeParameters.ScaleAndGainList[ch1].Item2;
                                YLimit = new DoubleRange(-_yzoom, _yzoom); //ubdate visible limits        
                                YVisibleRange = YLimit;
                            }
                        }
                        //update step
                        StepRecalcMerge();
                        //update y axes
                        ChannelYtitles.TryGetValue(_selectedCh1DataSource, out _ch1Title);
                        YaxeTitle = _ch1Title == _ch2Title ? _ch1Title : "";
                    }
                    OnPropertyChanged("SelectedCh1DataSource");
                }
            }
        }
        public string SelectedCh2DataSource
        {
            get { return _selectedCh2DataSource; }
            set
            {
                //if(!_is_freeze)
                {
                    //if(_selectedCh2DataSource == value)
                    //    return;
                    _selectedCh2DataSource = value;

                    lock(ParserRayonM1.PlotListLock)
                    {
                        ch2 = _channel1SourceItems.FindIndex(x => x.Equals(_selectedCh2DataSource));
                        //y axle update
                        ChannelsYaxeMerge(ch2, 2);

                        if(Rs232Interface.GetInstance.IsSynced)
                        {
                            //Send command to the target 
                            PacketFields RxPacket;
                            RxPacket.ID = 60;
                            RxPacket.IsFloat = false;
                            RxPacket.IsSet = true;
                            RxPacket.SubID = 2;
                            RxPacket.Data2Send = (ch2 >= 28) ? ch2 + 3 : ch2;
                            //rise event
                            Rs232Interface.GetInstance.SendToParser(RxPacket);
                            //if(ch2 != 0)
                            //{
                            Rs232Interface.GetInstance.SendToParser(new PacketFields
                            {
                                Data2Send = "",
                                ID = Convert.ToInt16(60),
                                SubID = Convert.ToInt16(10), // Get Units Name
                                IsSet = false,
                                IsFloat = false
                            });
                            //}
                            ChannelsplotActivationMerge();
                        }
                        else
                        {
                            if(OscilloscopeParameters.ScaleAndGainList.Count != 0)
                            {
                                _yzoom = OscilloscopeParameters.ScaleAndGainList[ch2].Item2;
                                YLimit = new DoubleRange(-_yzoom, _yzoom); //ubdate visible limits        
                                YVisibleRange = YLimit;
                            }
                        }
                        //update step
                        StepRecalcMerge();
                        //update y axes
                        ChannelYtitles.TryGetValue(_selectedCh2DataSource, out _ch2Title);
                        //update tittle
                        YaxeTitle = _ch1Title == _ch2Title ? _ch1Title : "";



                        //Packetizer.GetInstance.PlotPacketsList.Clear();
                        //if (Rs232Interface._comPort != null)
                        //    Rs232Interface.GetInstance.Connect();
                        //}
                        //Rs232Interface.GetInstance.Rx2Packetizer += Packetizer.GetInstance.MakePacketsBuff;
                    }

                    OnPropertyChanged("SelectedCh2DataSource");
                }
            }
        }
        private void ChannelsplotActivationMerge()
        {
            //Activate plot
            if(OscilloscopeParameters.ChanTotalCounter > 0 && plotActivationstate == 0)
            {
                _series0 = new XyDataSeries<float, float>();
                _series1 = new XyDataSeries<float, float>();

                _series0.Clear();
                ChartData = _series0;
                _series1.Clear();
                ChartData1 = _series1;

                OnExampleEnter();
                plotActivationstate = 1;
            }
            else if(OscilloscopeParameters.ChanTotalCounter == 0 && plotActivationstate == 1)
            {
                plotActivationstate = 0;
                OnExampleExit();
            }
        }
        private void ChannelsYaxeMerge(int ch, int comboBox)
        {
            if(ch >= 0 && OscilloscopeParameters.ScaleAndGainList.Count != 0)
            {
                //Channel One
                switch(comboBox)
                {
                    case (1):
                        _chan1Counter = (ch == 0) ? 0 : 1;
                        //Update Y axel, gain,fullscale
                        OscilloscopeParameters.Gain = OscilloscopeParameters.ScaleAndGainList.ElementAt(ch).Item1;
                        OscilloscopeParameters.FullScale = OscilloscopeParameters.ScaleAndGainList.ElementAt(ch).Item2;
                        break;
                    case (2):
                        _chan2Counter = (ch == 0) ? 0 : 1;
                        OscilloscopeParameters.Gain2 = OscilloscopeParameters.ScaleAndGainList.ElementAt(ch).Item1;
                        OscilloscopeParameters.FullScale2 = OscilloscopeParameters.ScaleAndGainList.ElementAt(ch).Item2;
                        break;
                }
            }
            OscilloscopeParameters.ChanTotalCounter = _chan1Counter + _chan2Counter;

            if(OscilloscopeParameters.ChanTotalCounter == 1)
            {
                YVisibleRange = ch1 != 0
                    ? new DoubleRange(min: -OscilloscopeParameters.FullScale,
                        max: OscilloscopeParameters.FullScale)
                    : new DoubleRange(min: -OscilloscopeParameters.FullScale2,
                        max: OscilloscopeParameters.FullScale2);
                _yzoom = YVisibleRange.Max;
            }
            else if(OscilloscopeParameters.ChanTotalCounter == 2) //both of channels active
            {
                if(!RefreshManger.GetInstance.DisconnectedFlag)
                {
                    YVisibleRange = OscilloscopeParameters.FullScale > OscilloscopeParameters.FullScale2
                    ? new DoubleRange(min: -OscilloscopeParameters.FullScale,
                        max: OscilloscopeParameters.FullScale)
                    : new DoubleRange(min: -OscilloscopeParameters.FullScale2,
                    max: OscilloscopeParameters.FullScale2);

                    _yzoom = YVisibleRange.Max;

                    XLimit = new DoubleRange(0, _duration); //ubdate visible limits        
                    XVisibleRange = XLimit;
                    _undesample = 1;

                    pivot = (int)0; //update pivote and move to initial state
                    _isFull = false;

                    //using(this.ChartData.SuspendUpdates())
                    //{
                    //_series1.Clear();
                    //_series0.Clear();
                    //}
                    _yFloats = new float[0];
                    _yFloats2 = new float[0];

                    while(ParserRayonM1.GetInstanceofParser.FifoplotList.IsEmpty == false)
                    {
                        float dummy;
                        ParserRayonM1.GetInstanceofParser.FifoplotList.TryDequeue(out dummy);
                    }

                    while(ParserRayonM1.GetInstanceofParser.FifoplotListCh2.IsEmpty == false)
                    {
                        float dummy;
                        ParserRayonM1.GetInstanceofParser.FifoplotListCh2.TryDequeue(out dummy);
                    }

                    if(AllYData.Count > 0)
                    {
                        AllYData.Clear();
                    }
                    //update step
                    StepRecalcMerge();
                }
            }
            OscilloscopeParameters.ChanelFreq = OscilloscopeParameters.ChanTotalCounter == 0 ? OscilloscopeParameters.SingleChanelFreqC : OscilloscopeParameters.SingleChanelFreqC / OscilloscopeParameters.ChanTotalCounter;
            int ActualPOintstoPlot = POintstoPlot;//Actual points to plot transit value
            POintstoPlot = (int)(OscilloscopeParameters.ChanelFreq * _duration * 0.001);

            if(ActualPOintstoPlot != POintstoPlot)//Reset
            {
                ActChenCount = 1;
                _isFull = false;
                pivot = 0;
                using(this.ChartData.SuspendUpdates())
                {
                    _series0.Clear();
                    _series1.Clear();
                }
                _yFloats = new float[0];
                _yFloats2 = new float[0];
                AllYData.Clear();
            }
        }
        private void StepRecalcMerge()
        {
            if(_timer != null)
                lock(_timer)
                {
                    OscilloscopeParameters.Step = OscilloscopeParameters.ChanTotalCounter > 0
                        ? OscilloscopeParameters.ChanTotalCounter * 1000 / OscilloscopeParameters.SingleChanelFreqC
                        : 1000 / OscilloscopeParameters.SingleChanelFreqC;
                    OscilloscopeParameters.ChanelFreq =
                        (float)
                            (OscilloscopeParameters.SingleChanelFreqC * (1.0 / OscilloscopeParameters.ChanTotalCounter));

                }
            else
            {
                OscilloscopeParameters.Step = OscilloscopeParameters.ChanTotalCounter > 0
                    ? OscilloscopeParameters.ChanTotalCounter * 1000 / OscilloscopeParameters.SingleChanelFreqC
                    : 1000 / OscilloscopeParameters.SingleChanelFreqC;
                OscilloscopeParameters.ChanelFreq =
                    (float)(OscilloscopeParameters.SingleChanelFreqC * (1.0 / OscilloscopeParameters.ChanTotalCounter));
            }
        }
        #endregion Channels


        public IXyDataSeries<float, float> ChartData
        {
            get { return _series0; }
            set
            {
                _series0 = value;
                OnPropertyChanged("ChartData");
            }
        }
        public IXyDataSeries<float, float> ChartData1
        {
            get { return _series1; }
            set
            {
                _series1 = value;
                OnPropertyChanged("ChartData1");
            }
        }

        private void ResetZoom()
        {
            float maxval = 0, minval = 0;
            XLimit = new DoubleRange(0, _duration);
            if(OscilloscopeParameters.ChanTotalCounter == 1 && Ytemp.Count > 0)
            {
                maxval = Ytemp.Max();
                minval = Ytemp.Min();
            }
            else if(OscilloscopeParameters.ChanTotalCounter == 2 && AllYData.Count > 0 && AllYData2.Count > 0)
            {
                maxval = AllYData.Max() >= AllYData2.Max() ? AllYData.Max() : AllYData2.Max();
                minval = AllYData.Min() <= AllYData2.Min() ? AllYData.Min() : AllYData2.Min();
            }
            else
            {
                minval = -OscilloscopeParameters.FullScale;
                maxval = OscilloscopeParameters.FullScale;
            }
            if(minval == 0)
                minval = -1;
            if(maxval < 1)
                maxval *= 2;
            //YLimit = new DoubleRange(-OscilloscopeParameters.FullScale, OscilloscopeParameters.FullScale);
            YLimit = new DoubleRange(minval + 0.1 * minval, maxval + 0.1 * maxval);
            _yzoom = OscilloscopeParameters.FullScale;
            XVisibleRange = XLimit;
            YVisibleRange = YLimit;
        }

        //private void ResetZoom()
        //{
        //    if(_selectedDataSource == "Lissajous")
        //    {
        //        XLimit = new DoubleRange(-1.2, 1.2);
        //        YLimit = new DoubleRange(-1.2, 1.2);
        //    }
        //    else
        //    {
        //        XLimit = new DoubleRange(2.5, 4.5);
        //        YLimit = new DoubleRange(-12.5, 12.5);
        //    }

        //    XVisibleRange = XLimit;
        //    YVisibleRange = YLimit;
        //}
        private void ClearGraph()
        {
            lock(this)
            {
                try
                {
                    using(this.ChartData.SuspendUpdates())
                    {
                        using(this.ChartData1.SuspendUpdates())
                        {
                            _yFloats = new float[0];
                            _yFloats2 = new float[0];
                            pivot = 0;
                            _series0.Clear();
                            _series1.Clear();
                        }
                    }
                }
                catch(Exception e) { }
            }
        }
        public string YaxeTitle
        {
            get { return _yaxeTitle; }
            set
            {
                if(_yaxeTitle == value)
                    return;
                else
                {
                    _yaxeTitle = value;
                }
                OnPropertyChanged("YaxeTitle");
            }
        }
        public bool IsDigitalLine
        {
            get { return _isDigitalLine; }
            set
            {
                if(_isDigitalLine == value)
                    return;

                _isDigitalLine = value;
                OnPropertyChanged("IsDigitalLine");
            }
        }
        public DoubleRange XVisibleRange
        {
            get { return _xVisibleRange; }
            set
            {
                if(_xVisibleRange == value)
                    return;

                _xVisibleRange = value;
                OnPropertyChanged("XVisibleRange");
            }
        }
        public DoubleRange YVisibleRange
        {
            get { return _yVisibleRange; }
            set
            {
                if(_yVisibleRange == value)
                    return;

                _yVisibleRange = value;
                OnPropertyChanged("YVisibleRange");
            }
        }
        private string _yAxisUnits = "";
        public string YAxisUnits
        {
            get { return _yAxisUnits; }
            set
            {
                if(_yAxisUnits == value)
                    return;
                if(_yAxisUnits == "")
                {
                    _yAxisUnits = value;
                }
                else if(_yAxisUnits != null)
                {
                    char[] separator = { ':', ',' };
                    string[] words = _yAxisUnits.Split(separator);
                    string[] newWords = value.Split(separator);
                    if(words[0] == "CH1" && newWords[0] == "CH1")
                    {
                        if(words.Length < 3)
                            value = "CH1:" + newWords[1];
                        else
                            value = "CH1:" + newWords[1] + ", CH2:" + words[3];
                        _yAxisUnits = value;
                    }
                    else if(words[0] == "CH1" && newWords[0] == "CH2")
                    {
                        value = "CH1:" + words[1] + ", CH2:" + newWords[1];
                        _yAxisUnits = value;
                    }
                    else if(words[0] == "CH2" && newWords[0] == "CH1")
                    {
                        value = "CH1:" + newWords[1] + ", CH2:" + words[1];
                        _yAxisUnits = value;
                    }
                    else if(words[0] == "CH2" && newWords[0] == "CH2")
                    {
                        value = "CH2:" + newWords[1];
                        _yAxisUnits = value;
                    }
                }

                OnPropertyChanged("YAxisUnits");
            }
        }
        public ModifierType ChartModifier
        {
            get { return _chartModifier; }
            set
            {
                _chartModifier = value;
                OnPropertyChanged("ChartModifier");
                OnPropertyChanged("IsRolloverSelected");
                OnPropertyChanged("IsCursorSelected");
            }
        }
        public ResamplingMode SeriesResamplingMode
        {
            get { return _resamplingMode; }
            set
            {
                if(_resamplingMode == value)
                    return;
                _resamplingMode = value;
                OnPropertyChanged("SeriesResamplingMode");
            }
        }
        private void SetModifier(ModifierType modifierType)
        {
            ChartModifier = modifierType;
        }

        // Reset state when example exits
        public void OnExampleExit()
        {
            lock(this)
            {
                if(_timer != null)
                {
                    lock(_timer)
                    {
                        _timer.Stop();
                        _timer.Elapsed -= OnTick;
                        _chartRunning = false;
                        _timer = null;
                        Thread.Sleep(10);
                        // ChartData = null;
                    }
                }
            }
        }
        // Setup start condition when the example enters
        public void OnExampleEnter()
        {
            if(_timer == null)
            {
                Task.Factory.StartNew(action: () =>
                {
                    Thread.Sleep(100);
                    _timer = new Timer(TimerIntervalMs) { AutoReset = true };
                    _timer.Elapsed += OnTick;
                    _chartRunning = true;
                    _timer.Start();
                });
            }
        }

        private float[] _yFloats;
        private float[] _yFloats2;

        private float[] temp3;
        private float[] temp4;
        private int carry;
        private int carry2;
        private float[] yDataTemp;
        private float[] yDataTemp2;
        public static bool _chartRunning = false;
        /* On ticj function */
        //float item;
        //float item2;
        List<float> ytemp = new List<float>();
        List<float> ytemp2 = new List<float>();

        public static List<float> Ytemp = new List<float>();
        private void OnTick(object sender, EventArgs e)
        {
            if(!IsFreeze)
            {
                lock(_timer)
                {
                    State = 0;
                    if(OscilloscopeParameters.ChanTotalCounter == 1)
                    {
                        #region SingleChan
                        if(ParserRayonM1.GetInstanceofParser.FifoplotList.IsEmpty)
                        {
                            if(AllYData.Count > 1 && _isFull)
                                State = 4;
                            else
                                return;
                        }
                        else if(ActChenCount == 1)//First throw
                        {
                            float item;
                            while(ParserRayonM1.GetInstanceofParser.FifoplotList.TryDequeue(out item))
                            { }
                            ActChenCount = 0;
                        }
                        else //Collect whole the Data to the single grand list
                        {
                            Ytemp = new List<float>();
                            float item;
                            //Record 
                            //if(RecFlag)
                            //{
                            //    string[][] output;
                            //}

                            //retrieve all the data
                            float SubGain1 = Convert.ToSingle(Commands.GetInstance.Gain["Ch1 Gain"].CommandValue);
                            float SubGain2 = Convert.ToSingle(Commands.GetInstance.Gain["Ch2 Gain"].CommandValue);
                            while(ParserRayonM1.GetInstanceofParser.FifoplotList.TryDequeue(out item))
                            {
                                if(ch1 != 0)
                                    Ytemp.Add(item * OscilloscopeParameters.Gain * OscilloscopeParameters.FullScale * SubGain1);
                                else
                                    Ytemp.Add(item * OscilloscopeParameters.Gain2 * OscilloscopeParameters.FullScale2 * SubGain2);
                                //Record
                                if(RecFlag)
                                {
                                    if(SelectedCh1DataSource != "Pause:")
                                        RecList.Add(item * OscilloscopeParameters.Gain * OscilloscopeParameters.FullScale);
                                    else if(SelectedCh2DataSource != "Pause:")
                                        RecList2.Add(item * OscilloscopeParameters.Gain2 * OscilloscopeParameters.FullScale2);
                                }
                            }
                            AllYData.AddRange(Ytemp);

                            if(_isFull)
                                State = 4;
                            else if(POintstoPlot > pivot && _isFull == false)//fills buffer
                                State = 2;
                            else if(POintstoPlot == pivot && _isFull == false) //buffer is full
                            {
                                _isFull = true;
                                State = 4;
                            }
                            else
                                return;
                            #region Switch
                            switch(State)
                            {
                                case (2): //Fills y buffer
                                    float[] temp;
                                    if((POintstoPlot - pivot) > 0)
                                        temp = AllYData.Take(POintstoPlot - pivot).ToArray();
                                    else
                                        return;
                                    if(_undesample == 1)
                                    {
                                        if(_yFloats.Length == 0) //Start fills
                                        {
                                            _yFloats = new float[temp.Length];
                                            xData = new float[temp.Length];

                                            //X fills
                                            for(int i = 0; i < temp.Length; i++)
                                            {
                                                xData[i] = i * OscilloscopeParameters.Step;
                                            }

                                            Array.Copy(temp, 0, _yFloats, 0, temp.Length);
                                            pivot = temp.Length;
                                        } //Follow
                                        else
                                        {
                                            Array.Resize(ref xData, temp.Length + pivot);
                                            Array.Resize(ref _yFloats, temp.Length + pivot);

                                            for(int i = 0; i < pivot + temp.Length; i++)
                                            {
                                                xData[i] = i * OscilloscopeParameters.Step;
                                            }
                                            Array.Copy(temp, 0, _yFloats, pivot, temp.Length);
                                            pivot = pivot + temp.Length;
                                        }
                                    }
                                    else // under sampled
                                    {
                                        if(_yFloats.Length == 0 && pivot == 0) //Start fills
                                        {

                                            utemp3L = new List<float>();

                                            for(int i = 0/*, j = 0*/; i < temp.Length; i++)
                                            {
                                                if(_undesampleCounter++ == (_undesample - 1))
                                                {
                                                    _undesampleCounter = 0;
                                                    utemp3L.Add(temp[i]);
                                                }
                                            }

                                            xData = new float[utemp3L.Count];

                                            for(int i = 0; i < utemp3L.Count; i++)
                                            {
                                                xData[i] = i * OscilloscopeParameters.Step * _undesample;
                                            }

                                            _yFloats = new float[utemp3L.Count];

                                            Array.Copy(utemp3L.ToArray(), 0, _yFloats, 0, utemp3L.Count);
                                            pivot += utemp3L.Count;
                                        } //Follow
                                        else
                                        {
                                            utemp3L = new List<float>();

                                            for(int i = 0/*, j = 0*/; i < temp.Length; i++)
                                            {
                                                if(_undesampleCounter++ == (_undesample - 1))
                                                {
                                                    _undesampleCounter = 0;
                                                    utemp3L.Add((float)temp[i]);
                                                }
                                            }
                                            Array.Resize(ref xData, utemp3L.Count + pivot);
                                            Array.Resize(ref _yFloats, utemp3L.Count + pivot);

                                            for(int i = 0; i < utemp3L.Count + pivot; i++)
                                            {
                                                xData[i] = i * OscilloscopeParameters.Step * _undesample;
                                            }

                                            Array.Copy(utemp3L.ToArray(), 0, _yFloats, pivot, utemp3L.Count);
                                            pivot += utemp3L.Count;

                                        }

                                    }
                                    lock(this)
                                    {
                                        using(this.ChartData.SuspendUpdates())
                                        {
                                            _series0.Clear();
                                            _series1.Clear();

                                            if(ch1 != 0)
                                                _series0.Append(xData, _yFloats);
                                            if(ch2 != 0)
                                                _series1.Append(xData, _yFloats);
                                        }
                                    }



                                    AllYData.RemoveRange(0, temp.Length - 1);

                                    if(utemp3L != null && utemp3L.Count > 0)
                                    {
                                        utemp3L.Clear();
                                    }
                                    break;

                                case (4):
                                    if(_undesample == 1)
                                    {
                                        temp3 = AllYData.Take(POintstoPlot).ToArray();
                                        carry = temp3.Length;
                                        yDataTemp = new float[POintstoPlot];
                                        Array.Copy(_yFloats, carry, yDataTemp, 0, _yFloats.Length - (carry)); //Shift Left
                                        Array.Copy(temp3, 0, yDataTemp, _yFloats.Length - carry, carry); // Add range
                                        Array.Copy(yDataTemp, 0, _yFloats, 0, POintstoPlot);

                                        //  var watch = Stopwatch.StartNew();                     
                                        //watch.Stop();
                                        //float elapsedMs = (float)watch.ElapsedMilliseconds;
                                        //if (elapsedMs > 35)
                                        //    Averadge++;
                                        // return;

                                        for(int i = 0; i < POintstoPlot; i++)
                                        {
                                            xData[i] = i * (OscilloscopeParameters.Step * _undesample);
                                        }

                                        using(this.ChartData.SuspendUpdates())
                                        {
                                            _series0.Clear();
                                            _series1.Clear();

                                            if(ch1 != 0)
                                                _series0.Append(xData, _yFloats);
                                            else if(ch2 != 0)
                                                _series1.Append(xData, _yFloats);
                                        }

                                        AllYData.RemoveRange(0, (carry) - 1);
                                    }
                                    else
                                    {
                                        utemp3L = new List<float>();
                                        utemp3 = AllYData.Take(POintstoPlot).ToArray(); //Take Data
                                        ucarry = utemp3.Length;

                                        for(int i = 0/*, j = 0*/; i < utemp3.Length; i++)
                                        {
                                            if(_undesampleCounter++ == (_undesample - 1))
                                            {
                                                _undesampleCounter = 0;
                                                utemp3L.Add(utemp3[i]);
                                            }
                                        }

                                        temp3 = utemp3L.ToArray();
                                        carry = temp3.Length;
                                        yDataTemp = new float[POintstoPlot];
                                        Array.Copy(_yFloats, carry, yDataTemp, 0, _yFloats.Length - (carry)); //Shift Left
                                        Array.Copy(temp3, 0, yDataTemp, _yFloats.Length - carry, carry); // Add range
                                        Array.Copy(yDataTemp, 0, _yFloats, 0, POintstoPlot);

                                        for(int i = 0; i < POintstoPlot; i++)
                                        {
                                            xData[i] = i * (OscilloscopeParameters.Step * _undesample);
                                        }

                                        using(this.ChartData.SuspendUpdates())
                                        {
                                            _series0.Clear();
                                            _series1.Clear();

                                            if(ch1 != 0)
                                                _series0.Append(xData, _yFloats);
                                            else if(ch2 != 0)
                                                _series1.Append(xData, _yFloats);
                                        }

                                        AllYData.RemoveRange(0, (ucarry) - 1);
                                    }
                                    break;
                            }
                            #endregion Switch
                        }
                        #endregion
                    }
                    else if(OscilloscopeParameters.ChanTotalCounter == 2)// Two channels
                    {
                        #region DoubleChan
                        if(ParserRayonM1.GetInstanceofParser.FifoplotList.IsEmpty)
                        {
                            if(AllYData.Count > 1 && _isFull)
                                State = 4;
                            else
                                return;
                        }
                        else if(ActChenCount == 1)//First throw
                        {
                            float item;
                            float item2;
                            //Collect data from first channel
                            while(ParserRayonM1.GetInstanceofParser.FifoplotList.TryDequeue(out item))
                            {
                                ParserRayonM1.GetInstanceofParser.FifoplotListCh2.TryDequeue(out item2);
                            }
                            ActChenCount = 0;
                        }
                        else //Collect whole the Data to the single grand list
                        {
                            float item;
                            float item2;
                            Debug.WriteLine("Plot 1: " + DateTime.Now.ToString("h:mm:ss.fff"));

                            //Collect data from first channel
                            #region RecordAray
                            if(RecFlag)
                            {
                                while(ParserRayonM1.GetInstanceofParser.FifoplotList.TryDequeue(out item))
                                {
                                    AllYData.Add(item * OscilloscopeParameters.Gain * OscilloscopeParameters.FullScale);
                                    ParserRayonM1.GetInstanceofParser.FifoplotListCh2.TryDequeue(out item2);
                                    AllYData2.Add(item2 * OscilloscopeParameters.Gain2 * OscilloscopeParameters.FullScale2);

                                    RecList.Add(item * OscilloscopeParameters.Gain * OscilloscopeParameters.FullScale);
                                    RecList2.Add(item2 * OscilloscopeParameters.Gain2 * OscilloscopeParameters.FullScale2);

                                }
                            }
                            #endregion RecordAray
                            else
                            {
                                float SubGain1 = Convert.ToSingle(Commands.GetInstance.Gain["Ch1 Gain"].CommandValue);
                                float SubGain2 = Convert.ToSingle(Commands.GetInstance.Gain["Ch2 Gain"].CommandValue);

                                while(ParserRayonM1.GetInstanceofParser.FifoplotList.TryDequeue(out item))
                                {
                                    AllYData.Add(item * OscilloscopeParameters.Gain * OscilloscopeParameters.FullScale * SubGain1);
                                    ParserRayonM1.GetInstanceofParser.FifoplotListCh2.TryDequeue(out item2);
                                    AllYData2.Add(item2 * OscilloscopeParameters.Gain2 * OscilloscopeParameters.FullScale2 * SubGain2);
                                }
                            }

                            if(_isFull)
                                State = 4;
                            else if(POintstoPlot > pivot && _isFull == false)//fills buffer
                                State = 2;
                            else if(POintstoPlot == pivot && _isFull == false) //buffer is full
                            {
                                _isFull = true;
                                State = 4;
                            }
                            else
                                return;
                            #region Switch

                            switch(State)
                            {
                                case (2): //Fills y buffer
                                    #region case2
                                    float[] temp;
                                    float[] temp2;

                                    if((POintstoPlot - pivot) > 0)
                                    {
                                        temp2 = AllYData2.Take(POintstoPlot - pivot).ToArray();
                                        temp = AllYData.Take(POintstoPlot - pivot).ToArray();
                                    }
                                    else
                                        return;


                                    if(_yFloats.Length == 0) //Start fills
                                    {
                                        _yFloats = new float[temp.Length];
                                        _yFloats2 = new float[temp2.Length];

                                        xData = new float[temp.Length];

                                        for(int i = 0; i < temp.Length; i++)
                                        {
                                            xData[i] = i * OscilloscopeParameters.Step;
                                        }

                                        Array.Copy(temp, 0, _yFloats, 0, temp.Length);
                                        Array.Copy(temp2, 0, _yFloats2, 0, temp2.Length);
                                        pivot = temp.Length;
                                    } //Follow
                                    else
                                    {
                                        Array.Resize(ref xData, temp.Length + pivot);
                                        Array.Resize(ref _yFloats, temp.Length + pivot);
                                        Array.Resize(ref _yFloats2, temp2.Length + pivot);

                                        for(int i = 0; i < pivot + temp.Length; i++)
                                        {
                                            xData[i] = i * OscilloscopeParameters.Step;
                                        }
                                        Array.Copy(temp, 0, _yFloats, pivot, temp.Length);
                                        Array.Copy(temp2, 0, _yFloats2, pivot, temp.Length);
                                        pivot = pivot + temp.Length;
                                    }
                                    lock(this)
                                    {
                                        using(this.ChartData.SuspendUpdates())
                                        {
                                            using(this.ChartData1.SuspendUpdates())
                                            {
                                                _series0.Clear();
                                                _series1.Clear();
                                                _series0.Append(xData, _yFloats);
                                                _series1.Append(xData, _yFloats2);
                                            }
                                        }
                                    }

                                    AllYData.RemoveRange(0, temp.Length - 1);
                                    AllYData2.RemoveRange(0, temp2.Length - 1);
                                    #endregion case2
                                    break;

                                case (4):
                                    //_isFull = false;
                                    temp3 = AllYData.Take(POintstoPlot).ToArray();
                                    temp4 = AllYData2.Take(POintstoPlot).ToArray();

                                    carry = temp3.Length;
                                    carry2 = temp4.Length;
                                    //1
                                    yDataTemp = new float[POintstoPlot];
                                    Array.Copy(_yFloats, carry, yDataTemp, 0, _yFloats.Length - (carry)); //Shift Left - public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length);
                                    Array.Copy(temp3, 0, yDataTemp, _yFloats.Length - carry, carry); // Add range
                                    Array.Copy(yDataTemp, 0, _yFloats, 0, POintstoPlot);

                                    //2
                                    yDataTemp2 = new float[POintstoPlot];
                                    Array.Copy(_yFloats2, carry, yDataTemp2, 0, _yFloats2.Length - (carry2)); //Shift Left
                                    Array.Copy(temp4, 0, yDataTemp2, _yFloats2.Length - carry2, carry2); // Add range
                                    Array.Copy(yDataTemp2, 0, _yFloats2, 0, POintstoPlot);

                                    for(int i = 0; i < POintstoPlot; i++)
                                        xData[i] = i * (OscilloscopeParameters.Step * _undesample);

                                    //lock(this)
                                    //{
                                    using(this.ChartData.SuspendUpdates())
                                    {
                                        using(this.ChartData1.SuspendUpdates())
                                        {
                                            _series0.Clear();
                                            _series1.Clear();
                                            _series0.Append(xData, _yFloats);
                                            _series1.Append(xData, _yFloats2);
                                        }
                                    }
                                    //}

                                    AllYData.RemoveRange(0, (carry) - 1);
                                    AllYData2.RemoveRange(0, (carry2) - 1);
                                    break;

                            }
                            #endregion Switch

                            //Debug.WriteLine("POintstoPlot {0}, pivot {1}: ", POintstoPlot, pivot);

                            Debug.WriteLine("Plot 2: " + DateTime.Now.ToString("h:mm:ss.fff"));
                            #endregion
                        }
                    }
                }
            }
        }

        private DoubleRange _xLimit;
        public DoubleRange XLimit
        {
            get { return _xLimit; }
            set
            {
                if(_xLimit == value)
                    return;
                _xLimit = value;
                OnPropertyChanged("XLimit");
            }
        }
        private DoubleRange _yLimit;
        public DoubleRange YLimit
        {
            get { return _yLimit; }
            set
            {
                if(_yLimit == value)
                    return;
                _yLimit = value;
                OnPropertyChanged("YLimit");
            }
        }


        public ActionCommand RecordCommand
        {
            get { return new ActionCommand(Record); }
        }

        private string filePath;
        //private float RecDtx = 0;
        //private int _xlsCounter = 0;
        private bool RecFlag = false;
        //private float ChangeValue = 0;
        string delimiter = ",";

        public void Record()
        {
            if(RecFlag == true)
            {
                string Date = Day(DateTime.Now.Day) + ' ' + MonthTrans(DateTime.Now.Month) + ' ' + DateTime.Now.Year.ToString();
                string path = "\\MotorController\\Charts\\" + Date + ' ' + DateTime.Now.ToString("HH:mm:ss");
                path = (path.Replace('-', ' ')).Replace(':', '_');
                path += ".csv";
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + path;
                string name = LeftPanelViewModel.name;
                filePath = path;// LeftPanelViewModel.Recordsrootpath + name + "_Record.csv";
                if(!File.Exists(filePath))
                {
                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MotorController\\Charts\\";
                    if(!Directory.Exists(tempPath))
                        Directory.CreateDirectory(tempPath);
                    File.Create(filePath);
                }
                //else if(File.Exists(filePath))
                //{
                //    File.Delete(filePath);
                //    File.Create(filePath).Close();
                //    RecFlag = true;
                //}
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    RecFlag = false;
                    Thread.Sleep(100);
                    StringBuilder sb = new StringBuilder();

                    // string[] output = new string[RecList.Count];
                    // float[] xxls = new float[RecList.Count];
                    if(RecList.Count <= 1040000)
                    {
                        float[] yxls = RecList.ToArray();
                        float[] yxls2 = RecList2.ToArray();

                        string[] xstring = new string[RecList.Count + 1];

                        string[] ystring = new string[RecList.Count + 1];
                        if(SelectedCh1DataSource != "Pause:")
                        {
                            xstring = new string[RecList.Count + 1];
                            ystring[0] = "Channel 1 - " + SelectedCh1DataSource;
                        }

                        string[] ystring2 = new string[RecList2.Count + 1];
                        if(SelectedCh2DataSource != "Pause:")
                        {
                            xstring = new string[RecList2.Count + 1];
                            ystring2[0] = "Channel 2 - " + SelectedCh2DataSource;
                        }
                        xstring[0] = "Time";

                        if(SelectedCh1DataSource != "Pause:" && SelectedCh2DataSource != "Pause:")
                        {
                            for(int i = 1; i < RecList.Count; i++)
                            {
                                xstring[i] = (i * OscilloscopeParameters.Step).ToString(CultureInfo.CurrentCulture);
                                ystring[i] = yxls[i - 1].ToString(CultureInfo.CurrentCulture);
                                ystring2[i] = yxls2[i - 1].ToString(CultureInfo.CurrentCulture);
                                sb.AppendLine(string.Join(delimiter, xstring[i - 1], ystring[i - 1], ystring2[i - 1]));
                            }
                            RecList.Clear();
                            RecList2.Clear();
                        }
                        else if(SelectedCh1DataSource != "Pause:")
                        {
                            for(int i = 1; i < RecList.Count; i++)
                            {
                                xstring[i] = (i * OscilloscopeParameters.Step).ToString(CultureInfo.CurrentCulture);
                                ystring[i] = yxls[i - 1].ToString(CultureInfo.CurrentCulture);
                                sb.AppendLine(string.Join(delimiter, xstring[i - 1], ystring[i - 1]));
                            }
                            RecList.Clear();
                        }
                        else if(SelectedCh2DataSource != "Pause:")
                        {
                            for(int i = 1; i < RecList2.Count; i++)
                            {
                                xstring[i] = (i * OscilloscopeParameters.Step).ToString(CultureInfo.CurrentCulture);
                                ystring2[i] = yxls2[i - 1].ToString(CultureInfo.CurrentCulture);
                                sb.AppendLine(string.Join(delimiter, xstring[i - 1], ystring2[i - 1]));
                            }
                            RecList2.Clear();
                        }

                        File.AppendAllText(filePath, sb.ToString());
                        sb.Clear();
                    }
                    else
                    {
                        int count = 0;
                        for(int j = 0, index = 0; j < RecList.Count; j += 1040000, index++)
                        {
                            if((j + 1040000) < RecList.Count)
                                count = 1040000;
                            else
                                count = RecList.Count - j - 2;

                            List<float> yxls = RecList.GetRange(j, count);
                            List<float> yxls2 = RecList2.GetRange(j, count);

                            string[] xstring = new string[count + 1];

                            string[] ystring = new string[count + 1];
                            if(SelectedCh1DataSource != "Pause:")
                            {
                                xstring = new string[count + 1];
                                ystring[0] = "Channel 1 - " + SelectedCh1DataSource;
                            }

                            string[] ystring2 = new string[count + 1];
                            if(SelectedCh2DataSource != "Pause:")
                            {
                                xstring = new string[count + 1];
                                ystring2[0] = "Channel 2 - " + SelectedCh2DataSource;
                            }
                            xstring[0] = "Time";

                            if(SelectedCh1DataSource != "Pause:" && SelectedCh2DataSource != "Pause:")
                            {
                                for(int i = 1; i < count; i++)
                                {
                                    xstring[i] = (i * OscilloscopeParameters.Step).ToString(CultureInfo.CurrentCulture);
                                    ystring[i] = yxls[i - 1].ToString(CultureInfo.CurrentCulture);
                                    ystring2[i] = yxls2[i - 1].ToString(CultureInfo.CurrentCulture);
                                    sb.AppendLine(string.Join(delimiter, xstring[i - 1], ystring[i - 1], ystring2[i - 1]));
                                }
                                //RecList.Clear();
                                //RecList2.Clear();
                            }
                            else if(SelectedCh1DataSource != "Pause:")
                            {
                                for(int i = 1; i < count; i++)
                                {
                                    xstring[i] = (i * OscilloscopeParameters.Step).ToString(CultureInfo.CurrentCulture);
                                    ystring[i] = yxls[i - 1].ToString(CultureInfo.CurrentCulture);
                                    sb.AppendLine(string.Join(delimiter, xstring[i - 1], ystring[i - 1]));
                                }
                                //RecList.Clear();
                            }
                            else if(SelectedCh2DataSource != "Pause:")
                            {
                                for(int i = 1; i < count; i++)
                                {
                                    xstring[i] = (i * OscilloscopeParameters.Step).ToString(CultureInfo.CurrentCulture);
                                    ystring2[i] = yxls2[i - 1].ToString(CultureInfo.CurrentCulture);
                                    sb.AppendLine(string.Join(delimiter, xstring[i - 1], ystring2[i - 1]));
                                }
                                //RecList2.Clear();
                            }
                            filePath = filePath.Substring(0, filePath.Length - 4) + "_" + j.ToString() + ".csv";
                            File.AppendAllText(filePath, sb.ToString());
                            sb.Clear();
                        }
                    }
                });
            }

        }
        public static string MonthTrans(int month)
        {
            switch(month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "Septembre";
                case 10:
                    return "Octobre";
                case 11:
                    return "Novembre";
                case 12:
                    return "Decembre";
                default:
                    return "x";
            }

        }
        public static string Day(int day)
        {
            if(day < 10)
            {
                return "0" + day.ToString();
            }
            else
                return day.ToString();

        }

        private bool _is_recording = false;
        private bool _is_freeze = false;
        public bool IsRecording
        {
            get { return _is_recording; }
            set
            {
                if(_is_recording == value)  return; 
                if(OscilloscopeParameters.ChanTotalCounter > 0 && plotActivationstate == 1)
                {
                    _is_recording = value;
                    if(value)
                    {
                        RecFlag = true;
                        Record();
                    }
                    else
                    {
                        RecFlag = false;
                        Record();
                    }
                }
                else
                {
                    EventRiser.Instance.RiseEevent(string.Format($"No plot detected !"));
                    _is_recording = false;
                }
                //SaveToDisk.IsRecording = value;

                OnPropertyChanged("IsRecording");
            }
        }
        static int Ch1Value = 0;
        static int Ch2Value = 0;
        public bool IsFreeze
        {
            get { return _is_freeze; }
            set
            {
                if(_is_freeze == value)
                { return; }
                if(value && OscilloscopeParameters.ChanTotalCounter > 0 && plotActivationstate == 1)
                {
                    _is_freeze = value;
                    // OnPropertyChanged("IsFreeze");
                    Ch1Value = _channel1SourceItems.FindIndex(x => x.Equals(_selectedCh1DataSource));
                    Ch2Value = _channel1SourceItems.FindIndex(x => x.Equals(_selectedCh2DataSource));
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = 0,
                        ID = Convert.ToInt16(60),
                        SubID = Convert.ToInt16(1),
                        IsSet = true,
                        IsFloat = false
                    });
                    Thread.Sleep(20);
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = 0,
                        ID = Convert.ToInt16(60),
                        SubID = Convert.ToInt16(2),
                        IsSet = true,
                        IsFloat = false
                    });
                }
                else if(_is_freeze && !value)
                {
                    _is_freeze = value;
                    //OnPropertyChanged("IsFreeze");
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = Ch1Value,
                        ID = Convert.ToInt16(60),
                        SubID = Convert.ToInt16(1),
                        IsSet = true,
                        IsFloat = false
                    });
                    Thread.Sleep(20);
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = Ch2Value,
                        ID = Convert.ToInt16(60),
                        SubID = Convert.ToInt16(2),
                        IsSet = true,
                        IsFloat = false
                    });
                }
                else
                {
                    EventRiser.Instance.RiseEevent(string.Format($"No plot detected !"));
                    _is_freeze = false;
                }
                //SaveToDisk.IsRecording = value;

                OnPropertyChanged("IsFreeze");
            }
        }
    }
}