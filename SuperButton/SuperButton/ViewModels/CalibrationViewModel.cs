using Abt.Controls.SciChart;
using SuperButton.Models.DriverBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperButton.ViewModels
{
    internal class CalibrationViewModel : ViewModelBase
    {
        private static readonly object Synlock = new object();
        private static CalibrationViewModel _instance;


        public ActionCommand OffsetCal { get { return new ActionCommand(OffsetCalCmd); } }
        public ActionCommand PiCurrentCal { get { return new ActionCommand(PiCurrentCalCmd); } }
        public ActionCommand PiSpeedCal { get { return new ActionCommand(PiSpeedCalCmd); } }
        public ActionCommand HallMapCal { get { return new ActionCommand(HallMapCalCmd); } }
        public ActionCommand EncoderCal { get { return new ActionCommand(EncoderCalCmd); } }
        public ActionCommand XCal { get { return new ActionCommand(XCalCmd); } }
        public static CalibrationViewModel GetInstance
        {
            get
            {
                lock (Synlock)
                {
                    if (_instance != null) return _instance;
                    _instance = new CalibrationViewModel();
                    return _instance;
                }
            }
        }

        private CalibrationViewModel()
        {

        }
        private void OffsetCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "",
                ID = Convert.ToInt16(100),
                SubID = Convert.ToInt16(1),
                IsSet = false,
                IsFloat = false
            }
        );

        }
        private void PiCurrentCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "254",
                ID = Convert.ToInt16(284),
                SubID = Convert.ToInt16(6),
                IsSet = true,
                IsFloat = false
            }
        );
        }
        private void PiSpeedCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "",
                ID = Convert.ToInt16(100),
                SubID = Convert.ToInt16(1),
                IsSet = false,
                IsFloat = false
            }
        );

        }
        private void HallMapCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "254",
                ID = Convert.ToInt16(284),
                SubID = Convert.ToInt16(6),
                IsSet = true,
                IsFloat = false
            }
        );
        }
        private void EncoderCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "",
                ID = Convert.ToInt16(100),
                SubID = Convert.ToInt16(1),
                IsSet = false,
                IsFloat = false
            }
        );

        }
        private void XCalCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "254",
                ID = Convert.ToInt16(284),
                SubID = Convert.ToInt16(6),
                IsSet = true,
                IsFloat = false
            }
        );
        }
    }
}






