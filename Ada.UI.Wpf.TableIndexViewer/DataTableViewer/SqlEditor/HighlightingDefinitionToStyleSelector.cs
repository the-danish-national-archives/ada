namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.SqlEditor
{
    #region Namespace Using

    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using ICSharpCode.AvalonEdit.Highlighting;
    using Ra.Common.Wpf.Utils;

    #endregion

    public class HighlightingDefinitionToStyleSelector
        : BaseConverter, IMultiValueConverter, IValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //            var v = value as Column;
            //            if (v == null)
            //                return Visibility.Collapsed;
            //            return v.IsPrimaryKey() ? Visibility.Visible : Visibility.Collapsed;
//            var grid = values[0] as DataGrid;

            var name = parameter as string;
            var def = values[0] as IHighlightingDefinition;
            if ((def == null) | (name == null) | (targetType != typeof(Style)))
                return null;

            var hColor = def.GetNamedColor(name);

            var res = new Style(typeof(TextBlock));
            res.Setters.Add(new Setter(TextBlock.ForegroundProperty, hColor.Foreground.GetBrush(null)));
            res.Setters.Add(new Setter(TextBlock.BackgroundProperty, hColor.Background.GetBrush(null)));
            res.Setters.Add(new Setter(TextBlock.FontStyleProperty, hColor.FontStyle));
            res.Setters.Add(new Setter(TextBlock.FontWeightProperty, hColor.FontWeight));
            if (hColor.Underline ?? false)
                res.Setters.Add(new Setter(TextBlock.TextDecorationsProperty, TextDecorations.Underline));

//            var column = (cell?.DataContext as DataRowView)?.Row.Table.Columns[(string)cell.Column.Header];

            return res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(new[] {value}, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}