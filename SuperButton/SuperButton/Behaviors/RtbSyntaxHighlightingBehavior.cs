using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;

namespace SuperButton.Behaviors
{
    public class RtbSyntaxHighlightingBehavior: Behavior<RichTextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObjectLoaded;
        }

        void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
#if !SILVERLIGHT
            AssociatedObject.Document = new FlowDocument();
#endif
         //   AssociatedObject.Highlight();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= AssociatedObjectLoaded;
        }
    }
}
