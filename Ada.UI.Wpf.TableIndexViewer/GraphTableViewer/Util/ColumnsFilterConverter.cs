namespace Ada.UI.Wpf.TableIndexViewer.GraphTableViewer.Util
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;
    using Model;
    using Ra.Common.Wpf.Utils;
    using Ra.DomainEntities.TableIndex;
    using Ra.EntityExtensions.TableIndex;

    #endregion

    public class ColumnsFilterConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = value as IEnumerable<Column>;

            if (input == null)
                return null;

            if (!(parameter is ERGraphVisualSettings.ColumnsFilterEnum))
                throw new ArgumentException("ERGraphVisualSettings.ColumnsFilterEnum expected", nameof(parameter));

            var filterType = (ERGraphVisualSettings.ColumnsFilterEnum) parameter;


//            return ((value as bool?) ?? false) ? Visibility.Visible : FalseVisibility;
            switch (filterType)
            {
                case ERGraphVisualSettings.ColumnsFilterEnum.None:
                    return input.Where(c => false);
                case ERGraphVisualSettings.ColumnsFilterEnum.PrimaryKeys:
                    return input.Where(c => c.IsPrimaryKey());
                case ERGraphVisualSettings.ColumnsFilterEnum.PrimaryAndForeignKeys:
                    return input.Where(c => c.IsPrimaryKey() || c.IsForeignKey());
                case ERGraphVisualSettings.ColumnsFilterEnum.All:
                    return input.Where(c => true);
                default:
                    throw new InvalidOperationException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}