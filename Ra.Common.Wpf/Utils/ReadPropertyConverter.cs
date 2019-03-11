namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Globalization;
    using System.Windows.Data;

    #endregion

    [ValueConversion(typeof(object), typeof(object))]
    public class ReadPropertyConverter : BaseConverter, IValueConverter
    {
        #region Static

        private static ReadPropertyConverter _instance;

        #endregion

        #region Properties

        public static ReadPropertyConverter Instance => _instance ?? (_instance = new ReadPropertyConverter());

        public object Parameter { get; set; }

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var propertyName = parameter as string ?? Parameter as string;
            if (propertyName == null || value == null)
                return null;
            var type = value.GetType();

            return type.GetProperty(propertyName)?.GetValue(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}