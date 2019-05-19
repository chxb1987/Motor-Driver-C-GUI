using System;
using System.Windows.Input;
using SuperButton.Models;
using SuperButton.Models.DriverBlock;
using MathNet.Numerics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Linq;
using SuperButton.CommandsDB;
using Abt.Controls.SciChart;
using SuperButton.Views;

namespace SuperButton.ViewModels
{
    class DebugObjModel : ViewModelBase
    {
        private string _id;
        private string _index;
        private string _getData;
        private string _setData;
        private bool _intfloat = true; // true = int, false = float

        public DebugObjModel() {  }
        public bool IntFloat
        {
            get { return _intfloat; }
            set { _intfloat = value; }
        }
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public string GetData
        {
            get { return _getData; }
            set { _getData = value; }
        }
        public string SetData
        {
            get { return _setData; }
            set { _setData = value; }
        }
    }
}
