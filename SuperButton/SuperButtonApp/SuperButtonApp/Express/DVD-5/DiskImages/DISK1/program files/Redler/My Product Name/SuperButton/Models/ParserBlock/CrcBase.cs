using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperButton.Models.ParserBlock;

namespace SuperButton.Models.ParserBlock
{
    class CrcBase
    {

        static private ushort[] crctable = new ushort[]
        {
            0x0000, 0xCC01, 0xD801, 0x1400, 0xF001, 0x3C00, 0x2800, 0xE401,
            0xA001, 0x6C00, 0x7800, 0xB401, 0x5000, 0x9C01, 0x8801, 0x4400
        };

        static private ushort crc16(ushort crc, IEnumerable<byte> buffer,int offset)
        {

            foreach (var byt in buffer)
            {
                if (offset == 0)
                {
                    //  if (totalcnt > 0)
                   // {
                      //  totalcnt--;
                        // CRC the lower 4 bits
                        crc = (ushort) ((crc >> 4) ^ crctable[((crc ^ (byt & 0xF)) & 0xF)]);

                        // CRC the upper 4 bits
                        crc = (ushort) ((crc >> 4) ^ crctable[((crc ^ (byt >> 4)) & 0xF)]);
                   // }
                }

                // Move on to the next element
                else
                offset--;

            }
                
            return crc;
        }

        static  private ushort crc16_byte(ushort crc, byte _byte)
        {
            // CRC the lower 4 bits
            crc = (ushort) ((crc >> 4) ^ crctable[((crc ^ (_byte & 0xF)) & 0xF)]);

            // CRC the upper 4 bits
            crc = (ushort) ((crc >> 4) ^ crctable[((crc ^ (_byte >> 4)) & 0xF)]);


            // Return the cumulative CRC-16 value
            return crc;
        }

        public  int CheckFrameCrc(byte[] data)
        {
            return (crc16(0x0000, data, 0)); //,0));

        }

        static public ushort CalcHostFrameCrc(IEnumerable<byte> data,int offset)
        {
            return crc16(0x0000, data, offset);// totalcnt);
        }
    }


}

