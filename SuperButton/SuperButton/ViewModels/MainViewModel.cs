using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Rendering.Common;
using Abt.Controls.SciChart.Visuals;
using SuperButton.Models.DriverBlock;
using SuperButton.Models.ParserBlock;
using SuperButton.Views;
using UserControlLibrary;
using UserControlLibrary.ViewModels;
using BaseViewModel = SuperButton.Common.BaseViewModel;
using SuperButton.Helpers;
using SharpDX;
//using SharpDX.Design;
using System.Windows.Media;

namespace SuperButton.ViewModels
{
    public class MainViewModel : BaseViewModel
	{
        OscilloscopeViewModel oscilloscopeViewModel=new OscilloscopeViewModel();
        public OscilloscopeViewModel OscilloscopeViewModel { get { return oscilloscopeViewModel; }
            set { ; } 
        }



        //Friday 08.01
        public ActionCommand MainWindowResized { get { return new ActionCommand(mainWindowResized); } }

        private double maxHeight = 240;
        void mainWindowResized()
        {
            MaxHeight  = (float)Application.Current.MainWindow.ActualHeight-101;
        }
      

        #region Actions
        public ActionCommand SetAutoConnectActionCommandCommand {
            get { return new ActionCommand(AutoConnectCommand); }
        }
        
        #endregion

        public double MaxHeight {
            //get {return maxHeight;}
            set
            {
                
                    maxHeight = value;
                    OnPropertyChanged("MaxHeight");           
            }
        }

        //Data content binding between views of panels within main window
        //and their view models, write binding in XAMLs also

        private RightPanelViewModel rightPanelViewModel = new RightPanelViewModel();
        public RightPanelViewModel RPcontent
        {
            get { return rightPanelViewModel; }
            set {  }
        }

        private LeftPanelViewModel leftPanelViewModel = new LeftPanelViewModel();
        public LeftPanelViewModel LPcontent
        {
            get { return leftPanelViewModel; }
            set { }
        }

        #region Debug

        //private Example _selectedExample;
        //private  string connectButtonContent;
        //private UserControlLibrary.ViewModels.UserControl1ViewModel Test;
        //private UserControlLibrary.ViewModels.UserControl1ViewModel Test2;
        //private UserControl1 b;
        //private UserControl1 b2;
        //public static MainViewModel mainViewModel;
        //public float LeftGrid_Width;
        //public float Left_Grid_Width
        //{
        //    get { return LeftGrid_Width;}
        //    set { LeftGrid_Width = value; }
        //}
        //public   UserControl1 TestBinding     
        //{ get { return b; }      
        //  set{} }
        //public UserControl1 TestBinding2
        //{
        //    get { return b2; }
        //    set { }
        //}
        //public ActionCommand CloseLeftGrid { get { return new ActionCommand(() => closeLeftGrid()); } }
        //public void closeLeftGrid()
        //{
        //    LeftGrid:
        //    Left_Grid_Width = 10;
        //    Left_Grid_Width = 10;
        //}
        #endregion
        public MainViewModel()
		{
            //System.Windows.Media.Color color;

            leftPanelViewModel.ConnectButtonContent = "Connect";
            //leftPanelViewModel.ConnectButtonBackground = ColorConverter;
            leftPanelViewModel.ComToolTipText = "Pls Choose CoM";
            Rs232Interface.GetInstance.Driver2Mainmodel += SincronizationPos;

            leftPanelViewModel.SendButtonContent = "Send";
            leftPanelViewModel.StopButtonContent = "Force Stop";
            /*Left Panel*/

            
            //  KUKU();
            //  rightPanel.DataContext = rightPanelViewModel;

            // rightPanel = new RightPanel();

            //  rightPanel.DataContext = rightPanelViewModel;

            //rightPanelViewModel=new RightPanelViewModel();
            //rightPanel.DataContext = rightPanelViewModel;
            //rightPanelViewModel.ConnetButtonContent = "Disconnect";

            //Test = new UserControl1ViewModel();
            //Test.Label = "kjlljkljkljkljkljkljkljk";

            //Test2 = new UserControl1ViewModel();
            

            //b2 = new UserControl1();
            //b2.DataContext = Test2;

            // b=new UserControl1();
            //b.DataContext = Test;

            //  LeftGrid_Width = 500;


            // ConnectButtonContent="Connect";
            //   mainViewModel = this;

            //Create packetizer


            // Insert code required on object creation below this point.
		}


        private void  SincronizationPos(object sender, Rs232InterfaceEventArgs e)
        {
            leftPanelViewModel.ConnectButtonContent = e.ConnecteButtonLabel;
            leftPanelViewModel.ComToolTipText = "Allready Connected";
        }

        public void AutoConnectCommand()
        {
            EventRiser.Instance.RiseEevent(string.Format($"You Pressed : button"));
            Rs232Interface comRs232Interface = Rs232Interface.GetInstance;
            Task task = new Task(new Action(comRs232Interface.AutoConnect));
            task.Start();
        }
		
		private string viewModelProperty = "Runtime Property Value";
		/// <summary>
		/// Sample ViewModel property; this property is used in the view to display its value using a Binding.
		/// </summary>
		/// <returns></returns>
		public string ViewModelProperty
		{ 
			get
			{
				return this.viewModelProperty;
			}
			set
			{
				this.viewModelProperty = value;
				this.NotifyPropertyChanged("ViewModelProperty");
			}
		}
		
		/// <summary>
		/// Sample ViewModel method; this method is invoked by a Behavior that is associated with it in the View.
		/// </summary>
		public void ViewModelMethod()
		{ 
			if(!this.ViewModelProperty.EndsWith("Updated Value", StringComparison.Ordinal)) 
			{ 
				this.ViewModelProperty = this.ViewModelProperty + " - Updated Value";
			}
		}

        //DirecX10



  

	}
}