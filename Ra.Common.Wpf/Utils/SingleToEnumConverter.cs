namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Data;

    #endregion

    public class SingleToEnumConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new List<object> {value};
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}