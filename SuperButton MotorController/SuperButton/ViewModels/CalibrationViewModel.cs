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
    internal class CalibrationViewModel : ViewModelBase
    {
        #region FIELDS
        private static readonly object Synlock = new object();
        private static CalibrationViewModel _instance;
        #endregion FIELDS
        
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
            set
            {
                _instance = value;
            }
        }
        private CalibrationViewModel()
        {

        }

        private ObservableCollection<object> _calibrationList;
        public ObservableCollection<object> CalibrationList
        {
            get
            {
                return Commands.GetInstance.CalibartionCommandsListbySubGroup["Calibration List"];
            }
            set
            {
                _calibrationList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<object> _calibrationResultList;
        public ObservableCollection<object> CalibrationResultList
        {
            get
            {
                return Commands.GetInstance.CalibartionCommandsListbySubGroup["Calibration Result List"];
            }
            set
            {
                _calibrationResultList = value;
                OnPropertyChanged();
            }
        }
    }
}






