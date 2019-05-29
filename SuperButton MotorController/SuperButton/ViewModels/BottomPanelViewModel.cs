using SuperButton.CommandsDB;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SuperButton.Models;
using Abt.Controls.SciChart;
using SuperButton.Models.DriverBlock;
using System;
using System.Threading;
using System.Diagnostics;

namespace SuperButton.ViewModels
{
    public class BottomPanelViewModel : ViewModelBase
    {
        public BottomPanelViewModel()
        {

        }
        private static BottomPanelViewModel _instance;
        private static readonly object Synlock = new object();
        public static BottomPanelViewModel GetInstance
        {
            get
            {
                lock(Synlock)
                {
                    if(_instance != null)
                        return _instance;
                    _instance = new BottomPanelViewModel();
                    return _instance;
                }
            }
        }
        private ObservableCollection<object> _motionCommandList;
        private ObservableCollection<object> _motionCommandList2;

        private ObservableCollection<object> _motionStatusList;
        private ObservableCollection<object> _digitalInputList;
        private ObservableCollection<object> _SGList;
        private ObservableCollection<object> _positionCountersList;



        private bool _powerOutputChecked = false;

        public bool PowerOutputChecked
        {
            get
            {
                return _powerOutputChecked;
            }
            set
            {
                _powerOutputChecked = value;
                // get call stack
                StackTrace stackTrace = new StackTrace();

                if(stackTrace.GetFrame(1).GetMethod().Name != "UpdateModel")
                {
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = _powerOutputChecked ? 1 : 0,
                        ID = Convert.ToInt16(12),
                        SubID = Convert.ToInt16(1),
                        IsSet = true,
                        IsFloat = false
                    });
                }
                OnPropertyChanged("PowerOutputChecked");

            }
        }
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

        public ObservableCollection<object> MotionCommandList2
        {
            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["MotionCommand List2"]; //Motion Limit
            }
            set
            {
                _motionCommandList2 = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<object> DigitalInputList
        {
            get
            {
                return Commands.GetInstance.DigitalInputListbySubGroup["Digital Input List"]; //Motion Limit
            }
            set
            {
                _digitalInputList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<object> PositionCountersList
        {
            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["Position counters List"]; //Motion Limit
            }
            set
            {
                _positionCountersList = value;
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
