namespace Ra.Common.Wpf.Utils.Interactivity
{
    #region Namespace Using

    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    #endregion

    // Used on sub-controls of an expander to bubble the mouse wheel scroll event up 
    public sealed class BubbleScrollEvent : Behavior<UIElement>
    {
        #region

        private void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = UIElement.MouseWheelEvent;
            AssociatedObject.RaiseEvent(e2);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
            base.OnDetaching();
        }

        #endregion
    }
}