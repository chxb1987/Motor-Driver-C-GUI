using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SuperButton.ViewModels;
using SuperButton.Models.DriverBlock;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Windows.Interop;

namespace SuperButton.Views
{
    /// <summary>
    /// Interaction logic for ParametarsWindow.xaml
    /// </summary>
    public partial class ParametarsWindow : Window
    {
        public static int ParametersWindowTabSelected = -1;
        public static bool WindowsOpen = false;

        private static readonly object Synlock = new object();
        private static ParametarsWindow _instance;
        public static ParametarsWindow GetInstance
        {
            get
            {
                lock(Synlock)
                {
                    if(_instance != null)
                        return _instance;
                    _instance = new ParametarsWindow();
                    var vaultHwnd = Process.GetCurrentProcess().MainWindowHandle; //Vault window handle
                    new WindowInteropHelper(_instance) { Owner = vaultHwnd };
                    return _instance;
                }
            }
        }

        public ParametarsWindow()
        {
            InitializeComponent();
            ParametarsWindow.WindowsOpen = true;
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //LeftPanelViewModel.flag = false;
            WindowsOpen = false;
            ParametersWindowTabSelected = -1;
            _instance = null;
        }

        ~ParametarsWindow() {
        }

        private void TabSelected(object sender, SelectionChangedEventArgs e)
        {
            ParametersWindowTabSelected = ((System.Windows.Controls.Primitives.Selector)sender).SelectedIndex;
        }

        private void ItemsControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        //private void HandleChecked(object sender, RoutedEventArgs e)
        //{
        //    ToggleButton toggle = sender as ToggleButton;
        //    toggle.Background = new SolidColorBrush(Colors.Orange);
        //}
    }
}
