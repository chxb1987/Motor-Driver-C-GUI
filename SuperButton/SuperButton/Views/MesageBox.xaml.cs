using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace SuperButton.Views
{
    /// <summary>
    /// Interaction logic for MesageBox.xaml
    /// </summary>
    public partial class MesageBox : Window
    {
        #region Label DP

        //public String MessageText
        //{
        //    get { return (String)GetValue(MessageTextProperty); }
        //    set { SetValue(MessageTextProperty, value); }
        //}

        ///// <summary>
        ///// Identified the Label dependency property
        ///// </summary>
        //public static readonly DependencyProperty MessageTextProperty =
        //    DependencyProperty.Register("MessageText", typeof(string),
        //      typeof(MesageBox), new PropertyMetadata(""));

        #endregion

        // Prep stuff needed to remove close button on window
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public MesageBox(string msg)
        {
            Loaded += ToolWindow_Loaded;
            InitializeComponent();
            this.MessageBlock.Text = msg;
            
        }

        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
    }
}
