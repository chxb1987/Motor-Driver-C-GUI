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
    public class DebugObjModel : ViewModelBase
    {
        private string _id;
        private string _index;
        private string _getData;
        private string _setData;

        private bool _intfloat = false;

        public bool IntFloat
        {
            get { return _intfloat; }
            set { _intfloat = value; OnPropertyChanged("IntFloat"); }
        }
        public string ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }
        public string Index
        {
            get { return _index; }
            set { _index = value; OnPropertyChanged("Index"); }
        }
        public string GetData
        {
            get { return _getData; }
            set { _getData = value; OnPropertyChanged("GetData"); }
        }
        public string SetData
        {
            get { return _setData; }
            set { _setData = value; OnPropertyChanged("SetData"); }
        }
        
        public ActionCommand Get { get { return new ActionCommand(GetCmd); } }
        public ActionCommand Set { get { return new ActionCommand(SetCmd); } }

        private void GetCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = 0,
                ID = Convert.ToInt16(ID),
                SubID = Convert.ToInt16(Index),
                IsSet = false,
                IsFloat = IntFloat
            });
        }
        private void SetCmd()
        {
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = Convert.ToInt32(SetData),
                ID = Convert.ToInt16(ID),
                SubID = Convert.ToInt16(Index),
                IsSet = true,
                IsFloat = IntFloat
            });
        }
    }
}
