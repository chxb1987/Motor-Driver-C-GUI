using System.Windows;
using System.Windows.Media;
using WPFSpark;
using System.Collections.ObjectModel;

namespace WPFSparkClient
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : SparkWindow
    {
        public ClientWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
                {
                    ObservableCollection<UIElement> source = new ObservableCollection<UIElement>();

                    FontFamily font = new FontFamily("Segoe UI");
                    AppButton sprocketBtn = new AppButton() { Width = 130, Height = 130, FontFamily = font, FontSize = 18, FontWeight = FontWeights.Light, Foreground = Brushes.White, Content = "Sprocket Control" };
                    sprocketBtn.Click += new RoutedEventHandler(sprocketBtn_Click);
                    source.Add(sprocketBtn);
                    AppButton toggleSwitchBtn = new AppButton() { Width = 130, Height = 130, FontFamily = font, FontSize = 18, FontWeight = FontWeights.Light, Foreground = Brushes.White, Content = "ToggleSwitch Control" };
                    toggleSwitchBtn.Click += new RoutedEventHandler(toggleSwitchBtn_Click);
                    source.Add(toggleSwitchBtn);
                    AppButton fluidWrapPanelBtn = new AppButton() { Width = 130, Height = 130, FontFamily = font, FontSize = 18, FontWeight = FontWeights.Light, Foreground = Brushes.White, Content = "Fluid WrapPanel" };
                    fluidWrapPanelBtn.Click += new RoutedEventHandler(fluidWrapPanelBtn_Click);
                    source.Add(fluidWrapPanelBtn);
                    AppButton sparkWindowBtn = new AppButton() { Width = 130, Height = 130, FontFamily = font, FontSize = 18, FontWeight = FontWeights.Light, Foreground = Brushes.White, Content = "Spark Window" };
                    sparkWindowBtn.Click += new RoutedEventHandler(sparkWindowBtn_Click);
                    source.Add(sparkWindowBtn);
                    AppButton fluidPivotPanelBtn = new AppButton() { Width = 130, Height = 130, FontFamily = font, FontSize = 18, FontWeight = FontWeights.Light, Foreground = Brushes.White, Content = "Fluid PivotPanel" };
                    fluidPivotPanelBtn.Click += new RoutedEventHandler(fluidPivotPanelBtn_Click);
                    source.Add(fluidPivotPanelBtn);
                    AppButton fluidProgressBarBtn = new AppButton() { Width = 130, Height = 130, FontFamily = font, FontSize = 18, FontWeight = FontWeights.Light, Foreground = Brushes.White, Content = "Fluid ProgressBar" };
                    fluidProgressBarBtn.Click += new RoutedEventHandler(fluidProgressBarBtn_Click);
                    source.Add(fluidProgressBarBtn);
                    AppButton fluidStatusBarBtn = new AppButton() { Width = 130, Height = 130, FontFamily = font, FontSize = 18, FontWeight = FontWeights.Light, Foreground = Brushes.White, Content = "Fluid StatusBar" };
                    fluidStatusBarBtn.Click += new RoutedEventHandler(fluidStatusBarBtn_Click);
                    source.Add(fluidStatusBarBtn);

                    fluidWrapPanel.ItemsSource = source;
                };

        }

        private void sprocketBtn_Click(object sender, RoutedEventArgs e)
        {
            SprocketControlDemo scDemo = new SprocketControlDemo();

            scDemo.Owner = this;
            scDemo.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            scDemo.Show();
        }

        private void toggleSwitchBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleSwitchDemo tsDemo = new ToggleSwitchDemo();

            tsDemo.Owner = this;
            tsDemo.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tsDemo.Show();
        }

        private void fluidWrapPanelBtn_Click(object sender, RoutedEventArgs e)
        {
            FluidWrapPanelDemo fwpDemo = new FluidWrapPanelDemo();

            fwpDemo.Owner = this;
            fwpDemo.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            fwpDemo.Show();
        }

        private void sparkWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            SparkWindowDemo swDemo = new SparkWindowDemo();

            swDemo.Owner = this;
            swDemo.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            swDemo.Show();
        }

        private void fluidProgressBarBtn_Click(object sender, RoutedEventArgs e)
        {
            FluidProgressBarDemo spDemo = new FluidProgressBarDemo();

            spDemo.Owner = this;
            spDemo.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            spDemo.Show();
        }

        private void fluidPivotPanelBtn_Click(object sender, RoutedEventArgs e)
        {
            FluidPivotPanelDemo ppDemo = new FluidPivotPanelDemo();

            ppDemo.Owner = this;
            ppDemo.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ppDemo.Show();
        }

        private void fluidStatusBarBtn_Click(object sender, RoutedEventArgs e)
        {
            FluidStatusBarDemo fsDemo = new FluidStatusBarDemo();

            fsDemo.Owner = this;
            fsDemo.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            fsDemo.Show();
        }

        protected override void OnAbout(object sender, RoutedEventArgs e)
        {
            MsgBox mbox = new MsgBox();
            mbox.Title = "About";
            mbox.Owner = this;
            mbox.ShowDialog();
            base.OnAbout(sender, e);
        }
    }
}
