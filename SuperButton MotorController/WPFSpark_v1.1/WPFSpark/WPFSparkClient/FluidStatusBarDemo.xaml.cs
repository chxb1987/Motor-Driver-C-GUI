using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using WPFSpark;
using System.Threading;

namespace WPFSparkClient
{
    /// <summary>
    /// Interaction logic for FluidStatusBarDemo.xaml
    /// </summary>
    public partial class FluidStatusBarDemo : Window
    {
        int counter = 0;
        bool isBGWorking = false;
        BackgroundWorker bgWorker;

        public FluidStatusBarDemo()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += new DoWorkEventHandler(DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnWorkCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(OnProgress);
        }

        void DoWork(object sender, DoWorkEventArgs e)
        {
            if (bgWorker.CancellationPending)
                return;

            StatusMessage msg = new StatusMessage();
            msg.Message = "Verifying Code!";
            msg.IsAnimated = true ;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(750);

            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Verifying : 10%";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(300);

            for (int i = 1; i < 10; i++)
            {
                if (bgWorker.CancellationPending)
                    return;

                msg.Message = String.Format("Verifying : {0}%", (i + 1) * 10);
                msg.IsAnimated = false;
                bgWorker.ReportProgress(0, msg);
                Thread.Sleep(300);
            }

            Thread.Sleep(750);

            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Compiling Code!";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(500);
            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Compiling : 10%";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(300);

            for (int i = 1; i < 10; i++)
            {
                if (bgWorker.CancellationPending)
                    return;

                msg.Message = String.Format("Compiling : {0}%", (i + 1) * 10);
                msg.IsAnimated = false;
                bgWorker.ReportProgress(0, msg);
                Thread.Sleep(300);
            }

            Thread.Sleep(750);

            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Linking!";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(750);
            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Linking : 10%";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(300);

            for (int i = 1; i < 10; i++)
            {
                if (bgWorker.CancellationPending)
                    return;

                msg.Message = String.Format("Linking : {0}%", (i + 1) * 10);
                msg.IsAnimated = false;
                bgWorker.ReportProgress(0, msg);
                Thread.Sleep(300);
            }

            Thread.Sleep(500);

            if (bgWorker.CancellationPending)
                return;
            
            msg.Message = "Build Completed!";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);
        }

        void OnProgress(object sender, ProgressChangedEventArgs e)
        {
            StatusMessage msg = e.UserState as StatusMessage;
            if (msg != null)
            {
                customStatusBar.SetStatus(msg.Message, msg.IsAnimated);
            }
        }

        void OnWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isBGWorking = false;
            StartBtn.IsEnabled = true;
            DirectionCB.IsEnabled = true;
            StopBtn.IsEnabled = false;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            counter++;

            if (counter % 5 == 0)
            {
                customStatusBar.SetStatus(string.Format("{0} D by 5", counter), true);
            }
            else if (counter % 3 == 0)
            {
                customStatusBar.SetStatus(string.Format("{0} D by 3", counter), true);
            }
            else
            {
                customStatusBar.SetStatus(String.Format("Counter = {0}", counter), false);
            }

        }

        private void DirectionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (DirectionCB.SelectedIndex)
            {
                case 1: // Right
                    customStatusBar.FadeOutDirection = StatusDirection.Right;
                    customStatusBar.FadeOutDistance = 500;
                    customStatusBar.FadeOutDuration = new Duration(TimeSpan.FromSeconds(1));
                    customStatusBar.MoveDuration = new Duration(TimeSpan.FromSeconds(0.5));
                    break;
                case 2: // Up
                    customStatusBar.FadeOutDirection = StatusDirection.Up;
                    customStatusBar.FadeOutDistance = 50;
                    customStatusBar.FadeOutDuration = new Duration(TimeSpan.FromSeconds(0.75));
                    customStatusBar.MoveDuration = new Duration(TimeSpan.FromSeconds(0.35));
                    break;
                case 3: // Down
                    customStatusBar.FadeOutDirection = StatusDirection.Down;
                    customStatusBar.FadeOutDistance = 50;
                    customStatusBar.FadeOutDuration = new Duration(TimeSpan.FromSeconds(0.75));
                    customStatusBar.MoveDuration = new Duration(TimeSpan.FromSeconds(0.35));
                    break;
                case 0: // Left
                default:
                    customStatusBar.FadeOutDirection = StatusDirection.Left;
                    customStatusBar.FadeOutDistance = 500;
                    customStatusBar.FadeOutDuration = new Duration(TimeSpan.FromSeconds(1));
                    customStatusBar.MoveDuration = new Duration(TimeSpan.FromSeconds(0.5));
                    break;
            }
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = false;
            DirectionCB.IsEnabled = false;

            bgWorker.RunWorkerAsync();
            isBGWorking = true;

            StopBtn.IsEnabled = true;
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            StopBtn.IsEnabled = false;

            if (isBGWorking)
            {
                bgWorker.CancelAsync();
            }
        }
    }
}
