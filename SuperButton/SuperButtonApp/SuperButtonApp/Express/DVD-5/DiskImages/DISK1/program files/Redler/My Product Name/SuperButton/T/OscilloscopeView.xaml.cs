#define MILI

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Common.Helpers;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using SuperButton.Data;
using SuperButton.Models.ParserBlock;
using SuperButton.ViewModels;


namespace SuperButton.Views
{
    /// <summary>
    /// Interaction logic for OscilloscopeView.xaml
    /// </summary>
    public partial class OscilloscopeView : UserControl
    {
 #if FPS
        private Stopwatch _stopWatch;
        private double _lastFrameTime;
        private MovingAverage _fpsAverage= new MovingAverage(50);
#endif

        //Performance_Mili
        private Timer _timer;
        private XyDataSeries<double, float> Ch1Series;
        private const int TimerIntervalmili = 20;//Good- 10; // Interval of the timer to generate data in ms
        private const int BufferSize = 400;//Good-  200; // Interval of the timer to generate data in ms
        private double[] xBuffer = new double[BufferSize];
        private float[] yBuffer = new float[BufferSize];
        
        private double dtx;

        //Perfoemance2_Micro
        private const int TimerIntervalmicro = 100;
        private MicroTimer _timerMicro = new MicroTimer(TimerIntervalmicro);
        private double x;
        private float y;
        


        public OscilloscopeView()
        {
            InitializeComponent();

            //3.01.16 Double click event handler

            //Loaded += (a, e) =>
            //{
            //    if (!DesignerProperties.GetIsInDesignMode(this))
            //    {

            //        oscilloscopeChart.MouseDoubleClick += MouseDoubleClicked;

            //    }
            //};
                                           


            //this.DataContext = new OscilloscopeViewModel();

#if FPS
            // Used purely for FPS reporting
             oscilloscopeChart.Rendered += OnSciChartRendered;
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
#endif
          //  OnExampleEnter();      
        }


#if FPS
        private void OnSciChartRendered(object sender, EventArgs e)
        {
            // Compute the render time
            double frameTime = _stopWatch.ElapsedMilliseconds;
            double delta = frameTime - _lastFrameTime;
            double fps = 1000.0 / delta;

            // Push the fps to the movingaverage, we want to average the FPS to get a more reliable reading
            if (!double.IsInfinity(fps))
            {
                _fpsAverage.Push(fps);
            }

            // Render the fps to the screen
            fpsCounter.Text = double.IsNaN(_fpsAverage.Current) ? "-" : string.Format("{0:0}", _fpsAverage.Current);

            // Render the total point count (all series) to the screen
           // int numPoints = 3 * _mainSeries.Count;
          //  pointCount.Text = string.Format("{0:n0}", numPoints);

           // if (numPoints > MaxCount)
           // {
             //   this.PauseButton_Click(this, null);
          //  }

            _lastFrameTime = frameTime;
        }

#endif

    private void MouseDoubleClicked(object sender, MouseButtonEventArgs e)
    {
        int a = 0;

        //            var parent = this.FindParent<UC_Main>();
        //            /*fix for graphVisibility was set manually*/
        //            if (parent.LeftGrid.IsVisible || parent.RightGrid.IsVisible || parent.BottomGrid.IsVisible){
        //                IsGraphFullScreen = false;
        //            }else{
        //                IsGraphFullScreen = true;
        //            }

        //            if (!IsGraphFullScreen){

        //                //THIS WONT WORK CUZ GRID.WIDTH ISN'T DOUBLE.
        //                //TO SEE HOW TO MAKE IT WORK READ http://stackoverflow.com/questions/17265067/changing-grid-columns-rows-width-height-by-storyboard-animation-in-windows-sto


        //                parent.StartGraphFullScreenAnimation();

        //               // Storyboard sb = (Storyboard)parent.Resources["leftGridStoryboard"];
        //               // sb.Begin();
        //                //parent.LeftGrid.Visibility = parent.RightGrid.Visibility = parent.BottomGrid.Visibility = Visibility.Collapsed;
        //                IsGraphFullScreen = true;
        //            } else{
        //                parent.StartGraphExitFullScreenAnimation();
        //                IsGraphFullScreen = false;
        //            }
        //            return;
        //#if DEBUG
        //            SciChartOsciloscope thisos = FindThis((DependencyObject)sender);
        //            if (thisos.CanOpenGraphFullScreen){
        //                new OsciloscopeFullScreen(this).Show();
        //                CanOpenGraphFullScreen = false;
        //            }
        //#endif
    }



        public void OnExampleEnter()
        {
            // Manages the state of example on enter
           // Reset();
           // _startDelegate = TimedMethod.Invoke(this.Start).After(500).Go();

            using (oscilloscopeChart.SuspendUpdates())
            {
                Channel1Series = (FastLineRenderableSeries)oscilloscopeChart.RenderableSeries[0];

                // Create three DataSeries
                Ch1Series = new XyDataSeries<double, float>();
                Ch1Series.FifoCapacity =100000;//10 sec

                Channel1Series.DataSeries = Ch1Series;

#if MILI
               // _timer = new Timer(TimerIntervalmili);
               // _timer.Elapsed += OnTickMili;
               // _timer.AutoReset = true;
              //  _timer.Start();  
#else
                _timerMicro.MicroTimerElapsed += OnTickMicro;
                _timerMicro.Start();  

#endif
            }
        }

        #region OnTickMili


        private void OnTickMili(object sender, EventArgs e)
        {

            //lock (_timer)
            //{
            //    using (oscilloscopeChart.SuspendUpdates())
            //    {
            //        if (ParserRayonM1.GetInstanceofParser.FifoplotList.IsEmpty == false)
            //        {
            //            for (int i = 0; i < BufferSize; i++)
            //            {
            //                dtx = (dtx + 0.0001);
            //                ParserRayonM1.GetInstanceofParser.FifoplotList.TryDequeue(out yBuffer[i]);
            //                xBuffer[i] = dtx;
            //            }

            //            Ch1Series.Append(xBuffer, yBuffer);

            //        }
            //        else
            //        {
            //           // Ch1Series.ResumeUpdates(osc);
            //            Ch1Series.Append(dtx, 0);
            //           // dtx = dtx + 0.0001;
            //        }

            //    }
            //}

            lock (_timer)
            {
                using (oscilloscopeChart.SuspendUpdates())
                {
                    if (ParserRayonM1.GetInstanceofParser.FifoplotList.IsEmpty == false)
                    {
                        int BufferSizeTemp = ParserRayonM1.GetInstanceofParser.FifoplotList.Count;
                        float[] yBufferTemp = new float[BufferSizeTemp];
                        double[] xBufferTemp = new double[BufferSizeTemp];

                        for (int i = 0; i < BufferSizeTemp; i++)
                        {
                            dtx = (dtx + 0.0001);
                            ParserRayonM1.GetInstanceofParser.FifoplotList.TryDequeue(out yBufferTemp[i]);
                            xBufferTemp[i] = dtx;
                        }

                        Ch1Series.Append(xBufferTemp, yBufferTemp);

                    }
                    else
                    {
                        // Ch1Series.ResumeUpdates(osc);
                   //     Ch1Series.Append(dtx, 0);
                        // dtx = dtx + 0.0001;
                    }

                }
            }
        }

        #endregion

        #region OnTickMicro


        private void OnTickMicro(object sender, MicroTimerEventArgs timerEventArgs)
        {

            //   try
            // {
            lock (_timerMicro)
            {
                using(oscilloscopeChart.SuspendUpdates())
                {
                 
                    if ((ParserRayonM1.GetInstanceofParser.FifoplotList.IsEmpty == false))
                    {
                     
                           ParserRayonM1.GetInstanceofParser.FifoplotList.TryDequeue(out y);
                           Ch1Series.Append(dtx, y);
                           dtx = (dtx + 0.0001); 

                        
                    }
                    else
                    {
                      
                    }
                }
            //
     
            }


        }


        #endregion

       


    }
}
