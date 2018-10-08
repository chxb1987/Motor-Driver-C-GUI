using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1.DriverBlock;
using SuperButton.Models.DriverBlock;

namespace WindowsFormsApplication1.DriverBlock
{

    internal class CanInterface:ConnectionBase
    {
        public override void AutoConnect()
        {
            return;//false;

        }

        public override void Disconnect()
        {
            return;//false;

        }
    }
}
