using Microsoft.Expression.Interactivity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperButton.CommandsDB;
using SuperButton.Models.DriverBlock;

namespace SuperButton.ViewModels
{
    internal class OperationViewModel : ViewModelBase
    {
        private static readonly object Synlock = new object();
        private static OperationViewModel _instance;
        private string _driverVersion;
        private string _motorDriver;

        public ActionCommand GetDriverVersion { get { return new ActionCommand(GetDriverVersionCmd); } }
        public ActionCommand GetMotorDriver { get { return new ActionCommand(GetMotorDriverCmd); } }
        public static OperationViewModel GetInstance
        {
            get
            {
                lock (Synlock)
                {
                    if (_instance != null) return _instance;
                    _instance = new OperationViewModel();
                    return _instance;
                }
            }
        }

        private  OperationViewModel()
        {
        }

        private void GetDriverVersionCmd()
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

        private void GetMotorDriverCmd()
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
        public string DriverVersion
        {
            get
            {
                return _driverVersion;
            }

            set
            {
                _driverVersion = value;
                OnPropertyChanged("DriverVersion"); //"DriverVersion"
            }
        }

        public string MotorDriver
        {
            get
            {
                return _motorDriver;
            }

            set
            {
                _motorDriver = value;
                RaisePropertyChanged("MotorDriver"); //"DriverVersion"
            }
        }
    }
}
