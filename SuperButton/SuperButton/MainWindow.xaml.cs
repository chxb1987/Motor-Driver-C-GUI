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

        private void LeftPanelView_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}