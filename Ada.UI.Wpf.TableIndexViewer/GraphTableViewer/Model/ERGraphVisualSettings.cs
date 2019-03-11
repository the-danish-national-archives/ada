namespace Ada.UI.Wpf.TableIndexViewer.GraphTableViewer.Model
{
    #region Namespace Using

    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using ERGraph.Behaviors;
    using GraphX.PCL.Common.Enums;
    using Properties;

    #endregion

    public class ERGraphVisualSettings : INotifyPropertyChanged
    {
        #region ColumnsFilterEnum enum

        public enum ColumnsFilterEnum
        {
            None,
            PrimaryKeys,
            PrimaryAndForeignKeys,
            All
        }

        #endregion

        #region GraphControlEnum enum

        public enum GraphControlEnum
        {
            Vertex,
            Edge,
            VertexAndEdge
        }

        #endregion

        #region HighlightEdgesEnum enum

        public enum HighlightEdgesEnum
        {
            In,
            Out,
            All
        }

        #endregion

        #region SelectedEdgesEnum enum

        public enum SelectedEdgesEnum
        {
            None,
            In,
            Out,
            All
        }

        #endregion

        #region Static

        public static readonly DependencyProperty VisualSettingProperty = DependencyProperty.RegisterAttached(
            "VisualSetting",
            typeof(ERGraphVisualSettings),
            typeof(ERGraphVisualSettings),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits)
        );

        #endregion

        #region  Fields

        private ColumnsFilterEnum _columnsFilter = ColumnsFilterEnum.PrimaryAndForeignKeys;

        private GraphControlEnum _graphControlEnum = GraphControlEnum.Edge;

        private HighlightEdgesEnum _highlightEdges = HighlightEdgesEnum.In;

        private bool _isEdgeLabelAligned = true;

        private bool _isEdgeLabelsShown;


        private bool _isHighLightingEnabled = true;

        private SelectedEdgesEnum _selectedEdges = SelectedEdgesEnum.In;

        #endregion

        #region Properties

        public ColumnsFilterEnum ColumnsFilter
        {
            get => _columnsFilter;
            set
            {
                _columnsFilter = value;
                OnPropertyChanged();
            }
        }

        public GraphControlEnum GraphControl
        {
            get => _graphControlEnum;
            set
            {
                _graphControlEnum = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GraphControlType));
            }
        }

        public GraphControlType GraphControlType =>
            (GraphControlType) Enum.Parse(typeof(GraphControlType), GraphControl.ToString());

        public HighlightEdgesEnum HighlightEdges
        {
            get => _highlightEdges;
            set
            {
                _highlightEdges = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HighlightEdgesType));
            }
        }


        public EdgesType HighlightEdgesType => (EdgesType) Enum.Parse(typeof(EdgesType), HighlightEdges.ToString());

        public bool IsEdgeLabelAligned
        {
            get => _isEdgeLabelAligned;
            set
            {
                _isEdgeLabelAligned = value;
                OnPropertyChanged();
            }
        }

        public bool IsEdgeLabelsShown
        {
            get => _isEdgeLabelsShown;
            set
            {
                _isEdgeLabelsShown = value;
                OnPropertyChanged();
            }
        }

        public bool IsHighLightingEnabled
        {
            get => _isHighLightingEnabled;
            set
            {
                _isHighLightingEnabled = value;
                OnPropertyChanged();
            }
        }

        public SelectedEdgesEnum SelectedEdges
        {
            get => _selectedEdges;
            set
            {
                _selectedEdges = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedEdgesType));
            }
        }

        public SelectBehavior.SelectedEdges SelectedEdgesType =>
            (SelectBehavior.SelectedEdges) Enum.Parse(typeof(SelectBehavior.SelectedEdges), SelectedEdges.ToString());

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region

        public static ERGraphVisualSettings GetVisualSetting(UIElement element)
        {
            return (ERGraphVisualSettings) element.GetValue(VisualSettingProperty);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static void SetVisualSetting(UIElement element, ERGraphVisualSettings value)
        {
            element.SetValue(VisualSettingProperty, value);
        }

        #endregion
    }
}