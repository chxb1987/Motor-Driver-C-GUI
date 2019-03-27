using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SuperButton.StatusGroup;
using BaseViewModel = SuperButton.Common.BaseViewModel;

namespace SuperButton
{
    public class GroupedExamplesViewModel: BaseViewModel
    {
        public GroupedExamplesViewModel()
        {

            StatusesModule.InitSatuses();

            GroupedExamples = StatusesModule.Statuses
                    .OrderBy(example => example.ExampleDisplayName)
                    .GroupBy(example => example.Group)
                    .OrderBy(group => group.Key)
                    .Select(group => new ExampleGroupViewModel
                    {
                       Group = group.Key,
                       Examples = group.Select(example => example).ToArray(),
                    }).ToArray();
        }

        public IEnumerable<ExampleGroupViewModel> GroupedExamples 
        { 
            get; 
            set; 
        }
    }
}
