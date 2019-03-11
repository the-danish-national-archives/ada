namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    #endregion

    public static class DropBehavior
    {
        #region Static

        public static readonly DependencyProperty DropCommandProperty = DependencyProperty.RegisterAttached("DropCommand", typeof(ICommand), typeof(DropBehavior),
            new UIPropertyMetadata(null, OnDropCommandPropertyChanged));

        public static readonly DependencyProperty DropTargetElementProperty = DependencyProperty.RegisterAttached("DropTargetElement", typeof(UIElement), typeof(DropBehavior),
            new UIPropertyMetadata(null, OnDropCommandPropertyChanged));

        public static readonly DependencyProperty DropPositionProperty = DependencyProperty.RegisterAttached("DropPosition", typeof(Point), typeof(DropBehavior),
            new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDropCommandPropertyChanged));

        #endregion

        #region

        private static DropArgs CreateDropArgs(DependencyObject d, DragEventArgs e)
        {
            var i = d as IInputElement;
            var ui = d as UIElement;
            if (i == null || ui == null) return default(DropArgs); // internal error, it should already have been caught sender must be a control...


            var target = GetDropTargetElement(d);
            var pos = e.GetPosition(ui);
            if (target != null) pos = ui.TranslatePoint(pos, target);

            return new DropArgs(e.Data, pos);
        }

        private static void DragEnter(object sender, DragEventArgs e)
        {
            var d = sender as DependencyObject;
            if (d == null) return;

            var command = GetDropCommand(d);
            if (command?.CanExecute(CreateDropArgs(d, e)) ?? false)
                e.Effects = DragDropEffects.Link;
            else
                e.Effects = DragDropEffects.None;
        }

        private static void Drop(object sender, DragEventArgs e)
        {
            var d = sender as DependencyObject;
            if (d == null) return;

            var command = GetDropCommand(d);
            var dropArgs = CreateDropArgs(d, e);
            d.SetValue(DropPositionProperty, dropArgs.Position);
            command?.Execute(dropArgs);
        }

        public static ICommand GetDropCommand(object d)
        {
            return (d as DependencyObject)?.GetValue(DropCommandProperty) as ICommand;
        }

        public static UIElement GetDropPosition(object d)
        {
            return (d as DependencyObject)?.GetValue(DropPositionProperty) as UIElement;
        }

        public static UIElement GetDropTargetElement(object d)
        {
            return (d as DependencyObject)?.GetValue(DropTargetElementProperty) as UIElement;
        }

        private static void OnDropCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as Control;
            if (c == null) return;

            c.DragEnter += DragEnter;
            c.Drop += Drop;
        }

        public static void SetDropCommand(DependencyObject d, object value)
        {
            d.SetValue(DropCommandProperty, value);
        }

        public static void SetDropPosition(DependencyObject d, object value)
        {
            d.SetValue(DropPositionProperty, value);
        }

        public static void SetDropTargetElement(DependencyObject d, object value)
        {
            d.SetValue(DropTargetElementProperty, value);
        }

        #endregion
    }

    public struct DropArgs
    {
        public IDataObject Data { get; }

        public Point Position { get; }

        public DropArgs(IDataObject data, Point position)
        {
            Data = data;
            Position = position;
        }
    }
}