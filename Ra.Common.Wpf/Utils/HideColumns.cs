namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System.Data;
    using System.Windows;
    using System.Windows.Controls;

    #endregion

    public static class HideColumns
    {
        #region Static

        public const string ExtendedPropertyForHideName = "Hidden";

        // from https://codingcontext.wordpress.com/2008/12/10/commandbindings-in-mvvm/

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached(
                "Enabled",
                typeof(bool),
                typeof(HideColumns),
                new PropertyMetadata(false, OnEnabledChanged));

        #endregion

        #region

        private static void ElementOnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs args)
        {
            var name = args.Column.Header as string;
            if (name == null)
                return;

            var real = ((sender as DataGrid)?.ItemsSource as DataView)?.Table.Columns[name];

            if (real == null)
                return;

            var isHidden = real.ExtendedProperties[ExtendedPropertyForHideName];
            if (isHidden is bool)
                args.Cancel = (bool) isHidden;
        }

        public static bool GetEnabled(UIElement element)
        {
            return (bool?) element?.GetValue(EnabledProperty) ?? false;
        }

        private static void OnEnabledChanged
            (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as DataGrid;
            if (element == null) return;

            var enabled = e.NewValue as bool?;
            if (enabled != null)
            {
                if (enabled.Value)
                    element.AutoGeneratingColumn += ElementOnAutoGeneratingColumn;
                else
                    element.AutoGeneratingColumn -= ElementOnAutoGeneratingColumn;
            }
        }

        public static void SetEnableds(UIElement element, bool value)
        {
            element?.SetValue(EnabledProperty, value);
        }

        #endregion
    }
}