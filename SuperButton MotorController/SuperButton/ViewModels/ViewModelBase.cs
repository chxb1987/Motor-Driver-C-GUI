using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace SuperButton.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected ViewModelBase()
        {
        }
        
        //protected virtual void OnPropertyChanged(string value,[CallerMemberName] string propertyName = null)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)

        //    {
        //        handler(this, new PropertyChangedEventArgs(propertyName));
        //      if(string.IsNullOrEmpty(value))
        //          throw new ArgumentException("error");
        //    }
        //}
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            PropertyChangedEventHandler handler = PropertyChanged;

            if(handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
