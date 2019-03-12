using System;
using Abt.Controls.SciChart.Example.Data;
using SuperButton.Data;

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
            DataChunk = dataChunk;      //Receive packet
        }

        public Rs232InterfaceEventArgs(string connecteButtonLabel)
        {
            ConnecteButtonLabel = connecteButtonLabel;
        }

   


        public Rs232InterfaceEventArgs(PacketFields packetRx)
        {
            PacketRx = packetRx;     // Send Packet
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