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
using System.Diagnostics;
using Abt.Controls.SciChart;


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

                if(value)
                {
                    _save = value;
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = 1,
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
            Reset = false;
        }
        private bool _manufacture = false;
        public bool Manufacture
        {
            get { return _manufacture; }
            set
            {
                if(value)
                {
                    _manufacture = value;
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = 1,
                        ID = 63,
                        SubID = Convert.ToInt16(1),
                        IsSet = true,
                        IsFloat = false
                    }
                    );
                    Task WaitManufacture = Task.Run((Action)GetInstance.Wait);
                }
                else if(!value)
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
                if(value)
                {
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = 1,
                        ID = 63,
                        SubID = Convert.ToInt16(9),
                        IsSet = true,
                        IsFloat = false
                    }
                    );
                    Task WaitManufacture = Task.Run((Action)GetInstance.Wait);
                }
                else if(!value)
                {
                    _reset = value;
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = "",
                        ID = Convert.ToInt16(60),
                        SubID = Convert.ToInt16(1), // SelectedCh1DataSource
                        IsSet = false,
                        IsFloat = false
                    });
                    Thread.Sleep(2);
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = "",
                        ID = Convert.ToInt16(60),
                        SubID = Convert.ToInt16(2), // SelectedCh2DataSource
                        IsSet = false,
                        IsFloat = false
                    });
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = "1",
                        ID = Convert.ToInt16(64),
                        SubID = Convert.ToInt16(0), // AutoBaud (Synch)
                        IsSet = true,
                        IsFloat = false
                    });
                }
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

        public static List<UInt32> ParamsToFile = new List<UInt32>();
        public static List<UInt32> FileToParams = new List<UInt32>();

        private bool _saveToFile = false;
        public static bool CurrentButton = false;
        public static bool DefaultButton = false;
        public static UInt32 ParamsCount = 0;
        public static UInt32 PbarParamsCount = 0;
        private string _pathToFile, _pathFromFile = "";
        public string PathToFile
        {
            get { return _pathToFile; }
            set { _pathToFile = value; OnPropertyChanged("PathToFile"); }
        }
        public string PathFromFile
        {
            get { return _pathFromFile; }
            set { _pathFromFile = value; OnPropertyChanged("PathFromFile"); }
        }
        public ActionCommand OpenToFileCmd
        {
            get { return new ActionCommand(OpenToFile); }
        }
        public ActionCommand OpenFromFileCmd
        {
            get { return new ActionCommand(OpenFromFile); }
        }
        public void OpenToFile()
        {
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MotorController\\Parameters\\";
            if(!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);
            Process.Start(tempPath);
        }
        public void OpenFromFile()
        {
            System.Windows.Forms.OpenFileDialog ChooseFile = new System.Windows.Forms.OpenFileDialog();
            ChooseFile.Filter = "All Files (*.*)|*.*";
            ChooseFile.FilterIndex = 1;

            ChooseFile.Multiselect = false;
            ChooseFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MotorController\\Parameters\\";

            if(ChooseFile.ShowDialog() == DialogResult.OK)
            {
                PathFromFile = ChooseFile.FileName;
            }
        }
        public bool SaveToFile
        {
            get { return _saveToFile; }
            set
            {
                if(value)
                {
                    /*if(OscilloscopeParameters.ChanTotalCounter != 0 || DebugViewModel.GetInstance.EnRefresh == true)
                    {
                        EventRiser.Instance.RiseEevent(string.Format($"Please disable plot and Refresh option and retry!"));
                        _saveToFile = !value;
                    }
                    else*/
                    PbarValueToFile = 0;
                    _redoState = PreRedoState(OscilloscopeParameters.ChanTotalCounter, DebugViewModel.GetInstance.EnRefresh);
                    {
                        _saveToFile = value;
                        //CurrentButton = false;
                        //DefaultButton = false;
                        ParamsToFile.Clear();
                        //ParamsToFile.Add(1000);
                        //ParamsToFile.Add(5);

                        //SaveToFileFunc(ParamsToFile);
                        Rs232Interface.GetInstance.SendToParser(new PacketFields
                        {
                            Data2Send = 0,
                            ID = 67,
                            SubID = Convert.ToInt16(1),
                            IsSet = false,
                            IsFloat = false
                        });
                        var task1 = Task.Factory.StartNew(action: () =>
                        {
                            CheckPBar("ToFile");
                        });
                        Task.WaitAll(task1);
                    }
                }
                else if(!value)
                {
                    _saveToFile = value;
                }

                OnPropertyChanged();
            }
        }

        public string PreRedoState(int plot, bool refresh)
        {
            if(plot > 0 && refresh)
            {
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = 0,
                    ID = 64,
                    SubID = Convert.ToInt16(1),
                    IsSet = true,
                    IsFloat = false
                });
                DebugViewModel.GetInstance.EnRefresh = false;
                return "both";
            }
            else if(plot > 0)
            {
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = 0,
                    ID = 64,
                    SubID = Convert.ToInt16(1),
                    IsSet = true,
                    IsFloat = false
                });
                return "plot";
            }
            else if(refresh)
            {
                DebugViewModel.GetInstance.EnRefresh = false;
                return "refresh";
            }
            else
                return "nothing";

        }
        public void PostRedoState(string state)
        {
            if(state == "both")
            {
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = 1,
                    ID = 64,
                    SubID = Convert.ToInt16(1),
                    IsSet = true,
                    IsFloat = false
                });
                DebugViewModel.GetInstance.EnRefresh = true;
            }
            else if(state == "refresh")
            {
                DebugViewModel.GetInstance.EnRefresh = true;
            }
            else if(state == "plot")
            {
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = 1,
                    ID = 64,
                    SubID = Convert.ToInt16(1),
                    IsSet = true,
                    IsFloat = false
                });
            }
        }
        public static string _redoState = "";
        private bool _loadFromFile = false;
        public bool LoadFromFile
        {
            get { return _loadFromFile; }
            set
            {
                if(value)
                {
                    /*if(OscilloscopeParameters.ChanTotalCounter != 0 || DebugViewModel.GetInstance.EnRefresh == true)
                    {
                        EventRiser.Instance.RiseEevent(string.Format($"Please disable plot and Refresh option and retry!"));
                        _loadFromFile = !value;
                        OnPropertyChanged("LoadFromFile");
                    }
                    else */
                    _redoState = PreRedoState(OscilloscopeParameters.ChanTotalCounter, DebugViewModel.GetInstance.EnRefresh);
                    PbarValueFromFile = 0;
                    if(PathFromFile == "")
                    {
                        //OpenToFile();
                        EventRiser.Instance.RiseEevent(string.Format($"Please choose a file and retry!"));
                        _loadFromFile = !value;
                        OnPropertyChanged("LoadFromFile");
                    }
                    else
                    {
                        FileToParams.Clear();
                        _loadFromFile = value;
                        OnPropertyChanged("LoadFromFile");
                        Rs232Interface.GetInstance.SendToParser(new PacketFields
                        {
                            Data2Send = 0,
                            ID = 67,
                            SubID = Convert.ToInt16(1),
                            IsSet = false,
                            IsFloat = false
                        } );
                        CheckPBar("FromFile");
                    }
                }
                else if(!value)
                {
                    _loadFromFile = value;
                    OnPropertyChanged("LoadFromFile");
                }
            }
        }

        public void DataToList(UInt32 data)
        {
            if(ParamsCount > 0)
            {
                ParamsCount -= 1;
                ParamsToFile.Add(data);
                PbarValueToFile = (ParamsToFile.Count) * 100 / PbarParamsCount;
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
                UInt32 Sum = 0;
                for(int i = 0; i < ParamsToFile.Count - 1; i++)
                {
                    Sum += ((ParamsToFile.ElementAt(i) >> 16) & 0xFFFF) + (ParamsToFile.ElementAt(i) & 0xFFFF);
                }
                if(ParamsToFile.ElementAt(ParamsToFile.Count - 1) == Sum)
                {
                    SaveToFile = false;
                    SaveToFileFunc(ParamsToFile);
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = 0,
                        ID = 67,
                        SubID = Convert.ToInt16(12),
                        IsSet = true,
                        IsFloat = false
                    });
                    EventRiser.Instance.RiseEevent(string.Format($"Load Parameters successed"));
                    PostRedoState(_redoState);

                }
                else
                {
                    Rs232Interface.GetInstance.SendToParser(new PacketFields
                    {
                        Data2Send = 0,
                        ID = 67,
                        SubID = Convert.ToInt16(12),
                        IsSet = true,
                        IsFloat = false
                    });
                    EventRiser.Instance.RiseEevent(string.Format($"Checksum Failed!"));
                    SaveToFileFunc(ParamsToFile);
                }
            }
        }

        private string filePath;
        public void SaveToFileFunc(List<UInt32> ListToSave)
        {
            string Date = Day(DateTime.Now.Day) + ' ' + MonthTrans(DateTime.Now.Month) + ' ' + DateTime.Now.Year.ToString();
            string path = "\\MotorController\\Parameters\\" + Date + ' ' + DateTime.Now.ToString("HH:mm:ss");
            path = (path.Replace('-', ' ')).Replace(':', '_');
            path += ".txt";
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + path;

            System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();
            saveFile.Filter = "Text (*.txt)|*.txt";
            saveFile.FileName = path;

            var t = new Thread((ThreadStart)(() =>
            {
                if(saveFile.ShowDialog() == DialogResult.OK)
                {
                    PathToFile = saveFile.FileName;
                    using(StreamWriter writer = new StreamWriter(File.Open(PathToFile, FileMode.Create)))
                    {
                        foreach(var item in ListToSave)
                        {
                            writer.Write(item.ToString("X2").PadLeft(8, '0'));
                            writer.Write(" ");
                        }
                    }
                }
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
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
        public bool SelectFile(UInt32 ParamsCount)
        {
            string readText = File.ReadAllText(PathFromFile);
            var array = readText.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
            foreach(var elements in array)
                FileToParams.Add(Convert.ToUInt32(elements, 16));
            if(ParamsCount == FileToParams.Count() - 1)
            {
                return true;
            }
            else
            {
                EventRiser.Instance.RiseEevent(string.Format($"Wrong File Detected!"));
                Rs232Interface.GetInstance.SendToParser(new PacketFields
                {
                    Data2Send = 0,
                    ID = 67,
                    SubID = Convert.ToInt16(2),
                    IsSet = true,
                    IsFloat = false
                });
                return false;
            }
            //LoadFromFile = false;
        }

        private long _pbarValueFromFile = 0;
        public long PbarValueFromFile {
            get { return _pbarValueFromFile; }
            set { _pbarValueFromFile = value; OnPropertyChanged("PbarValueFromFile"); }
        }
        private long _pbarValueToFile = 0;
        public long PbarValueToFile
        {
            get { return _pbarValueToFile; }
            set { _pbarValueToFile = value; OnPropertyChanged("PbarValueToFile"); }
        }

        private void CheckPBar(string way)
        {
            int timeout = 0;
            long tempPbarVal = 0;
            if(way == "ToFile")
            {
                Task.Factory.StartNew(action: () =>
                {
                    Thread.Sleep(1);
                    while(PbarValueToFile != 100 && timeout < 100)
                    {
                        if(tempPbarVal != PbarValueToFile)
                        {
                            tempPbarVal = PbarValueToFile;
                            timeout = 0;
                        }
                        else
                            timeout++;
                        Thread.Sleep(5);
                        if(PbarValueToFile == 100)
                            break;
                        if(timeout >= 100)
                        {
                            SaveToFile = false;
                            PbarValueToFile = 0;
                            EventRiser.Instance.RiseEevent(string.Format($"Load Parameters Failed"));
                            MaintenanceViewModel.GetInstance.PostRedoState(MaintenanceViewModel._redoState);
                        }
                    }
                });
            }
            else
            {
                Task.Factory.StartNew(action: () =>
                {
                    Thread.Sleep(1);
                    while(PbarValueFromFile != 100 && timeout < 100)
                    {
                        if(tempPbarVal != PbarValueFromFile)
                            tempPbarVal = PbarValueFromFile;
                        else
                            timeout++;
                        Thread.Sleep(5);
                        if(PbarValueFromFile == 100)
                            break;
                        if(timeout >= 100)
                        {
                            SaveToFile = false;
                            PbarValueFromFile = 0;
                            EventRiser.Instance.RiseEevent(string.Format($"Load Parameters Failed"));
                            MaintenanceViewModel.GetInstance.PostRedoState(MaintenanceViewModel._redoState);
                        }
                    }
                });
            }
        }
    }
}
