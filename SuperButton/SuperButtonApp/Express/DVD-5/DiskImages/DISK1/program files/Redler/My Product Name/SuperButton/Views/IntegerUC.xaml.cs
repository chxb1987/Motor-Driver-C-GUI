using System;
using System.Windows;
using System.Windows.Controls;
using SuperButton.ViewModels;

namespace SuperButton.Views
{
	/// <summary>
	/// Interaction logic for IntegerUC.xaml
	/// </summary>
	public partial class IntegerUC : UserControl
	{
        #region LabelName DP

        public String LabelIntName
        {
            get { return (String)GetValue(LabelIntNameProperty); }
            set { SetValue(LabelIntNameProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty LabelIntNameProperty =
            DependencyProperty.Register("LabelIntName", typeof(string),
              typeof(IntegerUC), new PropertyMetadata(""));

        #endregion

        #region LabelUnit DP

        /// <summary>
        /// Gets or sets the Value which is being displayed
        /// </summary>
        public String LabelIntUnit
        {
            get { return (String)GetValue(LabelIntUnitProperty); }
            set { SetValue(LabelIntUnitProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty LabelIntUnitProperty =
            DependencyProperty.Register("LabelIntUnit", typeof(string),
              typeof(IntegerUC), new PropertyMetadata(null));

        #endregion

        #region Text DP

        public String TextMessege
        {
            get { return (String)GetValue(TextMessegeProperty); }
            set { SetValue(TextMessegeProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty TextMessegeProperty =
            DependencyProperty.Register("TextMessege", typeof(string),
              typeof(IntegerUC), new PropertyMetadata(""));

        #endregion


		public IntegerUC()
		{
			this.InitializeComponent();
            this.DataContext = new IntegerUCModel();
			
			// Insert code required on object creation below this point.
		}
	}
}