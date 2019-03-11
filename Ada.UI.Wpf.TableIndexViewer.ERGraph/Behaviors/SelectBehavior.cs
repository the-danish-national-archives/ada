namespace Ada.UI.Wpf.TableIndexViewer.ERGraph.Behaviors
{
    #region Namespace Using

    using System;
    using System.Windows;
    using System.Windows.Input;
    using GraphX;
    using GraphX.Controls;

    #endregion

    public class SelectBehavior
    {
        #region SelectedEdges enum

        public enum SelectedEdges
        {
            None,
            In,
            Out,
            All
        }

        #endregion

        #region Static

        //trigger
        public static readonly DependencyProperty IsSelectableEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsSelectableEnabled",
                typeof(bool),
                typeof(SelectBehavior),
                new PropertyMetadata(false, IsSelectableEnabledPropertyChangedCallback));

        //settings
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.RegisterAttached(
            "Selected",
            typeof(bool),
            typeof(SelectBehavior),
            new PropertyMetadata(false, SelectedPropertyChangedCallback));

        public static readonly DependencyProperty SelectedForEdgeProperty = DependencyProperty.RegisterAttached(
            "SelectedForEdge",
            typeof(bool),
            typeof(SelectBehavior),
            new PropertyMetadata(false, SelectedPropertyChangedCallback, SelectedForEdgeCoerceValueCallback));

        public static readonly DependencyProperty SyncDragBehaviourProperty =
            DependencyProperty.RegisterAttached(
                "SyncDragBehaviour",
                typeof(bool),
                typeof(SelectBehavior),
                new PropertyMetadata(false));

        public static readonly DependencyProperty UpdateDataContextProperty =
            DependencyProperty.RegisterAttached(
                "UpdateDataContext",
                typeof(bool),
                typeof(SelectBehavior),
                new PropertyMetadata(false));

        public static readonly DependencyProperty SelectedEdgesProperty =
            DependencyProperty.RegisterAttached(
                "SelectedEdges",
                typeof(SelectedEdges),
                typeof(SelectBehavior),
                new PropertyMetadata(SelectedEdges.Out, PropertyChangedCallback));

        private static Point? mousePos;

        #endregion

        #region

        private static void CoarseConnectedEdges(DependencyObject d)
        {
            var area = GetAreaFromObject(d);
            foreach (var gc in area.GetRelatedEdgeControls(d as IGraphControl))
            {
                var ec = gc as EdgeControl;
                if (ec == null)
                    continue;
                if (ReferenceEquals(ec.Source, d) || ReferenceEquals(ec.Target, d))
                    ec.CoerceValue(SelectedForEdgeProperty);
            }
        }

        private static void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var ii = sender as IInputElement;
            if (ii == null)
                return;
            mousePos = e.GetPosition(ii);
        }

        private static void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (mousePos == null)
                return;

            var d = sender as DependencyObject;
            {
                SetSelected(d, !GetSelected(d));
            }
        }

        private static void Element_MouseMove(object sender, MouseEventArgs e)
        {
            var ii = sender as IInputElement;
            if (ii == null)
                return;
            if (mousePos != null)
            {
                var newPos = e.GetPosition(ii);
                if (newPos != mousePos)
                    mousePos = null;
            }
        }

        private static GraphAreaBase GetAreaFromObject(object obj)
        {
            GraphAreaBase area = null;

            if (obj is VertexControl)
                area = ((VertexControl) obj).RootArea;
            else if (obj is EdgeControl)
                area = ((EdgeControl) obj).RootArea;
            else if (obj is DependencyObject)
                area = VisualTreeHelperEx.FindAncestorByType((DependencyObject) obj, typeof(GraphAreaBase), false) as GraphAreaBase;

            return area;
        }

        public static bool GetIsSelectableEnabled(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsSelectableEnabledProperty);
        }


        public static bool GetSelected(DependencyObject obj)
        {
            return (bool) obj.GetValue(SelectedProperty);
        }

        public static SelectedEdges GetSelectedEdges(DependencyObject obj)
        {
            return (SelectedEdges) obj.GetValue(SelectedEdgesProperty);
        }

        public static bool GetSelectedForEdge(DependencyObject obj)
        {
            return (bool) obj.GetValue(SelectedForEdgeProperty);
        }

        public static bool GetSyncDragBehaviour(DependencyObject obj)
        {
            return (bool) obj.GetValue(SyncDragBehaviourProperty);
        }

        public static bool GetUpdateDataContext(DependencyObject obj)
        {
            return (bool) obj.GetValue(UpdateDataContextProperty);
        }

        private static void IsSelectableEnabledPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as IInputElement;

            if (element == null)
                return;

            if (!(e.NewValue is bool))
                return;

            var value = (bool) e.NewValue;

            if (value)
            {
                element.MouseLeftButtonDown += Element_MouseLeftButtonDown;
                element.MouseMove += Element_MouseMove;
                element.MouseLeftButtonUp += Element_MouseLeftButtonUp;
            }
            else
            {
                element.MouseLeftButtonUp -= Element_MouseLeftButtonDown;
                element.MouseMove -= Element_MouseMove;
                element.MouseLeftButtonUp -= Element_MouseLeftButtonUp;
            }
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(SelectedForEdgeProperty);
        }

        private static object SelectedForEdgeCoerceValueCallback(DependencyObject d, object b)
        {
            var element = d as FrameworkElement;
            var selectableConnector = element?.DataContext as ISelectableConnector;
            if (selectableConnector == null)
                return SelectedForEdgeProperty.DefaultMetadata.DefaultValue;

            switch (GetSelectedEdges(d))
            {
                case SelectedEdges.None:
                    return false;
                case SelectedEdges.In: // Switched around to show similar behavior to hightbehavior
                    return selectableConnector.Out.Selected;
                case SelectedEdges.Out:
                    return selectableConnector.In.Selected;
                case SelectedEdges.All:
                    return selectableConnector.Out.Selected || selectableConnector.In.Selected;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void SelectedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is bool))
                return;

            if (GetSyncDragBehaviour(d)) DragBehaviour.SetIsTagged(d, (bool) e.NewValue);
            if (GetUpdateDataContext(d))
            {
                var element = d as FrameworkElement;
                var selectable = element?.DataContext as ISelectable;
                if (selectable != null)
                    selectable.Selected = (bool) e.NewValue;
            }

            CoarseConnectedEdges(d);
        }

        public static void SetIsSelectableEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectableEnabledProperty, value);
        }

        public static void SetSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectedProperty, value);
        }

        public static void SetSelectedEdges(DependencyObject obj, SelectedEdges value)
        {
            obj.SetValue(SelectedEdgesProperty, value);
        }

        public static void SetSelectedForEdge(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectedForEdgeProperty, value);
        }

        public static void SetSyncDragBehaviour(DependencyObject obj, bool value)
        {
            obj.SetValue(SyncDragBehaviourProperty, value);
        }

        public static void SetUpdateDataContext(DependencyObject obj, bool value)
        {
            obj.SetValue(UpdateDataContextProperty, value);
        }

        private static int UnselectSiblings(DependencyObject d)
        {
            var res = 0;
            var area = GetAreaFromObject(d);
            foreach (var vc in area.GetAllVertexControls())
            {
                if (ReferenceEquals(vc, d))
                    continue;
                if (!GetSelected(vc))
                    continue;
                SetSelected(vc, false);
                res++;
            }

            return res;
        }

        #endregion
    }
}