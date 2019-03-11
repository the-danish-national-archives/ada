namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System.Windows;

    #endregion

    public static class CloseWindowBehavior
    {
        #region Static

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached(
                "Value",
                typeof(object),
                typeof(CloseWindowBehavior),
                new UIPropertyMetadata(null, ValuePropertyChanged));

        #endregion

        #region

        public static object GetValue(object d)
        {
            return (d as DependencyObject)?.GetValue(ValueProperty);
        }

        public static void SetValue(DependencyObject d, object value)
        {
            d.SetValue(ValueProperty, value);
        }

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var parentWindow = Window.GetWindow(d);
                parentWindow?.Close();
            }
        }

        #endregion
    }
}