using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SuperButton.Models;

namespace SuperButton.ViewModels
{
     class EnumViewModel:DataViewModel
    {

       
        private  readonly List<string> _commandList=new List<string>();
        public bool IsUpdate = false;
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
                //if(IsUpdate)
                //SelectedValue = CommandList[Convert.ToInt16(value) - 1];


            }
        }


        public virtual ICommand SelectedItemChanged
        {
            get
            {
                

                return new RelayCommand(SendData, IsEnabled);
            }
        }

        private void SendData()
        {
            if (!IsUpdate)
            {
                CommandValue = (CommandList.FindIndex(x => x.StartsWith(SelectedValue)) + 1).ToString();
                BuildPacketTosend(CommandValue);
            }
           
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


            var tmp = new PacketFields
            {
                Data2Send = val,
                ID = Convert.ToInt16(CommandId),
                SubID = Convert.ToInt16(CommandSubId),
                IsSet = true,
                IsFloat = IsFloat
            };
            //Rs232Interface.GetInstance.SendToParser(tmp);
        }


        

       

    }
}

