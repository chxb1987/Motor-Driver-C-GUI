using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SuperButton.CommandsDB;
using System.Windows.Input;
using SuperButton.Models;

namespace SuperButton.ViewModels
{
    class MotionViewModel : ViewModelBase
    {
        private static readonly object Synlock = new object();
        private static MotionViewModel _instance;
        public static MotionViewModel GetInstance
        {
            get
            {
                lock (Synlock)
                {
                    if (_instance != null) return _instance;
                    _instance = new MotionViewModel();
                    return _instance;
                }
            }
        }
        private MotionViewModel()
        {

        }

        private ObservableCollection<object> _currentLimitList;
        public ObservableCollection<object> CurrentLimitList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["CurrentLimit List"];
            }
            set
            {
                _currentLimitList = value;
                OnPropertyChanged();
            }

        }
    }
}
