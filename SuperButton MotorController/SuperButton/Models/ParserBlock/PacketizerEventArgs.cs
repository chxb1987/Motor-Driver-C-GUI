using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperButton.Models.ParserBlock
{
    class PacketizerEventArgs
    {
        public byte[] DataChunk { get; private set; }
        public readonly byte[] MagicFirst;
        public readonly byte[] MagicSecond;
        public UInt16 PacketLength;



        public PacketizerEventArgs(byte[] dataChunk, byte[] magicFirst, byte[] magicSecond,UInt16 packetLength)
        {

            DataChunk = dataChunk;
            MagicFirst = magicFirst;
            MagicSecond = magicSecond;
            PacketLength = packetLength;

        }


    }
}
