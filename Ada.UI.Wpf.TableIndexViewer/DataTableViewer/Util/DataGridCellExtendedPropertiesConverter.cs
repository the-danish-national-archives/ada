namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.Util
{
    #region Namespace Using

    using System;
    using System.Data;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Ra.Common.Wpf.Utils;

    #endregion

    public class DataGridCellExtendedPropertiesConverter
        : BaseConverter, IValueConverter
    {
        #region Static

        private static DataGridCellExtendedPropertiesConverter _instance;

        #endregion

        #region Properties

        public static DataGridCellExtendedPropertiesConverter Instance => _instance ?? (_instance = new DataGridCellExtendedPropertiesConverter());

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string)
                parameter = new DataGridCellExtendedPropertiesConverterHelper {PropertyName = (string) parameter};

            var helper = parameter as DataGridCellExtendedPropertiesConverterHelper;
            if (helper == null)
                return null;

            var cell = value as DataGridCell;
            if (cell?.Column == null)
                return null;

            var column = (cell?.DataContext as DataRowView)?.Row.Table.Columns[(string) cell.Column.Header];

            return helper.TargetValue.Equals(column?.ExtendedProperties[helper.PropertyName]) ? helper.ValueForTrue : helper.ValueForFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class DataGridCellExtendedPropertiesConverterHelper
    {
        #region Properties

        public string PropertyName { get; set; } = "";
        public object TargetValue { get; set; } = true;
        public object ValueForFalse { get; set; } = false;
        public object ValueForTrue { get; set; } = true;

        #endregion
    }
}