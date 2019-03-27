using SuperButton.Models.DriverBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SuperButton.CommandsDB;

namespace SuperButton.ViewModels
{
    class LoadParamsViewModel : ViewModelBase
    {
        private static readonly object Synlock = new object();
        private static LoadParamsViewModel _instance;
        public static LoadParamsViewModel GetInstance
        {
            get
            {
                lock(Synlock)
                {
                    if(_instance != null)
                        return _instance;
                    _instance = new LoadParamsViewModel();
                    return _instance;
                }
            }
            set
            {
                _instance = value;
            }
        }
        private LoadParamsViewModel() { }

        private List<Int32> ParamsToFile = new List<Int32>();

        private bool _saveToFile = false;
        public bool SaveToFile
        {
            get { return _saveToFile; }
            set
            {
                if(value)
                {
                    _saveToFile = value;
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = _saveToFile ? 1 : 0,
                        ID = 1,
                        SubID = Convert.ToInt16(0),
                        IsSet = false,
                        IsFloat = false
                    }
                    );
                    Task WaitSave = Task.Run((Action)GetInstance.Wait);
                }
                else if(!value)
                {
                    _saveToFile = value;
                }

                OnPropertyChanged();
            }
        }
        private bool _loadFromFile = false;
        public bool LoadFromFile
        {
            get { return _loadFromFile; }
            set
            {
                if(value)
                {
                    _loadFromFile = value;
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = _saveToFile ? 1 : 0,
                        ID = 63,
                        SubID = Convert.ToInt16(0),
                        IsSet = true,
                        IsFloat = false
                    }
                    );
                    Task WaitSave = Task.Run((Action)GetInstance.Wait);
                }
                else if(!value)
                {
                    _loadFromFile = value;
                }

                OnPropertyChanged();
            }
        }
        private void Wait()
        {
            Thread.Sleep(1000);
        }

        public void DataToList(Int32 data)
        {
            ParamsToFile.Add(data);
        }

    }
}
