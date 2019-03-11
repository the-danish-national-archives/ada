namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    #endregion

    // from https://www.codeproject.com/kb/wpf/pipingvalueconverters_wpf.aspx
    [ContentProperty("Converters")]
    public class ValueConverterGroup : IValueConverter
    {
        #region  Fields

        private readonly Dictionary<IValueConverter, ValueConversionAttribute>
            cachedAttributes = new Dictionary<IValueConverter, ValueConversionAttribute>();
        /* In the ValueConverterGroup class */

        // Fields

        #endregion

        #region  Constructors

        // Constructor
        public ValueConverterGroup()
        {
            Converters.CollectionChanged +=
                OnConvertersCollectionChanged;
        }

        #endregion

        #region Properties

        public ObservableCollection<IValueConverter> Converters { get; } = new ObservableCollection<IValueConverter>();

        #endregion

        #region IValueConverter Members

        object IValueConverter.Convert
        (
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            var output = value;


            for (var i = 0; i < Converters.Count; ++i)
            {
                var converter = Converters[i];
                var currentTargetType = GetTargetType(i, targetType, true);
                output = converter.Convert(output, currentTargetType, parameter, culture);

                // If the converter returns 'DoNothing' 
                // then the binding operation should terminate.
                if (output == Binding.DoNothing)
                    break;
            }

            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion

        #region

        protected virtual Type GetTargetType
        (
            int converterIndex, Type finalTargetType, bool convert)
        {
            // If the current converter is not the last/first in the list, 
            // get a reference to the next/previous converter.
            IValueConverter nextConverter = null;
            if (convert)
            {
                if (converterIndex < Converters.Count - 1) nextConverter = Converters[converterIndex + 1];
            }
            else
            {
                if (converterIndex > 0) nextConverter = Converters[converterIndex - 1];
            }

            if (nextConverter != null)
            {
                var attr = cachedAttributes[nextConverter];

                // If the Convert method is going to be called, 
                // we need to use the SourceType of the next 
                // converter in the list.  If ConvertBack is called, use the TargetType.
                return convert ? attr.SourceType : attr.TargetType;
            }

            // If the current converter is the last one to be executed return the target 
            // type passed into the conversion method.
            return finalTargetType;
        }

        // Callback
        private void OnConvertersCollectionChanged
        (
            object sender, NotifyCollectionChangedEventArgs e)
        {
            // The 'Converters' collection has been modified, so validate that each 
            // value converter it now contains is decorated with ValueConversionAttribute
            // and then cache the attribute value.

            IList convertersToProcess = null;

            if (e.Action == NotifyCollectionChangedAction.Add ||
                e.Action == NotifyCollectionChangedAction.Replace)
            {
                convertersToProcess = e.NewItems;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IValueConverter converter in e.OldItems)
                    cachedAttributes.Remove(converter);
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                cachedAttributes.Clear();
                convertersToProcess = Converters;
            }

            if (convertersToProcess != null && convertersToProcess.Count > 0)
                foreach (IValueConverter converter in convertersToProcess)
                {
                    var attributes = converter.GetType().GetCustomAttributes(
                        typeof(ValueConversionAttribute), false);

                    if (attributes.Length != 1)
                        throw new InvalidOperationException("All value converters added to a " +
                                                            "ValueConverterGroup must be decorated with the " +
                                                            "ValueConversionAttribute attribute exactly once.");

                    cachedAttributes.Add(
                        converter, attributes[0] as ValueConversionAttribute);
                }
        }

        #endregion
    }
}