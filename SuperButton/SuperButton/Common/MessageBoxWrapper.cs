using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SuperButton.Common
{
    class MessageBoxWrapper
    {
        public static bool IsOpen { get; set; }
        //MessageBoxImage Background = 1;

        // give all arguments you want to have for your MSGBox
        public static void Show(string messageBoxText, string caption)
        {
            IsOpen = true;
            //if (Application.Current.Dispatcher.CheckAccess())
            //{
            //    MessageBox.Show(Application.Current.MainWindow, messageBoxText, caption);
            //}
            //else
            //{
                System.Media.SystemSounds.Asterisk.Play();
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    SuperButton.Views.MesageBox message = new Views.MesageBox(messageBoxText);
                    message.ShowDialog();
                }));
            //}
            IsOpen = false;
        }
    }
}
