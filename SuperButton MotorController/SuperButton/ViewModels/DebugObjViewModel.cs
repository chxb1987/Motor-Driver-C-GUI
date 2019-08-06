using Abt.Controls.SciChart;
using SuperButton.CommandsDB;
using SuperButton.Models;
using SuperButton.Models.DriverBlock;
using SuperButton.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections;
using System.Runtime.CompilerServices;

namespace SuperButton.ViewModels
{
    public class DebugObjViewModel : ViewModelBase
    {
        private DebugObjModel _debugObjModel = new DebugObjModel();


        public bool IntFloat
        {
            get { return _debugObjModel.IntFloat; }
            set
            {
                _debugObjModel.IntFloat = value;
                OnPropertyChanged("IntFloat");
            }
        }
        public string ID
        {
            get { return _debugObjModel.ID; }
            set { _debugObjModel.ID = value; OnPropertyChanged("ID"); }
        }
        public string Index
        {
            get { return _debugObjModel.Index; }
            set { _debugObjModel.Index = value; OnPropertyChanged("Index"); }
        }


        public string GetData
        {
            get
            {
                Debug.WriteLine("get: " + _debugObjModel.GetData);
                return _debugObjModel.GetData;
            }
            set
            {
                _debugObjModel.GetData = value;
                Debug.WriteLine("set: " + value);
                OnPropertyChanged(nameof(GetData));
            }
        }
        public string SetData
        {
            get { return _debugObjModel.SetData; }
            set { _debugObjModel.SetData = value; OnPropertyChanged("SetData"); }
        }

        public ActionCommand Get { get { return new ActionCommand(GetCmd); } }
        public ActionCommand Set { get { return new ActionCommand(SetCmd); } }

        private void GetCmd()
        {
            var data = new DebugObjViewModel
            {
                ID = ID,
                Index = Index,
                IntFloat = IntFloat,
                GetData = "",
                SetData = "",
            };

            Commands.GetInstance.DebugCommandsList.Remove(new Tuple<int, int>(Commands.GetInstance.DebugCommandsList.ElementAt(0).Key.Item1, Commands.GetInstance.DebugCommandsList.ElementAt(0).Key.Item2));
            Commands.GetInstance.DebugCommandsList.Add(new Tuple<int, int>(Convert.ToInt16(ID), Convert.ToInt16(Index)), data);
            try
            {
                //Rs232Interface.GetInstance.SendToParser(new PacketFields
                //{
                //    Data2Send = 0,
                //    ID = Convert.ToInt16(ID),
                //    SubID = Convert.ToInt16(Index),
                //    IsSet = false,
                //    IsFloat = !this.IntFloat
                //});
                var tmp = new PacketFields
                {
                    Data2Send = 0,
                    ID = Convert.ToInt16(ID),
                    SubID = Convert.ToInt16(Index),
                    IsSet = false,
                    IsFloat = !this.IntFloat,
                };
                Task.Factory.StartNew(action: () => { Rs232Interface.GetInstance.SendToParser(tmp); });

            }
            catch(Exception)
            {
            }

        }
        private void SetCmd()
        {
            if(SetData != "" && ID != "" && Index != "")
            {
                var tmp = new PacketFields
                {
                    Data2Send = Convert.ToInt32(SetData),
                    ID = Convert.ToInt16(ID),
                    SubID = Convert.ToInt16(Index),
                    IsSet = true,
                    IsFloat = !this.IntFloat,
                };
                Task.Factory.StartNew(action: () => { Rs232Interface.GetInstance.SendToParser(tmp); });
            }
        }
    }
}
