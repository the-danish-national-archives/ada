namespace Ada.UI.Wpf.TableIndexViewer.ERGraph.Controls
{
    #region Namespace Using

    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Behaviors;
    using GraphX.Controls;
    using GraphX.Controls.Models;
    using Models;
    using QuickGraph;
    using Ra.Common.Wpf.Utils;

    #endregion

    public class TableIndexGraphArea :
        GraphArea<TableIndexVertex, TableIndexEdge, BidirectionalGraph<TableIndexVertex, TableIndexEdge>>,
        INotifyPropertyChanged
    {
        #region Static

        public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register(
            "Settings",
            typeof(TableIndexGraphSettings),
            typeof(TableIndexGraphArea),
            new PropertyMetadata(default(TableIndexGraphSettings), SettingsPropertyChangedCallback));

        // Using a DependencyProperty as the backing store for Graph.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register("Graph", typeof(TableIndexGraph), typeof(TableIndexGraphArea), new PropertyMetadata(GraphPropertyChangedCallback));

        #endregion

        #region  Fields

        private TableIndexZoomControl ZoomContainer;

        #endregion

        #region  Constructors

        static TableIndexGraphArea()
        {
            AllowDropProperty.OverrideMetadata(
                typeof(TableIndexGraphArea),
                new FrameworkPropertyMetadata(true));
        }

        public TableIndexGraphArea()
        {
            EdgeLabelFactory = new DefaultEdgelabelFactory();
            ControlFactory = new TableIndexGraphControlFactory(this);
        }

        #endregion

        #region Properties

        public TableIndexGraph Graph
        {
            get => (TableIndexGraph) GetValue(GraphProperty);
            set => SetValue(GraphProperty, value);
        }

        public Point NextTablePosition { get; set; }

        public TableIndexGraphSettings Settings
        {
            get => (TableIndexGraphSettings) GetValue(SettingsProperty);
            set => SetValue(SettingsProperty, value);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region

        private static void DebugWithTime(string s)
        {
            Debug.WriteLine($"{DateTime.Now:O}: {s}");
        }

        public override void GenerateGraph(BidirectionalGraph<TableIndexVertex, TableIndexEdge> graph, bool generateAllEdges = true, bool dataContextToDataItem = true)
        {
            var oldCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                base.GenerateGraph(graph, generateAllEdges, dataContextToDataItem);
            }
            catch (Exception)
            {
                // GenerateGraph can throw error not worth dealing with
            }
            finally
            {
                Mouse.OverrideCursor = oldCursor;
            }
        }

        private static void GraphPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as TableIndexGraphArea;

            var tableIndexGraph = e.NewValue as TableIndexGraph;

            if (me == null || tableIndexGraph == null) return;


            me.ZoomContainer?.ZoomToFill();

            me.LogicCore = new TableIndexLogicCore {Graph = tableIndexGraph, AsyncAlgorithmCompute = true};

            me.SetupLogicCoreFromSettings();

            me.GenerateGraph(me.LogicCore.Graph, false);

            me.ZoomContainer?.ZoomToFill();
        }


//        private int? _verteciesSelected = null;
//
//        public int VerteciesSelected
//        {
//            get
//            {
//                return _verteciesSelected ?? (_verteciesSelected = GetVerteciesSelected()).Value;
//            }
//        }
//        private int GetVerteciesSelected()
//        {
//            return VertexList.Values.Count(vc => SelectBehavior.GetSelected(vc));
//        }
//
//        public void InvalidateVerteciesSelected()
//        {
//            _verteciesSelected = null;
//            OnPropertyChanged(nameof(VerteciesSelected));
//        }


        private void NewTableIndexGraph_EdgeRemoved(TableIndexEdge tableIndexEdge)
        {
            RemoveEdge(tableIndexEdge);
        }

        private void NewTableIndexGraph_VertexRemoved(TableIndexVertex vertex)
        {
            SafeRemoveVertex(vertex);
        }

        protected override void OnGenerateGraphFinished()
        {
            base.OnGenerateGraphFinished();

            Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                new Action(
                    () =>
                    {
                        var oldCursor = Mouse.OverrideCursor;
                        Mouse.OverrideCursor = Cursors.Wait;

                        try
                        {
                            GenerateAllEdges();
                        }
                        finally
                        {
                            Mouse.OverrideCursor = oldCursor;
                        }
                    }));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

//

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == LogicCoreProperty)
            {
                var oldTableIndexLogicCore = e.OldValue as TableIndexLogicCore;
                var oldTableIndexGraph = oldTableIndexLogicCore?.Graph as TableIndexGraph;
                if (oldTableIndexGraph != null)
                {
                    oldTableIndexGraph.VertexAdded -= TableIndexGraph_VertexAdded;
                    oldTableIndexGraph.EdgeAdded -= TableIndexGraph_EdgeAdded;
                    oldTableIndexGraph.VertexRemoved -= NewTableIndexGraph_VertexRemoved;
                    oldTableIndexGraph.EdgeRemoved -= NewTableIndexGraph_EdgeRemoved;
                }

                var newTableIndexLogicCore = e.NewValue as TableIndexLogicCore;
                var newTableIndexGraph = newTableIndexLogicCore?.Graph as TableIndexGraph;
                if (newTableIndexGraph != null)
                {
                    newTableIndexGraph.VertexAdded += TableIndexGraph_VertexAdded;
                    newTableIndexGraph.EdgeAdded += TableIndexGraph_EdgeAdded;
                    newTableIndexGraph.VertexRemoved += NewTableIndexGraph_VertexRemoved;
                    newTableIndexGraph.EdgeRemoved += NewTableIndexGraph_EdgeRemoved;
                }
            }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            if (ZoomContainer != null)
                ZoomContainer.AreaSelected -= ZoomContainerOnAreaSelected;

            base.OnVisualParentChanged(oldParent);

            ZoomContainer = UIHelper.FindVisualParent<TableIndexZoomControl>(this);

            if (ZoomContainer != null)
                ZoomContainer.AreaSelected += ZoomContainerOnAreaSelected;
        }

        /// <summary>
        ///     Remove vertex and do all cleanup necessary for current demo
        /// </summary>
        /// <param name="tableIndexVertex">vertexControl object</param>
        private void SafeRemoveVertex(TableIndexVertex tableIndexVertex)
        {
            // Hack to fix GraphX giving null-error when deleting controls with selfloop
            foreach (var foreignKey in
                tableIndexVertex.Table.ForeignKeys.Where(
                    f => string.Equals(f.ReferencedTable, tableIndexVertex.Table.Name)))
            {
                var edge = EdgesList.Keys
                    .FirstOrDefault(e => e.ForeignKey == foreignKey);
                if (edge != null) RemoveEdge(edge);
            }


            RemoveVertexAndEdges(tableIndexVertex);
//            InvalidateVerteciesSelected();
        }

        private static void SettingsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as TableIndexGraphArea;

            var oldSetttings = e.OldValue as TableIndexGraphSettings;
            if (me != null && oldSetttings != null)
            {
                oldSetttings.RedrawLayoutRequested -= me.Setttings_RedrawLayoutRequested;
                oldSetttings.TableWantsFocus -= me.Setttings_TableWantsFocus;
                oldSetttings.RefreshSelected -= me.Setttings_RefreshSelected;
            }

            var setttings = e.NewValue as TableIndexGraphSettings;
            if (me != null && setttings != null)
            {
                setttings.RedrawLayoutRequested += me.Setttings_RedrawLayoutRequested;
                setttings.TableWantsFocus += me.Setttings_TableWantsFocus;
                setttings.RefreshSelected += me.Setttings_RefreshSelected;
            }

            if (me == null || setttings == null)
                return;

            me.SetupLogicCoreFromSettings();
        }

        private void Setttings_RedrawLayoutRequested()
        {
            var oldCore = LogicCore;

            LogicCore = new TableIndexLogicCore {Graph = oldCore.Graph};

            SetupLogicCoreFromSettings();

            GenerateGraph(LogicCore.Graph, false);


            ZoomContainer?.ZoomToFill();
        }

        private void Setttings_RefreshSelected()
        {
            foreach (var pair in VertexList)
                if (SelectBehavior.GetSelected(pair.Value) != pair.Key.Selected)
                    SelectBehavior.SetSelected(pair.Value, pair.Key.Selected);
        }

        private void Setttings_TableWantsFocus(TableIndexVertex tableIndexVertex)
        {
            var vertexControl = VertexList[tableIndexVertex];
            ZoomToVertex(vertexControl);
            SelectBehavior.SetSelected(vertexControl, true);
        }

        private void SetupLogicCoreFromSettings()
        {
            if (Settings == null)
                return;

            LogicCore.DefaultLayoutAlgorithm = Settings.LayoutAlgorithm;
            LogicCore.DefaultEdgeRoutingAlgorithm = Settings.EdgeRoutingAlgorithm;
            LogicCore.DefaultOverlapRemovalAlgorithm = Settings.OverlapRemovalAlgorithm;
            LogicCore.EdgeCurvingEnabled = Settings.EdgeCurvingEnabled;
        }

        private void TableIndexGraph_EdgeAdded(TableIndexEdge e)
        {
            var sourceControl = VertexList[e.Source] as TableVertexControl
                ;
            e.SourceConnectionPointId = sourceControl.GetSourceVertexConnectionPointId(e);
            var targetControl = VertexList[e.Target];
            var ec = ControlFactory.CreateEdgeControl(sourceControl, targetControl, e);
            AddEdge(e, ec, true);
        }

        private void TableIndexGraph_VertexAdded(TableIndexVertex vertex)
        {
            var vc = ControlFactory.CreateVertexControl(vertex);
            vc.SetPosition(NextTablePosition == default(Point) ? new Point(0, 0) : NextTablePosition);
            AddVertex(vertex, vc, true);
        }

        /// <summary>
        ///     Select vertex by setting its select value
        /// </summary>
        /// <param name="vc">VertexControl object</param>
        private void ToggleSelectVertex(DependencyObject vc)
        {
            SelectBehavior.SetSelected(vc, !SelectBehavior.GetSelected(vc));

//            InvalidateVerteciesSelected();
        }

        private void ZoomContainerOnAreaSelected(object sender, AreaSelectedEventArgs args)
        {
            var r = args.Rectangle;

            var oldCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                using (var d = Dispatcher.DisableProcessing())
                {
                    foreach (var control in VertexList.Values)
                    {
                        var offset = control.GetPosition();
                        var irect = new Rect(offset.X, offset.Y, control.ActualWidth, control.ActualHeight);
                        if (irect.IntersectsWith(r)) ToggleSelectVertex(control);
                    }
                }
            }
            finally
            {
                Mouse.OverrideCursor = oldCursor;
            }
        }


        private void ZoomToVertex(VertexControl vc)
        {
            // Animation?

            // let offset scale with number of items
            double offset = 0; // 50 * Math.Sqrt(GraphArea.VertexList.Count);

            var pos = vc.GetPosition();
            var rectangle = new Rect(
                pos.X - offset,
                pos.Y - offset,
                vc.ActualWidth + offset * 2,
                vc.ActualHeight + offset * 2);


            ZoomContainer?.ZoomToContent(rectangle);
            ZoomContainer?.ZoomToFill();
        }

        #endregion
    }
}