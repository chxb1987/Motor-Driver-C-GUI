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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperButton
{
	/// <summary>
	/// Interaction logic for BottomPanel.xaml
	/// </summary>
	public partial class BottomPanel : UserControl
	{
		public BottomPanel()
		{
			this.InitializeComponent();

           GroupedExamplesViewModel GroupedStaus=new GroupedExamplesViewModel();         
		}

	  
	}
}