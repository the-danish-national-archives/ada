namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using Interfaces;

    #endregion

    public static class TreeViewOpenBehavior
    {
        #region Static

        public static readonly DependencyProperty TreeViewOpenProperty =
            DependencyProperty.RegisterAttached(
                "TreeViewOpen",
                typeof(object),
                typeof(TreeViewOpenBehavior),
                new UIPropertyMetadata(null, TreeViewOpenPropertyChanged));

        private static readonly DependencyProperty TreeViewOpenIsUpdatingProperty =
            DependencyProperty.RegisterAttached(
                "TreeViewOpenIsUpdating",
                typeof(bool),
                typeof(TreeViewOpenBehavior),
                new UIPropertyMetadata(false));

        #endregion

        #region

        private static bool DeepSearch(TreeViewItem topTreeViewItem, object value)
        {
            if (ReferenceEquals(value, topTreeViewItem.DataContext))
            {
                topTreeViewItem.IsSelected = true;
                topTreeViewItem.BringIntoView();
                return true;
            }

            var found = false;

            var parent = topTreeViewItem.DataContext as IContainsChild;
            if (parent != null)
            {
                if (!parent.HasChild(value))
                    return false;

                topTreeViewItem.IsExpanded = true;

                if (topTreeViewItem.ItemContainerGenerator.Status == GeneratorStatus.NotStarted)
                {
                    topTreeViewItem.ItemContainerGenerator.StatusChanged += (sender, args) =>
                    {
                        if (topTreeViewItem.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                            return;
                        foreach (var item in topTreeViewItem.Items)
                        {
                            var treeViewItem =
                                topTreeViewItem.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                            if (treeViewItem == null)
                                continue;

                            DeepSearch(treeViewItem, value);
                        }
                    };
                    found = true;
                }
            }

            if (found)
                return true;

            foreach (var item in topTreeViewItem.Items)
            {
                var treeViewItem = topTreeViewItem.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (treeViewItem == null)
                    continue;

                if (DeepSearch(treeViewItem, value))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        public static bool? GetTreeViewIsUpdatingOpen(object d)
        {
            return (d as DependencyObject)?.GetValue(TreeViewOpenIsUpdatingProperty) as bool?;
        }

        public static object GetTreeViewOpen(object d)
        {
            return (d as DependencyObject)?.GetValue(TreeViewOpenProperty);
        }

        public static void SetTreeViewIsUpdatingOpen(DependencyObject d, bool value)
        {
            d.SetValue(TreeViewOpenIsUpdatingProperty, value);
        }

        public static void SetTreeViewOpen(DependencyObject d, object value)
        {
            d.SetValue(TreeViewOpenProperty, value);
        }

        private static void TreeViewOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (GetTreeViewIsUpdatingOpen(d) == true) return;

            SetTreeViewIsUpdatingOpen(d, true);

            try
            {
                var treeView = d as TreeView;
                if (treeView == null)
                    return;

                var value = e.NewValue;

                if (value == null)
                    return;

                foreach (var item in treeView.Items)
                {
                    var treeViewItem = treeView.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                    if (treeViewItem == null)
                        continue;

                    if (!DeepSearch(treeViewItem, value))
                        continue;

                    treeViewItem.IsExpanded = true;
                    break;
                }
            }
            finally
            {
                SetTreeViewIsUpdatingOpen(d, false);
            }
        }

        #endregion
    }
}