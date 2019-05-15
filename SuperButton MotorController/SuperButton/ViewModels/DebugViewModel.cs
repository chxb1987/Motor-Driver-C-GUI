using Abt.Controls.SciChart;
using SuperButton.Models.DriverBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Collections.ObjectModel;
using SuperButton.CommandsDB;

namespace SuperButton.ViewModels
{
    internal class DebugViewModel : ViewModelBase
    {
        #region FIELDS
        private static readonly object Synlock = new object();
        private static DebugViewModel _instance;
        #endregion FIELDS
        
        public static DebugViewModel GetInstance
        {
            get
            {
                lock (Synlock)
                {
                    if (_instance != null) return _instance;
                    _instance = new DebugViewModel();
                    return _instance;
                }
            }
            set
            {
                _instance = value;
            }
        }
        private DebugViewModel()
        {

        }

        private ObservableCollection<object> _debugList;
        public ObservableCollection<object> DebugList
        {
            get
            {
                return Commands.GetInstance.DebugListbySubGroup["DebugList"];
            }
            set
            {
                _debugList = value;
                OnPropertyChanged();
            }
        }
    }
}






