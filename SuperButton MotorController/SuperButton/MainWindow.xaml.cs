using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SuperButton.ViewModels;
using SuperButton.Views;
using System.Diagnostics;

namespace SuperButton
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow 
	{
		public MainWindow()
		{
			this.InitializeComponent();
            this.DataContext = new MainViewModel();

			// Insert code required on object creation below this point.
		}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ParametarsWindow.WindowsOpen == true)
                LeftPanelViewModel.GetInstance.Close_parmeterWindow();
            App.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }
    }
}