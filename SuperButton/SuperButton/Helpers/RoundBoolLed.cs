using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using SuperButton.Annotations;
using SuperButton.Common;

namespace SuperButton.Helpers
{
    /// <summary>
    /// Represents any of 5 states: Iddle, Pass, Fail, Disabled or Running.
    /// Simply set CurrStatus to any constant in UC_Lib.Utils
    /// </summary>
    public class RoundBoolLed : Grid, INotifyPropertyChanged
    {

        #region CONSTS
        public const int IDLE = 0;
        public const int PASSED = 1;
        public const int FAILED = 2;
        public const int DISABLED = 3;
        public const int RUNNING = 4;
        #endregion
        #region FIELDS
        private Grid disableLineGrid;
        private Grid innerGrid;

        private Path disabledLine1;
        private Path disabledLine2;

        protected static RadialGradientBrush _rg_iddle = makeRadialGradient(Color.FromArgb(0xFF, 0x4E, 0x64, 0x4A), Color.FromArgb(0xFF, 0x04, 0x17, 0x01), Color.FromArgb(0x00, 0xf0, 0xf0, 0xf0));
        protected RadialGradientBrush _rg_pass = makeRadialGradient(Color.FromArgb(0xFF, 0xcc, 0xff, 0xcc), Color.FromArgb(0xFF, 0x00, 0xff, 0x00), Color.FromArgb(0x00, 0xf0, 0xf0, 0xf0));
        protected RadialGradientBrush _rg_fail = makeRadialGradient(Color.FromArgb(0xFF, 0xFF, 0xCC, 0xCC), Color.FromArgb(0xFF, 0xFF, 0x00, 0x00), Color.FromArgb(0x00, 0xf0, 0xf0, 0xf0));
        protected RadialGradientBrush _rg_disabled = makeRadialGradient(Color.FromArgb(0xFF, 0x38, 0x38, 0x38), Color.FromArgb(0xFF, 0x00, 0x00, 0x00), Color.FromArgb(0x00, 0xf0, 0xf0, 0xf0));
        protected RadialGradientBrush _rg_wait = makeRadialGradient(Color.FromArgb(0xFF, 0xF9, 0xF9, 0xBF), Color.FromArgb(0xFF, 0xEA, 0xF5, 0x06), Color.FromArgb(0x00, 0xf0, 0xf0, 0xf0));

        #endregion

        #region PROPERTIES
        protected RadialGradientBrush Rg_wait
        {
            get { return _rg_wait; }
            set { _rg_wait = value; }
        }
        #endregion PROPERTIES

        #region DEPENDENCY_PROPERTIES

        private BackgroundWorker _worker;


        public int CurrStatus
        {
            get { return (int)GetValue(CurrStatusProperty); }
            set { SetValue(CurrStatusProperty, value); }
        }

        public static readonly DependencyProperty CurrStatusProperty =
           DependencyProperty.Register("CurrStatus", typeof(int), typeof(RoundBoolLed), new PropertyMetadata(CurrStatusChanged));

        private static void CurrStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RoundBoolLed)d).setStatus();
        }

        private RadialGradientBrush myBackgroundBrush
        {
            get { return (RadialGradientBrush)GetValue(myBackgroundBrushProperty); }
            set { SetValue(myBackgroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for myBackgroundBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty myBackgroundBrushProperty =
            DependencyProperty.Register("myBackgroundBrush", typeof(RadialGradientBrush), typeof(RoundBoolLed), new PropertyMetadata(_rg_iddle));



        // Using a DependencyProperty as the backing store for CurrStatus.  This enables animation, styling, binding, etc...

        #endregion

        #region CTOR

        public RoundBoolLed()
        {
            initDisabledLine(ref disabledLine1, new Point(0.3, 0.3), new Point(0.7, 0.7));
            initDisabledLine(ref disabledLine2, new Point(0.3, 0.7), new Point(0.7, 0.3));
            initDisableLineGrid();
            initInnerGrid();
            Children.Add(innerGrid);
            Children.Add(disableLineGrid);
            Loaded += (a, e) => { setStatus(); };
        }
        #endregion

        #region INIT

        private void initInnerGrid()
        {
            innerGrid = new Grid();
            Binding backgroundBinding = new Binding("myBackgroundBrush") { Source = this };
            BindingOperations.SetBinding(innerGrid, BackgroundProperty, backgroundBinding);
            innerGrid.VerticalAlignment = VerticalAlignment.Center;
            innerGrid.HorizontalAlignment = HorizontalAlignment.Center;
        }


        private static RadialGradientBrush makeRadialGradient(Color stop00, Color stop10, Color stop11)
        {
            GradientStopCollection collection = new GradientStopCollection(3);
            collection.Add(new GradientStop(stop00, 0));
            collection.Add(new GradientStop(stop10, 1));
            collection.Add(new GradientStop(stop11, 1));
            RadialGradientBrush result = new RadialGradientBrush(collection);
            return result;
        }

        private void initDisableLineGrid()
        {
            disableLineGrid = new Grid();
            disableLineGrid.Children.Add(disabledLine1);
            disableLineGrid.Children.Add(disabledLine2);
            disableLineGrid.VerticalAlignment = VerticalAlignment.Center;
            disableLineGrid.HorizontalAlignment = HorizontalAlignment.Center;
            disableLineGrid.Visibility = Visibility.Hidden;
        }

        private void initDisabledLine(ref Path dl, Point startPoint, Point endPoint)
        {
            dl = new Path();
            dl.Stroke = Brushes.White;
            dl.StrokeThickness = 1;
            dl.Stretch = Stretch.Fill;
            dl.Data = new LineGeometry(startPoint, endPoint);
        }
        #endregion

        #region EVENTS

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            //Side = Math.Min(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height);
            innerGrid.Width =
                innerGrid.Height =
                    disableLineGrid.Width =
                        disableLineGrid.Height = Math.Min(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height);
        }


        #endregion

        #region CHANGE_STATE
        public void setStatus()
        {

            if (_worker != null && _worker.IsBusy && CurrStatus != Consts.BOOL_RUNNING) _worker.CancelAsync();
            switch (CurrStatus)
            {
                case Consts.BOOL_RUNNING:
                    if (_worker != null && _worker.IsBusy)
                        break;
                    myBackgroundBrush = Rg_wait;
                    _worker = new BackgroundWorker();
                    _worker.DoWork += worker_DoWork;
                    _worker.ProgressChanged += WorkerProgressChanged;
                    _worker.WorkerReportsProgress = true;
                    _worker.WorkerSupportsCancellation = true;
                    _worker.RunWorkerAsync();
                    break;
                case Consts.BOOL_FAILED:
                    myBackgroundBrush = _rg_fail;
                    break;
                case Consts.BOOL_PASSED:
                    myBackgroundBrush = _rg_pass;

                    break;
                case Consts.BOOL_DISABLED:
                    myBackgroundBrush = _rg_disabled;

                    break;
                case Consts.BOOL_IDLE:
                    myBackgroundBrush = _rg_iddle;

                    break;
                default:
                    break;
            }
            disableLineGrid.Visibility = CurrStatus == Consts.BOOL_DISABLED
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(350);
                if (_worker.CancellationPending)
                    break;
                _worker.ReportProgress(0, null);
            }
        }

        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (CurrStatus == Consts.BOOL_RUNNING)
                myBackgroundBrush = (Equals(myBackgroundBrush, Rg_wait))
                                      ? _rg_iddle
                                      : Rg_wait;
        }

        public void Reset()
        {
            if (_worker != null && _worker.IsBusy) _worker.CancelAsync();
            myBackgroundBrush = _rg_iddle;
            disableLineGrid.Visibility = Visibility.Hidden;
        }
        #endregion

        #region PROPERTY_CHANGED
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) { handler(this, new PropertyChangedEventArgs(propertyName)); }
        }

        #endregion

    }
}
