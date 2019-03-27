using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//todo add interpase
namespace SuperButton.Models.DriverBlock
{
    abstract class ConnectionBase
    {
        public static readonly int[] BaudRates = new int[]
        {
            4800, 9600, 38400, 57600, 115200, 230400, 460800, 921600
        };



        internal class ComDevice
        {
            private int _databits = 8;
            private StopBits _stopbits = (StopBits)1;
            private int _baudrate;
            private string _portname;


            public int Baudrate
            {
                get { return _baudrate; }
                set { _baudrate = value; }
            }
            public int DataBits
            {
                get { return _databits; }
                set { _databits = value; }
            }

            public StopBits StopBits
            {
                get { return _stopbits; }
                set { _stopbits = value; }
            }
            public string Portname
            {
                get { return _portname; }
                set { _portname = value; }
            }
        }

        public abstract void AutoConnect(); //Virtual bstract Base function
        public abstract void Disconnect(); //Virtual bstract Base function
        //send 
        //recieve
        //Manual connect

    }
}
