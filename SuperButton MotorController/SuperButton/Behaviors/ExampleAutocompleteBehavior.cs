using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace SuperButton.Behaviors
{
    public class ExampleAutocompleteBehavior: Behavior<AutoCompleteBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.FilterMode = AutoCompleteFilterMode.Custom;
          //  AssociatedObject.ItemFilter = ExampleSearchViewModel.IsAutocompleteSuggestion;

            AssociatedObject.LostFocus += OnFocusLost;
        }

        private void OnFocusLost(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Text = "";
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.FilterMode = AutoCompleteFilterMode.StartsWith;
            AssociatedObject.ItemFilter = null;

            AssociatedObject.LostFocus -= OnFocusLost;
        }
    }
}

