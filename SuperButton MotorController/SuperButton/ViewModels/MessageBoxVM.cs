using SuperButton.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SuperButton.ViewModels
{
    public class MessageBoxVM : ViewModelBase
    {
        RelayCommand onCollectionChangeCommand;
        public RelayCommand OnCollectionChangeCommand
        {
            get { return onCollectionChangeCommand ?? (onCollectionChangeCommand = new RelayCommand(param => OnCollectionChange(param))); }
        }

        public RelayCommand OnCollectionChangeCommandDefault
        {
            get { return onCollectionChangeCommand ?? (onCollectionChangeCommand = new RelayCommand(param => OnCollectionChangeDefault(param))); }
        }

        private void OnCollectionChange(object param)
        {
            MaintenanceViewModel.CurrentButton = true;
            SuperButton.Views.MesageBox t = param as SuperButton.Views.MesageBox;
            t.Close();
        }

        private void OnCollectionChangeDefault(object param)
        {
            MaintenanceViewModel.DefaultButton = true;
            SuperButton.Views.MesageBox t = param as SuperButton.Views.MesageBox;
            t.Close();
        }

        private bool CheckValue()
        {
            return true;
        }

        
    }
}
