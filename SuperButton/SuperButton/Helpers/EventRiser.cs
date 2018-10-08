using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperButton.Helpers
{
    public sealed class EventRiser
    {
        public event EventHandler LoggerEvent;
        public event EventHandler LedEventTx;
        public event EventHandler LedEventRx;


        static readonly EventRiser _instance = new EventRiser();
        public static EventRiser Instance
        {
            get
            {
                if (_instance == null)
                    return new EventRiser();
                return _instance;
            }

        }

        public void RiseEevent(string msg)
        {

            LoggerEvent(null, new CustomEventArgs() { Msg = msg });

        }
        public void RiseEventLedTx(int led)
        {
            LedEventTx(null, new CustomEventArgs() { LedTx = led });
        }
        public void RiseEventLedRx(int led)
        {
            LedEventRx(null, new CustomEventArgs() { LedRx = led });
        }

    }

    public class CustomEventArgs : EventArgs
    {
        public string Msg { get; set; }
        public int LedTx { get; set; }
        public int LedRx { get; set; }

    }
}
