using System;
using System.Windows;
using System.Windows.Controls;
using SuperButton.ViewModels;

namespace SuperButton.Views
{


	/// <summary>
	/// Interaction logic for EnumUC.xaml
	/// </summary>
	public partial class EnumUC : UserControl
	{
        #region Label DP

        public String Label
        {
            get { return (String)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string),
              typeof(EnumUC), new PropertyMetadata(""));

        #endregion

        #region Label DP

        /// <summary>
        /// Gets or sets the Value which is being displayed
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object),
              typeof(EnumUC), new PropertyMetadata(null));

        #endregion







        public String SelectedValue
        {
            get { return (String)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(string),
              typeof(EnumUC), new PropertyMetadata(null));

		public EnumUC()
		{
			this.InitializeComponent();
            this.DataContext = new EnumUCModel();
			
			// Insert code required on object creation below this point.
		}
	}
}