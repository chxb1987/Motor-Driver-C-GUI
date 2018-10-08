using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SuperButton.Helpers
{
    public class RecLed : RoundBoolLed
    {
        public bool Recording
        {
            get { return (bool)GetValue(RecordingProperty); }
            set { SetValue(RecordingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Recording.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecordingProperty =
            DependencyProperty.Register("Recording", typeof(bool), typeof(RecLed), new FrameworkPropertyMetadata(false, recChanged));

        private static void recChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RecLed arg = (RecLed)d;
            bool newValue = (bool)e.NewValue;
            arg.CurrStatus = newValue ? RUNNING : IDLE;
        }

        public RecLed() { Rg_wait = _rg_fail; }
    }
}
