namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System.Windows;

    #endregion

    public static class ShowWindowWithDatacontextBehavior
    {
        #region Static

        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.RegisterAttached(
                "DataContext",
                typeof(object),
                typeof(ShowWindowWithDatacontextBehavior),
                new UIPropertyMetadata(null));

        public static readonly DependencyProperty TriggerProperty =
            DependencyProperty.RegisterAttached(
                "Trigger",
                typeof(object),
                typeof(ShowWindowWithDatacontextBehavior),
                new UIPropertyMetadata(null, TriggerPropertyChanged));

        public static readonly DependencyProperty WindowProperty =
            DependencyProperty.RegisterAttached(
                "Window",
                typeof(DependencyObject),
                typeof(ShowWindowWithDatacontextBehavior),
                new UIPropertyMetadata(null));

        #endregion

        #region

        public static object GetDataContext(object d)
        {
            return (d as DependencyObject)?.GetValue(DataContextProperty);
        }

        public static object GetTrigger(object d)
        {
            return (d as DependencyObject)?.GetValue(TriggerProperty);
        }


        public static DependencyObject GetWindow(object d)
        {
            return (d as DependencyObject)?.GetValue(WindowProperty) as DependencyObject;
        }

        public static void SetDataContext(DependencyObject d, object dataContext)
        {
            d.SetValue(DataContextProperty, dataContext);
        }

        public static void SetTrigger(DependencyObject d, object Trigger)
        {
            d.SetValue(TriggerProperty, Trigger);
        }

        public static void SetWindow(DependencyObject d, DependencyObject window)
        {
            d.SetValue(WindowProperty, window);
        }


        private static void TriggerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = GetWindow(d);
            if (window == null)
                return;

            if (e.NewValue != null)
            {
                var parentWindow = Window.GetWindow(window);
                if (parentWindow != null)
                {
                    parentWindow.DataContext = GetDataContext(d);
                    parentWindow.Show();
                }
            }
        }

        #endregion
    }
}