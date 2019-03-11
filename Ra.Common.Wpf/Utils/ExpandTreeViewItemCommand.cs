#region Header

// Author 
// Created 08

#endregion

namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Windows;
    using System.Windows.Automation.Provider;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    #endregion

    public class ExpandTreeViewItemCommand : DependencyObject, ICommand
    {
        #region Static

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            "Target", typeof(FrameworkElement), typeof(ExpandTreeViewItemCommand), new PropertyMetadata(default(FrameworkElement)));

        #endregion

        #region Properties

        public RoutedEvent RoutedEvent { get; set; }

        public FrameworkElement Target
        {
            get => (FrameworkElement) GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return Target != null;
        }

        public void Execute(object parameter)
        {
            var el = Target as DependencyObject;

            TreeView uiElement = null;
            IExpandCollapseProvider expandCollapseProvider = null;

            while (el != null)
            {
                el = VisualTreeHelper.GetParent(el);
                uiElement = el as TreeView;
                if (uiElement != null) break;
            }

            var item = uiElement?.ItemContainerGenerator.ContainerFromItem(parameter) as TreeViewItem;

            if (item != null)
                item.IsExpanded = true;
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}