using SuperButton.CommandsDB;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SuperButton.Models;
using Abt.Controls.SciChart;
using SuperButton.Models.DriverBlock;
using System;
using System.Threading;

namespace SuperButton.ViewModels
{
    public class BottomPanelViewModel : ViewModelBase
    {
        public BottomPanelViewModel()
        {

        }
        private ObservableCollection<object> _motionCommandList;
        private ObservableCollection<object> _motionStatusList;
        private ObservableCollection<object> _SGList;
        public ObservableCollection<object> MotionCommandList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["MotionCommand List"]; //Motion Limit
            }
            set
            {
                _motionCommandList = value;
                OnPropertyChanged();
            }


        }

        public ObservableCollection<object> MotionStatusList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["MotionStatus List"];
            }
            set
            {
                _motionStatusList = value;
                OnPropertyChanged();
            }


        }
        public ObservableCollection<object> ControlList
        {

            get
            {
                return Commands.GetInstance.EnumCommandsListbySubGroup["Control"];
            }


        }

        public ObservableCollection<object> MotorlList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["Motor"];
            }

        }
        public ObservableCollection<object> ProfilerModeList
        {
            get
            {
                return Commands.GetInstance.EnumCommandsListbySubGroup["Profiler Mode"];
            }
        }
        public ObservableCollection<object> SGTypeList
        {
            get
            {
                return Commands.GetInstance.EnumCommandsListbySubGroup["S.G.Type"];
            }
}
        public ObservableCollection<object> SGList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["S.G.List"];
            }
            set
            {
                _SGList = value;
                OnPropertyChanged();
            }


        }

        public ActionCommand StopMotion { get { return new ActionCommand(StopMotionCmd); } }
        private void StopMotionCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = 1,
                ID = Convert.ToInt16(1),
                SubID = Convert.ToInt16(1),
                IsSet = true,
                IsFloat = true
            });

        }
    }
}
