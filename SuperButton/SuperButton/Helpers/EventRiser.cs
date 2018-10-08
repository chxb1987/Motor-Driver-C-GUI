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

        static readonly EventRiser _instance = new EventRiser();
        public static EventRiser Instance
        {
            get
            {
                return _instance;
            }

        }

        public void RiseEevent(string msg)
        {

            LoggerEvent(null, new CustomEventArgs() { Msg = msg });

        }
    }

    public class CustomEventArgs : EventArgs
    {
        public string Msg { get; set; }
    }
}
