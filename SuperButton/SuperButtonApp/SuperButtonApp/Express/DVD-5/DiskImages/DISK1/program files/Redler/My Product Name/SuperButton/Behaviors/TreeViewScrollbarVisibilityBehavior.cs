using System.Windows.Controls;
using System.Windows.Interactivity;

namespace SuperButton.Behaviors
{
    //Silverlight specific - fixes Scrollbar Visibility bug in treeview
    public class TreeViewScrollbarVisibilityBehavior:Behavior<TreeView>
    {
        public ScrollBarVisibility? HorizontalScrollBarVisibility { get; set; }
        public ScrollBarVisibility? VerticalScrollBarVisibility { get; set; }

#if SILVERLIGHT

        protected override void OnAttached()
        {
            AssociatedObject.LayoutUpdated += OnTreeViewLayoutUpdated;

            base.OnAttached();
        }

        private void OnTreeViewLayoutUpdated(object sender, EventArgs e)
        {
            var scrollViewer = AssociatedObject.GetScrollHost();

            if(scrollViewer != null)
            {
                SetHorizontalScrolling(scrollViewer);
                SetVerticalScrolling(scrollViewer);
            }
        }

        private void SetHorizontalScrolling(ScrollViewer scrollViewer)
        {
            if (HorizontalScrollBarVisibility.HasValue)
            {
                if (!HorizontalScrollBarVisibility.Value.HasFlag(ScrollBarVisibility.Auto) && !HorizontalScrollBarVisibility.Value.HasFlag(ScrollBarVisibility.Visible))
                {
                    scrollViewer.ScrollToLeft();
                }

                scrollViewer.HorizontalScrollBarVisibility = HorizontalScrollBarVisibility.Value;
            }
        }

        private void SetVerticalScrolling(ScrollViewer scrollViewer)
        {
            if (VerticalScrollBarVisibility.HasValue)
            {
                if (!VerticalScrollBarVisibility.Value.HasFlag(ScrollBarVisibility.Auto) && !VerticalScrollBarVisibility.Value.HasFlag(ScrollBarVisibility.Visible))
                {
                    scrollViewer.ScrollToTop();
                }

                scrollViewer.VerticalScrollBarVisibility = VerticalScrollBarVisibility.Value;
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.LayoutUpdated -= OnTreeViewLayoutUpdated;

            base.OnDetaching();
        }

#endif
    }
}

