using System;
using System.Diagnostics;
using Abt.Controls.SciChart.Example.Data;
using SuperButton.Data;
using SuperButton.ViewModels;
using SuperButton.Helpers;

namespace SuperButton.Models.DriverBlock
{
    public class Rs232InterfaceEventArgs : EventArgs
    {
        public readonly PacketFields PacketRx;
        public readonly int ParseLength;
        public readonly byte[] InputChank;
        public readonly string ConnecteButtonLabel;

       // public readonly DoubleSeries Datasource1;
        public byte[] DataChunk { get; private set; }
        public readonly byte SMagicFirst;
        public readonly byte SMagicSecond;
        public readonly byte PMagicFirst;
        public readonly byte PMagicSecond;
        public readonly UInt16 PacketLength;



        public Rs232InterfaceEventArgs(byte[] dataChunk, byte smagicFirst, byte smagicSecond, byte pmagicFirst, byte pmagicSecond, UInt16 packetLength)
        {

            DataChunk = dataChunk;
            SMagicFirst = smagicFirst;
            SMagicSecond = smagicSecond;
            PacketLength = packetLength;
            PMagicFirst = pmagicFirst;
            PMagicSecond = pmagicSecond;
        }

        
        public Rs232InterfaceEventArgs(byte[] dataChunk)
        {
            //foreach(var b in dataChunk)
            //    RefreshManger.GetInstance.arr.Add(b);
            LeftPanelViewModel.GetInstance.LedStatusRx = RoundBoolLed.PASSED;
            DataChunk = dataChunk;      //Receive packet
        }

        public Rs232InterfaceEventArgs(string connecteButtonLabel)
        {
            ConnecteButtonLabel = connecteButtonLabel;
        }

   


        public Rs232InterfaceEventArgs(PacketFields packetRx)
        {
            LeftPanelViewModel.GetInstance.LedStatusTx = RoundBoolLed.PASSED;
            PacketRx = packetRx;     // Send Packet
            Debug.WriteLine(PacketRx.IsSet);
        }


     

        //public Rs232InterfaceEventArgs(DoubleSeries xyChannelOnep)
        //{
        //    Datasource1 = xyChannelOnep;
        //}


        public Rs232InterfaceEventArgs(int numberofbytes,byte[] inputBytes)
        {
            ParseLength = numberofbytes;
            InputChank = inputBytes;
        }

    }
}