using System.Collections.ObjectModel;
using SuperButton.CommandsDB;
using System.Windows.Input;
using SuperButton.Models;
using System.Windows.Controls;
using System;
using System.Threading;

namespace SuperButton.ViewModels
{
    internal class ParametarsWindowViewModel : ViewModelBase
    {
        private OperationViewModel _operationViewModel;
        private CalibrationViewModel _calibrationViewModel;
        private MotionViewModel _motionViewModel;
        private MaintenanceViewModel _maintenanceViewModel;
        private FeedBackViewModel _feedBackViewModel;
        private LoadParamsViewModel _loadParamsViewModel;
        public ParametarsWindowViewModel()
        {
            _operationViewModel = OperationViewModel.GetInstance;
            _calibrationViewModel = CalibrationViewModel.GetInstance;
            _motionViewModel = MotionViewModel.GetInstance;
            _maintenanceViewModel = MaintenanceViewModel.GetInstance;
            _feedBackViewModel = FeedBackViewModel.GetInstance;
            _loadParamsViewModel = LoadParamsViewModel.GetInstance;
        }
        ~ParametarsWindowViewModel() { }
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
        private ObservableCollection<object> _motorLimitlList;
        public ObservableCollection<object> MotorLimitlList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["Motion Limit"];
            }
            set
            {
                _motorLimitlList = value;
                OnPropertyChanged();
            }

        }
        public ObservableCollection<object> DigitalFeedbackFeedBackList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["Digital"];
            }

        }
        public ObservableCollection<object> AnalogFeedbackFeedBackList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["Analog"];
            }

        }
        private ObservableCollection<object> _pidCurrentList;
        public ObservableCollection<object> PidCurrentList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["PIDCurrent"];
            }
            set
            {
                _pidCurrentList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<object> _pidSpeedList;
        public ObservableCollection<object> PidSpeedList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["PIDSpeed"];
            }
            set
            {
                _pidSpeedList = value;
                OnPropertyChanged();
            }

        }
        private ObservableCollection<object> _pidPositionList;
        //private string _motorDriver;
        public ObservableCollection<object> PidPositionList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["PIDPosition"];
            }
            set
            {
                _pidPositionList = value;
                OnPropertyChanged();
            }

        }

       // private ObservableCollection<object> _deviceSerialList;
        public ObservableCollection<object> DeviceSerialList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["DeviceSerial"];
            }

        }
      //  private ObservableCollection<object> _driverFullScaleList;
        public ObservableCollection<object> DriverFullScaleList
        {

            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["DriverFullScale"];
            }

        }
        
        public virtual ICommand TestEnumChange
        {
            get
            {
                return new RelayCommand(EnumChange, CheckValue);
            }
        }
        
        public OperationViewModel OperationViewModel
        {
            get { return _operationViewModel; }
        }

        public CalibrationViewModel CalibrationViewModel
        {
            get { return _calibrationViewModel; }
        }

        public MotionViewModel MotionViewModel
        {
            get { return _motionViewModel; }
        }

        public MaintenanceViewModel MaintenanceViewModel
        {
            get { return _maintenanceViewModel; }
        }
        
        public FeedBackViewModel FeedBackViewModel
        {
            get { return _feedBackViewModel; }
        }
        public LoadParamsViewModel LoadParamsViewModel
        {
            get { return _loadParamsViewModel; }
        }


        private bool CheckValue()
        {
            return true;
        }

        private void EnumChange()
        {
        }
    }
}
