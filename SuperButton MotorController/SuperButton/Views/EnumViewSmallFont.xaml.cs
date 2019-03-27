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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperButton.Views
{
    /// <summary>
    /// Interaction logic for EnumView.xaml
    /// </summary>
    public partial class EnumViewSmallFont : UserControl
    {
        public EnumViewSmallFont()
        {
            InitializeComponent();
        }
        void cmbItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("OK");
        }
    }
}
