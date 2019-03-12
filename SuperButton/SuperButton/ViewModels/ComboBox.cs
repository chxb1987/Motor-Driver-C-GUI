using SuperButton.Common;
using SuperButton.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SuperButton.ViewModels
{
    public class ComboBox : ViewModelBase
    {
        private ObservableCollection<string> _comList;
        //public event EventHandler DropDownOpened;
        private string _comString;
        ICommand _dropDownOpenedCommand;

        private static ComboBox _instance;
        private static readonly object Synlock = new object(); //Single tone variable

        public static ComboBox GetInstance
        {
            get
            {
                lock (Synlock)
                {
                    if (_instance != null) return _instance;
                    _instance = new ComboBox();
                    return _instance;
                }
            }
        }

        public ComboBox()
        {
            InitComList();
        }

        #region commands
        public ICommand DropDownOpenedCommand
        {
            get
            {
                return _dropDownOpenedCommand ?? (_dropDownOpenedCommand = new RelayCommand(UpdateComList));
            }
        }

        #endregion
        #region Properies
        public string ComString
        {
            get { return _comString; }
            set
            {
                if (value != null)
                {
                    Configuration.SelectedCom = _comString = value;
                    RaisePropertyChanged("ComString");
                }
            }
        }

        public ObservableCollection<string> ComList
        {
            get
            {
                if (_comList == null)
                    _comList = new ObservableCollection<string>();
                return _comList;

            }
            private set { _comList = value; }
        }

        #endregion End Properies

        #region Methods
        public void InitComList()
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length != 0)
            {
                foreach (string comport in ports)
                    ComList.Add(comport);
            }
        }

        public void UpdateComList()
        {
            string[] ports = SerialPort.GetPortNames();
            string temp = ComString;
            ComList.Clear();
            foreach (string item in ports)
            {
                if ((ComList.Where(x => x == item).FirstOrDefault() == null))
                {
                    ComList.Add(item);
                }

            }
            if (ComList.Contains(ComString))
                ComString = temp;
        }
        #endregion Methods




    }
}
