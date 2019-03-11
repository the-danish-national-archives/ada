namespace Ada.UI.Wpf.TableIndexViewer.ERGraph.Behaviors
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Models;

    #endregion

    public class PathBehavior
    {
        #region Static

        //trigger
        public static readonly DependencyProperty PathPartProperty = DependencyProperty.RegisterAttached(
            "PathPart",
            typeof(TableIndexEdge),
            typeof(PathBehavior),
            new PropertyMetadata(
                null
            )
        );

        public static readonly DependencyProperty PathProperty = DependencyProperty.RegisterAttached(
            "Path",
            typeof(IEnumerable<TableIndexEdge>),
            typeof(PathBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, PathProperty_ChangedCallback));

        public static readonly DependencyProperty SelectedPartProperty =
            DependencyProperty.RegisterAttached(
                "SelectedPart",
                typeof(bool),
                typeof(PathBehavior)
                ,
                new FrameworkPropertyMetadata(false)
            );

        public static readonly DependencyProperty PathsPartProperty = DependencyProperty.RegisterAttached(
            "PathsPart",
            typeof(TableIndexEdge),
            typeof(PathBehavior),
            new PropertyMetadata(
                null
            )
        );

        public static readonly DependencyProperty PathsProperty = DependencyProperty.RegisterAttached(
            "Paths",
            typeof(IEnumerable<TableIndexEdge>),
            typeof(PathBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, PathsProperty_ChangedCallback));

        public static readonly DependencyProperty SelectedPartForPathsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedPartForPaths",
                typeof(bool),
                typeof(PathBehavior)
                ,
                new FrameworkPropertyMetadata(false)
            );

        #endregion

        #region

        public static IEnumerable<TableIndexEdge> GetPath(DependencyObject obj)
        {
            return (IEnumerable<TableIndexEdge>) obj.GetValue(PathProperty);
        }

        public static TableIndexEdge GetPathPart(DependencyObject obj)
        {
            return (TableIndexEdge) obj.GetValue(PathPartProperty);
        }

        public static IEnumerable<TableIndexEdge> GetPaths(DependencyObject obj)
        {
            return (IEnumerable<TableIndexEdge>) obj.GetValue(PathsProperty);
        }

        public static TableIndexEdge GetPathsPart(DependencyObject obj)
        {
            return (TableIndexEdge) obj.GetValue(PathsPartProperty);
        }

        public static bool GetSelectedPart(DependencyObject obj)
        {
            return (bool) obj.GetValue(SelectedPartProperty);
        }

        public static bool GetSelectedPartForPaths(DependencyObject obj)
        {
            return (bool) obj.GetValue(SelectedPartForPathsProperty);
        }

        private static void PathProperty_ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var edges = e.NewValue as IEnumerable<TableIndexEdge>;
            if (edges == null)
                return;

            var pp = GetPathPart(d);
            if (pp != null)
                SetSelectedPart(d, edges.Contains(pp));
        }

        private static void PathsProperty_ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var edges = e.NewValue as IEnumerable<TableIndexEdge>;
            if (edges == null)
                return;


            var psp = GetPathsPart(d);
            if (psp != null)
                SetSelectedPartForPaths(d, edges.Contains(psp));
        }

        public static void SetPath(DependencyObject obj, IEnumerable<TableIndexEdge> value)
        {
            obj.SetValue(PathProperty, value);
        }

        public static void SetPathPart(DependencyObject obj, TableIndexEdge value)
        {
            obj.SetValue(PathPartProperty, value);
        }

        public static void SetPaths(DependencyObject obj, IEnumerable<TableIndexEdge> value)
        {
            obj.SetValue(PathsProperty, value);
        }

        public static void SetPathsPart(DependencyObject obj, TableIndexEdge value)
        {
            obj.SetValue(PathsPartProperty, value);
        }

        public static void SetSelectedPart(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectedPartProperty, value);
        }

        public static void SetSelectedPartForPaths(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectedPartForPathsProperty, value);
        }

        #endregion
    }
}