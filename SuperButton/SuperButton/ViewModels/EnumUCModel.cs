using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Abt.Controls.SciChart.Common.Extensions;
using SuperButton.Views;

namespace SuperButton.ViewModels
{
	public class EnumUCModel : INotifyPropertyChanged
    {
        #region CommutationSource

        public List<string> CommutetionSrc=new List<string>();
        public List<string> PropsList { get; set; }

        //private IEnumerable<string> _selectedCommutationDataSource=  new[] {"DC"};
        //public IEnumerable<string> SelectedCommutationDataSource

	    private String _selectedCommutationDataSource;
        public String SelectedCommutationDataSource
        {
            get { return _selectedCommutationDataSource; }
            set
            {
                if (_selectedCommutationDataSource == value) return;
                _selectedCommutationDataSource = value;

                 ;
                 int Index = CommutetionSrc.FindIndex(x => x.Contains(_selectedCommutationDataSource));
           
                NotifyPropertyChanged("SelectedCommutationDataSource");
            }
        }

        #endregion

        #region DriveMode

        public List<string> DriveMode = new List<string>();
        public List<string> DriveModeprop { get; set; }

        #endregion

	
        #region XLS
        
        public List<string> XlsList = new List<string>(); //List of values within combo
        public List<string> XlsPropsList { get; set; }    //property on List of values within combo (XlsList)

        private string _selectedXls="const_acc_N.xlsx";                      //selected value
        
        public string SelectedXls                         //property on selected value
        {
            get { return _selectedXls; }
            set
            {
                if (_selectedXls == value) return;
                _selectedXls = value;
                LeftPanelViewModel.excelPath = LeftPanelViewModel.Exelsrootpath + @"\" + _selectedXls;
                string extension = System.IO.Path.GetExtension(_selectedXls);
                LeftPanelViewModel.name = _selectedXls.Substring(0, _selectedXls.Length - extension.Length);
                NotifyPropertyChanged("SelectedXLS");
            }
        }

        #endregion


        //public int Shoesize
        //{
        //    get { return _shoesize; }
        //    set
        //    {
        //        _shoesize = value;
        //        OnPropertyChanged("Shoesize");
        //    }
        //}



		public EnumUCModel()
		{
            CommutetionSrc.Add("Default");
            CommutetionSrc.Add("Forced");
            CommutetionSrc.Add("DC");
		    PropsList = CommutetionSrc;
		    SelectedCommutationDataSource = CommutetionSrc.Take(1).ToString();

            DriveMode.Add("Default");
            DriveMode.Add("Current Mode");
            DriveMode.Add("Speed Mode");
            DriveModeprop = DriveMode;


            XlsList.Add("const_acc_N.xlsx");
            XlsList.Add("single_N.xlsx");
            XlsList.Add("const_amp_N.xlsx");
            XlsList.Add("Dual_N.xlsx");
            XlsPropsList = XlsList;
           // string temp = XlsList.Take(1).ToString();
            SelectedXls = "const_acc_N.xlsx";
            string extension = System.IO.Path.GetExtension(SelectedXls);
            LeftPanelViewModel.name = _selectedXls.Substring(0, SelectedXls.Length - extension.Length);

		}

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
		#endregion
	}
}