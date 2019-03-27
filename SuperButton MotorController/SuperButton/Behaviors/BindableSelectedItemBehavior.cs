using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace SuperButton.Behaviors
{
    public class BindableSelectedItemBehavior : Behavior<TreeView>
    {
        //public static DependencyProperty BindableSelectedItemProperty =
        //    DependencyProperty.Register("BindableSelectedItem",
        //                                typeof (Abt.Controls.SciChart.Example),
        //                                typeof (BindableSelectedItemBehavior),
        //                                new PropertyMetadata(OnBindableSelectedItemChanged));

        //public Abt.Controls.SciChart.Example BindableSelectedItem
        //{
        //    get { return (Abt.Controls.SciChart.Example) GetValue(BindableSelectedItemProperty); }
        //    set { SetValue(BindableSelectedItemProperty, value); }
        //}

        //    protected override void OnAttached()
        //    {
        //        base.OnAttached();

        //        AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        //    }

        //    protected override void OnDetaching()
        //    {
        //        base.OnDetaching();

        //        AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
        //    }

        //    private static void OnBindableSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //    {
        //        var behavior = (BindableSelectedItemBehavior) d;
        //        behavior.AssociatedObject.Select(behavior.BindableSelectedItem);
        //    }

        //    private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //    {
        //        var example = AssociatedObject.SelectedItem as Abt.Controls.SciChart.Example;

        //        if (example != null && !example.Equals(BindableSelectedItem))
        //        {
        //            example.SelectCommand.Execute(example);
        //        }
        //    }
        //}
    }
}