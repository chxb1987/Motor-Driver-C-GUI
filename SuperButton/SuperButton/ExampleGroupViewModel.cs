using System.Collections.Generic;
using SuperButton.StatusGroup;
using BaseViewModel = SuperButton.Common.BaseViewModel;

namespace SuperButton
{
    public class ExampleGroupViewModel : BaseViewModel
    {
        private IEnumerable<Example> _examples;

        public StatusGroups Group { get; set; }

        public IEnumerable<Example> Examples
        {
            get { return _examples; }
            set 
            {   
                _examples = value;
                OnPropertyChanged("Examples");
            }
        }
    }
}
