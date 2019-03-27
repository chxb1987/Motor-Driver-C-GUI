using System;
using System.Windows;
using System.Timers;

namespace WPFSparkClient
{
    /// <summary>
    /// Interaction logic for SprocketControlDemo.xaml
    /// </summary>
    public partial class SprocketControlDemo : Window
    {
        Timer timer1 = new Timer(70);
        Timer timer2 = new Timer(70);

        public SprocketControlDemo()
        {
            InitializeComponent();

            timer1.Elapsed += new ElapsedEventHandler(timer1_Elapsed);
            timer2.Elapsed += new ElapsedEventHandler(timer2_Elapsed);
        }

        void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    sprocketControl3.Progress++;

                    if (sprocketControl3.Progress >= 100)
                    {
                        timer1.Enabled = false;
                        button1.IsEnabled = true;
                    }
                }));
        }

        void timer2_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                sprocketControl4.Progress++;

                if (sprocketControl4.Progress >= 100)
                {
                    timer2.Enabled = false;
                    button2.IsEnabled = true;
                }
            }));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    button1.IsEnabled = false;
                    sprocketControl3.Progress = 0;
                    textBlock1.Visibility = System.Windows.Visibility.Visible;
                    timer1.Enabled = true;
                }));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                button2.IsEnabled = false;
                sprocketControl4.Progress = 0;
                textBlock2.Visibility = System.Windows.Visibility.Visible;
                timer2.Enabled = true;
            }));
        }
    }
}
