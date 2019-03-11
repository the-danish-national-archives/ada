#region Header

// Author 
// Created 08

#endregion

namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    #endregion

    public class ExpandCollapseBehavior
    {
        #region Static

        public static readonly DependencyProperty ExpandCommandProperty =
            DependencyProperty.RegisterAttached("ExpandCommand", typeof(object),
                typeof(ExpandCollapseBehavior),
                new FrameworkPropertyMetadata(default(object),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    null, CoerceExpandCommandValueCallback));

        public static readonly DependencyProperty CollapseCommandProperty =
            DependencyProperty.RegisterAttached("CollapseCommand", typeof(object),
                typeof(ExpandCollapseBehavior),
                new FrameworkPropertyMetadata(default(object),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    null, CoerceCollapseCommandValueCallback));

        #endregion

        #region

        private static object CoerceCollapseCommandValueCallback(DependencyObject dependencyObject, object baseValue)
        {
            if (baseValue == null)
            {
                ICommand temp = new SetExpandedCommand(dependencyObject, false);

                dependencyObject.Dispatcher.BeginInvoke(
                    (Action) (() => { dependencyObject.SetCurrentValue(CollapseCommandProperty, temp); }));
            }

            return baseValue;
        }

        private static object CoerceExpandCommandValueCallback(DependencyObject dependencyObject, object baseValue)
        {
            if (baseValue == null)
            {
                ICommand temp = new SetExpandedCommand(dependencyObject, true);

                dependencyObject.Dispatcher.BeginInvoke(
                    (Action) (() => { dependencyObject.SetCurrentValue(ExpandCommandProperty, temp); }));
            }

            return baseValue;
        }

        public static object GetCollapseCommand(object d)
        {
            return (d as DependencyObject)?.GetValue(CollapseCommandProperty);
        }

        public static object GetExpandCommand(object d)
        {
            return (d as DependencyObject)?.GetValue(ExpandCommandProperty);
        }

        public static void SetCollapseCommand(DependencyObject d, object value)
        {
            d.SetValue(CollapseCommandProperty, value);
        }

        public static void SetExpandCommand(DependencyObject d, object value)
        {
            d.SetValue(ExpandCommandProperty, value);
        }

        private static void SetExpanded(object uiElement, bool state)
        {
            var treeView = uiElement as TreeView;
            if (treeView == null)
                return;
            foreach (var item in treeView.ItemContainerGenerator.Items)
            {
                var con = treeView.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (con != null)
                    con.IsExpanded = state;
            }
        }

        #endregion

        #region Nested type: SetExpandedCommand

        private class SetExpandedCommand : ICommand
        {
            #region  Fields

            private readonly DependencyObject dependencyObject;
            private readonly bool state;

            #endregion

            #region  Constructors

            public SetExpandedCommand(DependencyObject dependencyObject, bool state)
            {
                this.dependencyObject = dependencyObject;
                this.state = state;
            }

            #endregion

            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                SetExpanded(dependencyObject, state);
            }

            public event EventHandler CanExecuteChanged;

            #endregion
        }

        #endregion
    }
}