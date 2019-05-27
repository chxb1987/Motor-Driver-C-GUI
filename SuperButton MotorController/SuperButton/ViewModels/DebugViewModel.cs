using Abt.Controls.SciChart;
using SuperButton.Models.DriverBlock;
using SuperButton.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Collections.ObjectModel;
using SuperButton.CommandsDB;
using System.Threading;

namespace SuperButton.ViewModels
{
    class DebugViewModel : ViewModelBase
    {
        #region FIELDS
        private readonly DebugObjModel _debugObjModel = new DebugObjModel();
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
                return Commands.GetInstance.DebugListbySubGroup["Debug List"];
            }
            set
            {
                _debugList = value;
                OnPropertyChanged("DebugList");
            }
        }

        private string _debugValue = "";

        public string DebugValue
        {
            get { return _debugValue; }
            set { _debugValue = value; OnPropertyChanged("DebugValue"); }
        }

#if !DEBUG
        private bool _enRefresh = true;
#else
        private bool _enRefresh = false;
#endif
        public bool EnRefresh
        {
            get
            {
                return _enRefresh;
            }
            set
            {
                _enRefresh = value;
                OnPropertyChanged("EnRefresh");
                if(value && LeftPanelViewModel.flag)
                {
                    Thread bkgnd = new Thread(LeftPanelViewModel.GetInstance.BackGroundFunc);
                    bkgnd.Start();
                    //BackGroundFunc();
                }
                else if(!value)
                {
                    RefreshManger.DataPressed = false;
                    foreach(var list in Commands.GetInstance.DataViewCommandsList)
                    {
                        try
                        {
                            Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(list.Value.CommandId), Convert.ToInt16(list.Value.CommandSubId))].IsSelected = false;
                            Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(list.Value.CommandId), Convert.ToInt16(list.Value.CommandSubId))].BackgroundStd = new SolidColorBrush(Colors.White);
                            Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(Convert.ToInt16(list.Value.CommandId), Convert.ToInt16(list.Value.CommandSubId))].BackgroundSmallFont = new SolidColorBrush(Colors.Gray);
                        }
                        catch(Exception e)
                        {
                        }
                    }
                }
            }
        }
    }
}






