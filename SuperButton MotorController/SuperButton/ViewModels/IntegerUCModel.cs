using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SuperButton.ViewModels
{
	public class IntegerUCModel : INotifyPropertyChanged
	{

        #region CommutationSource

        private String _KPText;
        public String KPText
        {
            get { return _KPText; }
            set
            {
                if (_KPText == value) return;
                _KPText = value;
                NotifyPropertyChanged("KPText");
            }
        }

        #endregion

		public IntegerUCModel()
		{
			
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