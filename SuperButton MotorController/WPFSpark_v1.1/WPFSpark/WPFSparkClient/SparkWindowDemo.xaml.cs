using System.Windows;
using WPFSpark;

namespace WPFSparkClient
{
    /// <summary>
    /// Interaction logic for SparkWindowDemo.xaml
    /// </summary>
    public partial class SparkWindowDemo : SparkWindow
    {
        public SparkWindowDemo()
        {
            InitializeComponent();
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
