namespace Ada.UI.Wpf.TableIndexViewer.ViewUtil
{
    #region Namespace Using

    using System;
    using System.Globalization;
    using System.Windows.Data;
    using FieldCheaterViewer.ViewModel;
    using Ra.Common.Wpf.Utils;

    #endregion

    public class ToFieldCheaterConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Idea: change ToFieldPropertiesConverter to a general viewmodel factory and converter
            return new FieldCheaterViewModel(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}