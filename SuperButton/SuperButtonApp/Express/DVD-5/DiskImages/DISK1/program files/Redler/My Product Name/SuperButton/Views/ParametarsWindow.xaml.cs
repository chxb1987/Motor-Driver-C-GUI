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



namespace SuperButton.Views
{
    /// <summary>
    /// Interaction logic for ParametarsWindow.xaml
    /// </summary>
    public partial class ParametarsWindow : Window
    {
        public ParametarsWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LeftPanelViewModel.GetInstance.Close_parmterWindow();
        }
    }
}
