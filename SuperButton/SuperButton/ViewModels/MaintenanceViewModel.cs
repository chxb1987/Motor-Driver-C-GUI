using System;
using System.Collections.ObjectModel;
using SuperButton.CommandsDB;
using SuperButton.Models.DriverBlock;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using SuperButton.Common;
using SuperButton.Helpers;
using System.Linq;
using SuperButton.Views;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SuperButton.ViewModels
{
    class MaintenanceViewModel : ViewModelBase
    {
        private static readonly object Synlock = new object();
        private static MaintenanceViewModel _instance;
        public static MaintenanceViewModel GetInstance
        {
            get
            {
                lock(Synlock)
                {
                    if(_instance != null)
                        return _instance;
                    _instance = new MaintenanceViewModel();
                    return _instance;
                }
            }
            set
            {
                _instance = value;
            }
        }
        private MaintenanceViewModel()
        {
        }

        private ObservableCollection<object> _maintenanceList;
        public ObservableCollection<object> MaintenanceList
        {
            get
            {
                return Commands.GetInstance.DataCommandsListbySubGroup["Maintenance List"];
            }
            set
            {
                _maintenanceList = value;
                OnPropertyChanged();
            }
        }

        private bool _save = false;
        public bool Save
        {
            get { return _save; }
            set
            {

                if(value && !GetInstance.Manufacture)
                {
                    _save = value;
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = _save ? 1 : 0,
                        ID = 63,
                        SubID = Convert.ToInt16(0),
                        IsSet = true,
                        IsFloat = false
                    }
                    );
                    Task WaitSave = Task.Run((Action)GetInstance.Wait);
                }
                else if(!value && !GetInstance.Manufacture)
                {
                    _save = value;
                }

                OnPropertyChanged();
            }
        }

        private void Wait()
        {
            Thread.Sleep(1000);
            Save = false;
            Manufacture = false;
        }
        private bool _manufacture = false;
        public bool Manufacture
        {
            get { return _manufacture; }
            set
            {
                if(value && !GetInstance.Save)
                {
                    _manufacture = value;
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = _manufacture ? 1 : 0,
                        ID = 63,
                        SubID = Convert.ToInt16(1),
                        IsSet = true,
                        IsFloat = false
                    }
                    );
                    Task WaitManufacture = Task.Run((Action)GetInstance.Wait);
                }
                else if(!value && !GetInstance.Save)
                {
                    _manufacture = value;
                }
                OnPropertyChanged();
            }
        }
        private bool _reset;
        public bool Reset
        {
            get { return _reset; }
            set
            {
                _reset = value;
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = _reset ? 1 : 0,
                    ID = 63,
                    SubID = Convert.ToInt16(9),
                    IsSet = true,
                    IsFloat = false
                }
                );
                OnPropertyChanged();
            }
        }
        private bool _enableWrite;
        public bool EnableWrite
        {
            get { return _enableWrite; }
            set
            {
                _enableWrite = value;
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = true ? 1 : 0,
                    ID = 63,
                    SubID = Convert.ToInt16(10),
                    IsSet = true,
                    IsFloat = false
                }
                );
                OnPropertyChanged();
            }
        }
        private bool _enableLoder;
        public bool EnableLoder
        {
            get { return _enableLoder; }
            set
            {
                _enableLoder = value;
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = true ? 1 : 0,
                    ID = 65,
                    SubID = Convert.ToInt16(0),
                    IsSet = true,
                    IsFloat = true
                }
                );
                OnPropertyChanged();
            }
        }

        private List<UInt32> ParamsToFile = new List<UInt32>();
        private List<UInt32> FileToParams = new List<UInt32>();

        private bool _saveToFile = false;
        public static bool CurrentButton = false;
        public static bool DefaultButton = false;
        public static UInt32 ParamsCount = 0;
        public bool SaveToFile
        {
            get { return _saveToFile; }
            set
            {
                if(value)
                {
                    if(OscilloscopeParameters.ChanTotalCounter != 0 || LeftPanelViewModel.GetInstance.EnRefresh == true)
                    {
                        EventRiser.Instance.RiseEevent(string.Format($"Please disable plot and Refresh option and retry!"));
                        _saveToFile = !value;
                    }
                    else
                    {
                        _saveToFile = value;
                        CurrentButton = false;
                        DefaultButton = false;
                        ParamsToFile.Clear();
                        //string msg = string.Format("Do yo want to save default parameters or current parameters?");
                        //MessageBoxWrapper.Show(msg, "");
                        //if(DefaultButton)
                        //{
                        //    Rs232Interface.GetInstance.SendToParser(new PacketFields
                        //    {
                        //        Data2Send = 1,
                        //        ID = 63,
                        //        SubID = Convert.ToInt16(1),
                        //        IsSet = true,
                        //        IsFloat = false
                        //    }
                        //    );
                        //}
                        //else if(CurrentButton)
                        //{
                        Rs232Interface.GetInstance.SendToParser(new PacketFields
                        {
                            Data2Send = 1,
                            ID = 63,
                            SubID = Convert.ToInt16(0),
                            IsSet = true,
                            IsFloat = false
                        }
                        );
                        //}
                    }
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
                    if(OscilloscopeParameters.ChanTotalCounter != 0 || LeftPanelViewModel.GetInstance.EnRefresh == true)
                    {
                        EventRiser.Instance.RiseEevent(string.Format($"Please disable plot and Refresh option and retry!"));
                        _saveToFile = !value;
                    }
                    else
                    {
                        _loadFromFile = value;
                        SelectFile(FileToParams);


                        //Rs232Interface.GetInstance.SendToParser(new PacketFields
                        //{
                        //    Data2Send = _saveToFile ? 1 : 0,
                        //    ID = 63,
                        //    SubID = Convert.ToInt16(0),
                        //    IsSet = true,
                        //    IsFloat = false
                        //}
                        //);
                    }
                }
                else if(!value)
                {
                    _loadFromFile = value;
                }

                OnPropertyChanged();
            }
        }
        private void WaitB()
        {
            Thread.Sleep(1000);
        }

        public void DataToList(UInt32 data)
        {
            if(ParamsCount > 0)
            {
                ParamsCount -= 1;
                ParamsToFile.Add(data);
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = 1,
                    ID = 67,
                    SubID = Convert.ToInt16(13),
                    IsSet = false,
                    IsFloat = false
                });
            }
            else
            {
                ParamsToFile.Add(data);
                UInt32 Sum = 0, Sum1 = 0, temp = 0;
                foreach(var item in ParamsToFile)
                    Sum1 += item;
                for(int i = 0; i < ParamsToFile.Count - 1; i++)
                {
                    temp = ParamsToFile.ElementAt(i);
                    Sum += ParamsToFile.ElementAt(i);
                }
                if(ParamsToFile.ElementAt(ParamsToFile.Count - 1) == Sum)
                {
                    SaveToFile = false;
                    SaveToFileFunc(ParamsToFile);
                    EventRiser.Instance.RiseEevent(string.Format($"Save Parameters successed"));
                }
                SaveToFileFunc(ParamsToFile);

            }
        }

        private string filePath;

        public void SaveToFileFunc(List<UInt32> ListToSave)
        {
            string Date = Day(DateTime.Now.Day) + ' ' + MonthTrans(DateTime.Now.Month) + ' ' + DateTime.Now.Year.ToString();
            string path = "\\Redler\\Parameters\\" + Date + ' ' + DateTime.Now.ToString("HH:mm:ss");
            path = (path.Replace('-', ' ')).Replace(':', '_');
            path += ".txt";
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + path;
            filePath = path;
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Redler\\Parameters\\");
            if(!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                //RecFlag = true;
            }
            else if(File.Exists(filePath))
            {
                File.Delete(filePath);
                File.Create(filePath).Close();
                //RecFlag = true;
            }
            using(BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                foreach(var item in ListToSave)
                {
                    writer.Write(item.ToString("X2").PadLeft(8, '0'));
                    writer.Write(" ");
                }
            }
        }
        public static string MonthTrans(int month)
        {
            switch(month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "Septembre";
                case 10:
                    return "Octobre";
                case 11:
                    return "Novembre";
                case 12:
                    return "Decembre";
                default:
                    return "x";
            }

        }
        public static string Day(int day)
        {
            if(day < 10)
            {
                return "0" + day.ToString();
            }
            else
                return day.ToString();

        }

        private void SelectFile(List<UInt32> ListToBurn)
        {

            System.Windows.Forms.OpenFileDialog ChooseFile = new System.Windows.Forms.OpenFileDialog();
            ChooseFile.Filter = "All Files (*.*)|*.*";
            ChooseFile.FilterIndex = 1;

            ChooseFile.Multiselect = false;
            ChooseFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Redler\\Parameters\\";

            if(ChooseFile.ShowDialog() == DialogResult.OK)
            {
                string readText = Regex.Replace(File.ReadAllText(ChooseFile.FileName), @"[^\b-\u0001]", String.Empty);
                var array = readText.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
                List<string> temp = new List<string>();
                foreach(var elements in array)
                    temp.Add(Regex.Replace(elements, @"[^\b-\u0001]", String.Empty));
                //Remove('\b').Replace('\u0001', ' ');

                var numbers = array.Select(arg => int.Parse(arg)).ToList();
            }
        }
    }
}
