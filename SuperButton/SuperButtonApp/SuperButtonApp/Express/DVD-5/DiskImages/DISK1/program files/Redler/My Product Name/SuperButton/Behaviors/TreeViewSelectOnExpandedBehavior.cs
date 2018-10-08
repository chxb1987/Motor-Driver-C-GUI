using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace SuperButton.Behaviors
{
    public class TreeViewSelectOnExpandedBehavior : Behavior<TreeView>
    {
        //public static DependencyProperty SelectedItemProperty =
        //    DependencyProperty.Register("SelectedItem",
        //                                typeof (Abt.Controls.SciChart.Example),
        //                                typeof (TreeViewSelectOnExpandedBehavior),
        //                                new PropertyMetadata(null));

        //public Abt.Controls.SciChart.Example SelectedItem
        //{
        //    get { return (Abt.Controls.SciChart.Example) GetValue(SelectedItemProperty); }
        //    set { SetValue(SelectedItemProperty, value); }
        //}

        //protected override void OnAttached()
        //{
        //    base.OnAttached();

        //    AssociatedObject.LayoutUpdated += OnTreeViewLayoutUpdated;
        //}

        //private void OnTreeViewLayoutUpdated(object sender, EventArgs e)
        //{
        //    if (AssociatedObject.Select(SelectedItem))
        //    {
        //        AssociatedObject.LayoutUpdated -= OnTreeViewLayoutUpdated;

        //        AttachOnExpandedHandler();
        //    }
        //}

        //protected override void OnDetaching()
        //{
        //    base.OnDetaching();

        //    AssociatedObject.LayoutUpdated -= OnTreeViewLayoutUpdated;
        //    DetachOnExpandedHandler();
        //}

        //private void AttachOnExpandedHandler()
        //{
        //    foreach (var treeViewItem in TreeViewExtension.GetContainers(AssociatedObject))
        //    {
        //        if (treeViewItem != null)
        //        {
        //            treeViewItem.Expanded += OnTreeViewItemExpanded;
        //        }
        //    }
        //}

        //private void OnTreeViewItemExpanded(object sender, RoutedEventArgs e)
        //{
        //    AssociatedObject.Select(SelectedItem);
        //}

        //private void DetachOnExpandedHandler()
        //{
        //    foreach (var treeViewItem in TreeViewExtension.GetContainers(AssociatedObject))
        //    {
        //        if (treeViewItem != null)
        //        {
        //            treeViewItem.Expanded -= OnTreeViewItemExpanded;
        //        }
        //    }
        //}
    }
}
