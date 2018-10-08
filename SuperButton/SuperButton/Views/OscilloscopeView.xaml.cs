using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Abt.Controls.SciChart.Rendering.Common;
using Abt.Controls.SciChart.Rendering.HighSpeedRasterizer;
using Abt.Controls.SciChart.Visuals;
using Abt.Controls.SciChart3D.Context.D3D10;
using Application = System.Windows.Application;
using UserControl = System.Windows.Controls.UserControl;


namespace SuperButton.Views
{
    /// <summary>
    /// Interaction logic for OscilloscopeView.xaml
    /// </summary>
    public partial class OscilloscopeView : UserControl
    {
        public OscilloscopeView()
        {
            InitializeComponent();

            //Thread.Sleep(50);
            //this.OscilloscopeChart.RenderSurface.RecreateSurface();
            //Thread.Sleep(50);
            //this.OscilloscopeChart.RenderSurface.RecreateSurface();
            //Thread.Sleep(50);

           // this.oscilloscopeChart.RenderSurface.Style

            

            var type=this.OscilloscopeChart.RenderSurface;
        }

        private void FrameworkElement_OnInitialized(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
           
           var type = this.OscilloscopeChart.RenderSurface;
        }

   
    }
}


   