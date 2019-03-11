namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    #endregion

    public class BooleanToVisibilityConverter : BaseConverter, IValueConverter
    {
        #region Static

        private static BooleanToVisibilityConverter _instance;

        private static BooleanToVisibilityConverter _asCollapsed;

        private static BooleanToVisibilityConverter _asHidden;

        #endregion

        #region Properties

        public static BooleanToVisibilityConverter AsCollapsed => _asCollapsed ?? (_asCollapsed = new BooleanToVisibilityConverter {FalseVisibility = Visibility.Collapsed});

        public static BooleanToVisibilityConverter AsHidden => _asHidden ?? (_asHidden = new BooleanToVisibilityConverter {FalseVisibility = Visibility.Hidden});

        public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;

        public static BooleanToVisibilityConverter Instance => _instance ?? (_instance = new BooleanToVisibilityConverter());

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as bool? ?? false ? Visibility.Visible : FalseVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}