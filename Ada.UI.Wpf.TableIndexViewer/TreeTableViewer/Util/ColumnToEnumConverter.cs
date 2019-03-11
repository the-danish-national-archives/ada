namespace Ada.UI.Wpf.TableIndexViewer.TreeTableViewer.Util
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;
    using Ra.Common.ExtensionMethods;
    using Ra.Common.Wpf.Utils;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    #endregion

    public class ColumnToEnumConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as Column;
            return ((IEnumerable<object>) v?.Yield())?.Concat(v.GetForeignKeys().SelectMany(f => f.References));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}