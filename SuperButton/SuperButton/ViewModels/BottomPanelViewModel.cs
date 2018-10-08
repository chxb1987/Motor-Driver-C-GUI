using SuperButton.CommandsDB;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SuperButton.Models;
using Abt.Controls.SciChart;
using SuperButton.Models.DriverBlock;
using System;

namespace SuperButton.ViewModels
{
    public class BottomPanelViewModel : ViewModelBase
    {
        public BottomPanelViewModel()
        {

        }
        private ObservableCollection<object> _motionCommandList;
        private ObservableCollection<object> _motionStatusList; 
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
        public ObservableCollection<object> ProfilerModeList
        {
            get
            {
                return Commands.GetInstance.EnumCommandsListbySubGroup["Profiler Mode"];
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
