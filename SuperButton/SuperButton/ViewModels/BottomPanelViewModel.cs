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
        private ObservableCollection<object> _rpCommandsList;
        public ObservableCollection<object> RPCommandsList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["RPCommands List"]; //Motion Limit
            }
            set
            {
                _rpCommandsList = value;
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
