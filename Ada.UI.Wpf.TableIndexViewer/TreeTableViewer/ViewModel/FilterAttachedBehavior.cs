#region Header

// Author 
// Created 21

#endregion

namespace Ada.UI.Wpf.TableIndexViewer.TreeTableViewer.ViewModel
{
    #region Namespace Using

    using System.ComponentModel;
    using System.Windows;

    #endregion

    public class FilterAttachedBehavior : DependencyObject
    {
        #region Static

        // Using a DependencyProperty as the backing store for IsMarked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMarkedProperty =
            DependencyProperty.RegisterAttached("IsMarked", typeof(bool), typeof(FilterAttachedBehavior), new PropertyMetadata(default(bool)));

        // Using a DependencyProperty as the backing store for Filter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.RegisterAttached("Filter", typeof(TableFilterViewModel), typeof(FilterAttachedBehavior), new PropertyMetadata(default(TableFilterViewModel), FilterChangedCallback));

        #endregion

        #region

        private static void FilterChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs a)
        {
            // Only expected to change once per object (when set)
//            if (a.OldValue is TableFilterViewModel oldFilter)
//            {
//                oldFilter.PropertyChanged -= FilterOnPropertyChanged;
//            }

            if (a.NewValue is TableFilterViewModel filter)
            {
                filter.PropertyChanged += (s, e) => FilterOnPropertyChanged(d, s, e);
                FilterOnPropertyChanged(d, d, new PropertyChangedEventArgs(nameof(TableFilterViewModel.Filter)));
            }
        }

        private static void FilterOnPropertyChanged(DependencyObject d, object sender, PropertyChangedEventArgs pArgs)
        {
            switch (pArgs.PropertyName)
            {
                case nameof(TableFilterViewModel.Filter):
                    SetIsMarked(d, GetFilter(d).ShouldMark(GetData(d)));
                    break;
            }
        }


        public static object GetData(DependencyObject obj)
        {
            return (obj as FrameworkElement)?.DataContext;
        }


        public static TableFilterViewModel GetFilter(DependencyObject obj)
        {
            return (TableFilterViewModel) obj.GetValue(FilterProperty);
        }


        public static bool GetIsMarked(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsMarkedProperty);
        }

        public static void SetFilter(DependencyObject obj, TableFilterViewModel value)
        {
            obj.SetValue(FilterProperty, value);
        }

        public static void SetIsMarked(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMarkedProperty, value);
        }

        #endregion
    }
}