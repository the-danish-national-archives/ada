namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Data;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    #endregion

    public class GetDataGridCellValueConverter : BaseConverter, IValueConverter
    {
        #region Static

        private static GetDataGridCellValueConverter _instance;

        #endregion

        #region Properties

        public static GetDataGridCellValueConverter Instance => _instance ?? (_instance = new GetDataGridCellValueConverter());

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cell = value as DataGridCell;
            //            cell.

            return (cell?.DataContext as DataRowView)?.Row[cell.Column.Header as string ?? ""]; // [cell.Column];// .Row.ItemArray[];


//            var column = (cell?.DataContext as DataRowView)?[cell.Column.DisplayIndex];// [cell.Column];// .Row.ItemArray[];
//            return value != null ? value.DataContext as Data
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}