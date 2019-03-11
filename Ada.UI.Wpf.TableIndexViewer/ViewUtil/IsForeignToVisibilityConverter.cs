namespace Ada.UI.Wpf.TableIndexViewer.ViewUtil
{
    #region Namespace Using

    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using Ra.Common.Wpf.Utils;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    #endregion

    public class IsForeignToVisibilityConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Column)?.IsForeignKey() ?? false ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}