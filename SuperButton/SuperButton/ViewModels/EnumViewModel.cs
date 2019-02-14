using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SuperButton.Models;
using SuperButton.Models.DriverBlock;
using System.Diagnostics;

namespace SuperButton.ViewModels
{
    class EnumViewModel : DataViewModel
    {


        private readonly List<string> _commandList = new List<string>();
        public bool IsUpdate = false;
        private int Count = 0;
        private string _selectedValue;
        //private string _selectedindex="0";

        public List<string> CommandList
        {

            get { return _commandList; }
            set
            {
                foreach (var str in value)
                {
                    _commandList.Add(str);
                }

            }

        }
        public EnumViewModel(IEnumerable<string> enumlist)

        {
            _commandList.AddRange(enumlist);

        }

        new public string CommandValue
        {
            get { return base.CommandValue; }
            set
            {

                base.CommandValue = value;
                if (Count > 0)
                {
                    SelectedValue = CommandList[Convert.ToInt16(value) - 1];
                    Count++;
                }
                if (Count == 5)
                    Count = -1;
            }
        }
        public virtual ICommand SelectedItemChanged
        {
            get
            {
                return new RelayCommand(SendData, IsEnabled);
            }
        }
        public virtual ICommand MouseLeftClick
        {
            get
            {
                return new RelayCommand(SendData, IsEnabled);
            }
        }

        private new void SendData()
        {
            
            if(Count == 0 && SelectedValue != null)
            {
                Count = 0;
                int StartIndex = 0;
                int ListIndex = CommandList.FindIndex(x => x.StartsWith(SelectedValue));
                foreach(var List in SuperButton.CommandsDB.Commands.GetInstance.EnumViewCommandsList)
                {
                    if((ListIndex < List.Value.CommandList.Count() && List.Value.CommandList[ListIndex] == SelectedValue) || (ListIndex == 0 && List.Value.CommandList[ListIndex] == SelectedValue))
                    {
                        StartIndex = Convert.ToInt16(List.Value.CommandValue);
                    }
                }
                //CommandValue = (ListIndex + StartIndex).ToString();
                BuildPacketTosend((ListIndex + StartIndex).ToString());
            }
            if(Count == -1)
                Count = 0;
        }


        private bool IsEnabled()
        {
            return true;
        }

        public string SelectedValue
        {
            get { return _selectedValue; }
            set
            {

                _selectedValue = value;

                OnPropertyChanged();
            }
        }
        //public string SelectedValue
        //{
        //    get { return _selectedValue; }
        //    set
        //    {

        //        if (_selectedValue != null)
        //        {
        //            _selectedValue = value;

        //            var index = (CommandList.FindIndex(x => x.StartsWith(value)) + 1).ToString();
        //            OnPropertyChanged();
        //            if (!IsUpdate)
        //                BuildPacketTosend(index);
        //            else
        //            {
        //                IsUpdate = false;

        //            }

        //        }
        //        else
        //        {
        //            _selectedValue = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        //public string Selectedindex
        //{
        //    get { return _selectedindex; }
        //    set
        //    {

        //        if (_selectedindex != null)
        //        {
        //            _selectedindex = value;

        //            OnPropertyChanged();


        //        }
        //    }
        //}

        public EnumViewModel()
        {
            // TODO: Complete member initialization
        }
        private void BuildPacketTosend(string val)
        {
            if (LeftPanelViewModel.GetInstance.ConnectButtonContent == "Disconnect")
            {
                Task.Factory.StartNew(action: () =>
                {
                    var tmp = new PacketFields
                    {
                        Data2Send = val,
                        ID = Convert.ToInt16(CommandId),
                        SubID = Convert.ToInt16(CommandSubId),
                        IsSet = true,
                        IsFloat = IsFloat
                    };
                    Rs232Interface.GetInstance.SendToParser(tmp);
                    //Debug.WriteLine("{0} {1}[{2}]={3} {4}.", "Set", Convert.ToInt16(CommandId), Convert.ToInt16(CommandSubId), val, IsFloat?"F":"I");

                    //tmp = new PacketFields
                    //{
                    //    Data2Send = val,
                    //    ID = Convert.ToInt16(CommandId),
                    //    SubID = Convert.ToInt16(CommandSubId),
                    //    IsSet = false,
                    //    IsFloat = IsFloat
                    //};
                    //Rs232Interface.GetInstance.SendToParser(tmp);
                });
            }
        }






    }
}

