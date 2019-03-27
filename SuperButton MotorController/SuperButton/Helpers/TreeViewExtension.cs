using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Abt.Controls.SciChart.Example.Helpers
{
    static class TreeViewExtension
    {
        public static IEnumerable<TreeViewItem> GetContainers(this TreeView treeView)
        {
            return GetTreeViewItems(treeView.ItemContainerGenerator, treeView.Items);
        }

        private static IEnumerable<TreeViewItem> GetTreeViewItems(ItemContainerGenerator parentItemContainerGenerator, ItemCollection itemCollection)
        {
            foreach (object curChildItem in itemCollection)
            {
                var container = (TreeViewItem)parentItemContainerGenerator.ContainerFromItem(curChildItem);

                if (container != null)
                {
                    yield return container;

                    foreach (var treeViewItem in GetTreeViewItems(container.ItemContainerGenerator, container.Items))
                    {
                        yield return treeViewItem;
                    }
                }
            }
        }

        public static TreeViewItem FindContainer(this TreeView treeView, Predicate<TreeViewItem> condition)
        {
            return treeView.GetContainers().FirstOrDefault(item => condition(item));
        }

        public static bool Select(this TreeView treeView, object item)
        {
            var result = false;

            if (item != null && !item.Equals(treeView.SelectedItem))
            {
                var selected = treeView.FindContainer(tvItem => item.Equals(tvItem.Header));

                if (selected != null)
                {
                    result = (selected.IsSelected = true);
                }
            }

            return result;
        }
    }
}
