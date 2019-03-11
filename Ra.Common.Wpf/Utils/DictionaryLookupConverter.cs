namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    #endregion

    public class DictionaryLookupConverter : DependencyObject, IValueConverter
    {
        #region Static

        // TODO make generic?
        public static readonly DependencyProperty ConvertFunctionProperty = DependencyProperty.Register(
            "ConvertFunction", typeof(object), typeof(DictionaryLookupConverter),
            new PropertyMetadata(default(Func<string, string>)));

        #endregion

        #region Properties

        public object ConvertFunction
        {
            get => GetValue(ConvertFunctionProperty);
            set => SetValue(ConvertFunctionProperty, value);
        }

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dictionary = ConvertFunction as Func<string, string>;

            if (dictionary != null)
                return dictionary?.Invoke(value as string);

            var dictionary2 = ConvertFunction as Func<string, object>;
            if (dictionary2 != null)
                return dictionary2?.Invoke(value as string);

            return null;
            //            if (ConvertFunction.GetType().GetMethod("Invoke") as MethodInfo invoke)
            //            {
            //                
            //            }
            //
            //            return dictionary?.Invoke(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}