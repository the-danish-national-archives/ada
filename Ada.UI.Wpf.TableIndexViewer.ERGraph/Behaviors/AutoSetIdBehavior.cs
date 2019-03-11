#region Header

// Author 
// Created 13

#endregion

namespace Ada.UI.Wpf.TableIndexViewer.ERGraph.Behaviors
{
    #region Namespace Using

    using System.Windows;
    using Controls;
    using GraphX.Controls;
    using Ra.DomainEntities.TableIndex;
    using WPFLocalizeExtension.Providers;

    #endregion


    public static class AutoSetIdBehavior
    {
        #region Static

        public static readonly DependencyProperty IsRightProperty =
            DependencyProperty.RegisterAttached(
                "IsRight",
                typeof(bool?),
                typeof(AutoSetIdBehavior),
                new FrameworkPropertyMetadata(
                    default(bool?),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    IsRightPropertyChangedCallback));

        #endregion

        #region

        private static void ConnectionPoint_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var column = e.NewValue as Column;
            if (column == null)
                return;

            var me = sender as StaticVertexConnectionPoint;
            if (me == null)
                return;

            DependencyObject parent = me;
            TableVertexControl tableVertexControl = null;
            while (parent != null && tableVertexControl == null)
            {
                parent = parent.GetParent(true);
                tableVertexControl = parent as TableVertexControl;
            }

            if (tableVertexControl == null)
                return;

            tableVertexControl.AddConnectionPointFromColumn(me, column, GetIsRight(sender));
        }


        public static bool? GetIsRight(object d)
        {
            return (d as DependencyObject)?.GetValue(IsRightProperty) as bool?;
        }

        private static void IsRightPropertyChangedCallback(DependencyObject dpObj, DependencyPropertyChangedEventArgs args)
        {
            var connectionPoint = dpObj as StaticVertexConnectionPoint;
            if (connectionPoint == null)
                return;

            connectionPoint.DataContextChanged += ConnectionPoint_DataContextChanged;
            ConnectionPoint_DataContextChanged(dpObj,
                new DependencyPropertyChangedEventArgs(
                    FrameworkElement.DataContextProperty,
                    connectionPoint.DataContext,
                    connectionPoint.DataContext));
        }

        public static void SetIsRight(DependencyObject d, object value)
        {
            d.SetValue(IsRightProperty, value);
        }

        #endregion
    }
}