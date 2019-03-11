namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Data;
    using System.Globalization;
    using System.Windows.Data;
    using Types;

    #endregion

    [ValueConversion(typeof(object), typeof(object))]
    public class FieldFromDataRowViewConverter : BaseConverter, IValueConverter
    {
        #region Static

        private static FieldFromDataRowViewConverter _instance;

        #endregion

        #region Properties

        public static FieldFromDataRowViewConverter Instance => _instance ?? (_instance = new FieldFromDataRowViewConverter());

        public object Parameter { get; set; }

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var view = value as DataRowView;
            var matcher = new PatternMatcher<object>()
                .Case<int>(i => view?.Row[i])
                .Case<string>(s => view?.Row[s])
                .Case<DataColumn>(d => view?.Row[d])
                .Default(null);

            var res = matcher.Match(parameter ?? Parameter);
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}