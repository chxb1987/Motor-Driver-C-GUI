using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace SuperButton.ViewModels
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));


        }
    }
}
