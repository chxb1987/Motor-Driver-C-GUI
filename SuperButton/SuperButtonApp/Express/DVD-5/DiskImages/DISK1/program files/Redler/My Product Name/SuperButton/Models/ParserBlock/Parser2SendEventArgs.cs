using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperButton.Data;

namespace SuperButton.Models.ParserBlock
{
    class Parser2SendEventArgs
    {

        public readonly byte[] BytesTosend;
        public readonly DoubleSeries Datasource;
        public readonly double X;
        public readonly double Y;
   

        public Parser2SendEventArgs(byte[] temp)
        {
           
            BytesTosend = temp;
        }

        public Parser2SendEventArgs(DoubleSeries datasource)
        {
            Datasource = new DoubleSeries();
            Datasource = datasource;
        }

        public Parser2SendEventArgs(double x,double y)
        {
            X = x;
            Y = y;
        }

        public Parser2SendEventArgs(double y)
        {
            Y = y;
        }

    }
}
