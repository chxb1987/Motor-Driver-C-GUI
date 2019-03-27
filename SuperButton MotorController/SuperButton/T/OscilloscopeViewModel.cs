using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Example.Common;
using Abt.Controls.SciChart.Example.Data;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Numerics;
using SuperButton.Common;
using SuperButton.Data;
using SuperButton.Models.DriverBlock;
using SuperButton.Models.ParserBlock;
using Timer = System.Timers.Timer;

//using Abt.Controls.SciChart.Example.MVVM;

namespace SuperButton.Views
{
    public class OscilloscopeViewModel : BaseViewModel, IExampleAware
    {
        private DoubleRange _xVisibleRange;
        private DoubleRange _yVisibleRange;
        private ModifierType _chartModifier;
        private bool _isDigitalLine = true;
        private IXyDataSeries<double, double> _series0,_series1;
        //private Timer _timer;
        private double _phase0 = 0.0;
        private double _phase1 = 0.0;
        private uint _plot_time;
        private double _phaseIncrement;
        private ResamplingMode _resamplingMode;
        private bool _canExecuteRollover;
        private double y,t = 0;
        private Stopwatch plotStopwatch;
        private DoubleRange _animated_x_visible_range;
        public DoubleSeries datasource2;// = new DoubleSeries();
        private int Counter;

        private double Freq=Stopwatch.Frequency;

        private string _selectedDataSource;

        private const double TimerIntervalMs = 50;
        public static OscilloscopeViewModel OscilloscopeViewModelViewModel;

        //private Random _random = new Random();
        //private double dt;
        //private UInt16 Counterp;
        //private double temp;
        //private MicroTimer _timer = new MicroTimer(100);
        //private Timer _timerMili;

        public OscilloscopeViewModel()
        {
            OscilloscopeViewModelViewModel = this;
        }

      

        public ActionCommand ResetZoomCommand { get { return new ActionCommand(ResetZoom); } } //Rest Zoom

        

        public ActionCommand SetRolloverModifierCommand { get { return new ActionCommand(() => SetModifier(ModifierType.Rollover)); } }
        public ActionCommand SetCursorModifierCommand { get { return new ActionCommand(() => SetModifier(ModifierType.CrosshairsCursor)); } }


        public ActionCommand SetRubberBandZoomModifierCommand
        {
            get { return new ActionCommand(() => SetModifier(ModifierType.RubberBandZoom)); }
        }


        public ActionCommand SetZoomPanModifierCommand { get { return new ActionCommand(() => SetModifier(ModifierType.ZoomPan)); } }
        public ActionCommand SetNullModifierCommand { get { return new ActionCommand(() => SetModifier(ModifierType.Null)); } }

        public ActionCommand SetDigitalLineCommand
        {
            get { return new ActionCommand(() => IsDigitalLine = !IsDigitalLine); }
        }
        

        public bool IsRolloverSelected { get { return ChartModifier == ModifierType.Rollover; }}
        public bool IsCursorSelected { get { return ChartModifier == ModifierType.CrosshairsCursor; } }


        #region Prop


        public bool CanExecuteRollover
        {
            get { return _canExecuteRollover; }
            set
            {
                if (_canExecuteRollover == value) return;
                _canExecuteRollover = value;
                OnPropertyChanged("CanExecuteRollover");
            }
        }

        public IXyDataSeries<double, double> ChartData
        {
            get { return _series0; }
            set
            {
                _series0 = value;
                OnPropertyChanged("ChartData");
            }
        }

        public IXyDataSeries<double, double> ChartData1
        {
            get { return _series1; }
            set
            {
                _series0 = value;
                OnPropertyChanged("ChartData1");
            }
        }

        public string SelectedDataSource
        {
            get { return _selectedDataSource; }
            set
            {
                if (_selectedDataSource == value) return;
                _selectedDataSource = value;

                lock (this)
                {
                    if (_selectedDataSource == "Lissajous")
                    {
                        // For Lissajous plots, we must use an UnsortedXyDataSeries 
                        // and we cannot use the Rollover. Currently HitTest/Rollover is not implemented
                        // for UnsortedXyDataSeries. Also this series type does not currently support resampling
                        _phaseIncrement = Math.PI * 0.02;
                        _series0 = new XyDataSeries<double, double>();
                        IsDigitalLine = false;
                        if (ChartModifier == ModifierType.Rollover)
                            SetModifier(ModifierType.CrosshairsCursor);
                        SeriesResamplingMode = ResamplingMode.None;                        
                    }
                    else
                    {
                        // For FourierSeries plots, we can use the faster sorted XyDataSeries, 
                        // which supports the Rollover, HitTest and Resamplingd
                        _phaseIncrement = Math.PI * 0.1;
                        _series0 = new XyDataSeries<double, double>();
                        _series1 = new XyDataSeries<double, double>();
                        IsDigitalLine = true;
                        CanExecuteRollover = true;
                        SeriesResamplingMode = ResamplingMode.None;
                    }

                    // Setup the Zoom Limit (affects double click to zoom extents)
                    ResetZoom();

                    // Add the new dataseries and reset counters. See OnTick where data is appended
                    _series0.SeriesName = _selectedDataSource;
                    _series0.Clear();
                    ChartData = _series0;
                    _phase0 = 0;
                    _phase1 = 0.15;
                }
                OnPropertyChanged("SelectedDataSource");
            }
        }

        private void ResetZoom()
        {
            if (_selectedDataSource == "Lissajous")
            {
                XLimit = new DoubleRange(-1.2, 1.2);
                YLimit = new DoubleRange(-1.2, 1.2);                
            }
            else
            {
                XLimit = new DoubleRange(2.5, 4.5);
                YLimit = new DoubleRange(-12.5, 12.5);
            }

            XVisibleRange = XLimit;
            YVisibleRange = YLimit;
        }

        public bool IsDigitalLine
        {
            get { return _isDigitalLine; }
            set
            {
                if (_isDigitalLine == value) return;

                _isDigitalLine = value;
                OnPropertyChanged("IsDigitalLine");
            }
        }

        public DoubleRange XVisibleRange
        {
            get { return _xVisibleRange; }
            set 
            { 
                if (_xVisibleRange == value) return;

                _xVisibleRange = value;
                OnPropertyChanged("XVisibleRange");
            }
        }

        public DoubleRange YVisibleRange
        {
            get { return _yVisibleRange; }
            set
            {
                if (_yVisibleRange == value) return;

                _yVisibleRange = value;
                OnPropertyChanged("YVisibleRange");
            }
        }

        public ModifierType ChartModifier
        {
            get
            {
                return _chartModifier;
            }
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
                if (_resamplingMode == value) return;
                _resamplingMode = value;
                OnPropertyChanged("SeriesResamplingMode");
            }
        }
        

        private void SetModifier(ModifierType modifierType)
        {
            ChartModifier = modifierType;
        }
        #endregion


        // Reset state when example exits
        public void OnExampleExit()
        {
            //lock (this)
            //{
            //    if (_timer != null)
            //    {
            //        _timer.Stop();
            //       // _timer.Elapsed -= OnTick;
            //        _timer = null;
            //    }
            //    ChartData = null;
            //}
        }

        // Setup start condition when the example enters
        public void OnExampleEnter()
        {

        }


        private DoubleRange _xLimit;

        public DoubleRange XLimit
        {
            get { return _xLimit; }
            set
            {
                if (_xLimit == value) return;
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
                if (_yLimit == value) return;
                _yLimit = value;
                OnPropertyChanged("YLimit");
            }
        }




        //public DoubleSeries GetRandomDoubleSeries(int pointCount)
        //{
        //    var doubleSeries = new DoubleSeries();
        //    double amplitude = _random.NextDouble() + 0.5;
        //    double freq = 0.9;
        //    double offset =0;

        //    for (int i = 0; i < pointCount; i++)
        //    {
        //        doubleSeries.Add(new XYPoint() { X = i, Y = offset + amplitude * Math.Sin(freq) });
        //    }

        //    return doubleSeries;
        //}
    }
}

     