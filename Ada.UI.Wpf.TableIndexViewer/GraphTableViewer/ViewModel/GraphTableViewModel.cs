namespace Ada.UI.Wpf.TableIndexViewer.GraphTableViewer.ViewModel
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using ERGraph;
    using ERGraph.Models;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Model;
    using QuickGraph;
    using QuickGraph.Algorithms.ConnectedComponents;
    using QuickGraph.Algorithms.RankedShortestPath;
    using Ra.Common.Wpf.Utils;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class GraphTableViewModel : ViewModelBase
    {
        #region Static

        public static readonly RoutedCommand DeleteTableCommand = new RoutedCommand();

        #endregion

        #region  Fields

        private RelayCommand _addAllTablesCommand;

        private RelayCommand _clearAllTablesCommand;


        private RelayCommand<DropArgs> _dropCommand;

        private ICommand _findPathsCommand;

        private TableIndexGraph _graph;

        private IEnumerable<TableIndexEdge> _path;

        private IList<TableIndexEdge> _paths;

        private ICommand _selectConnectedCommand;

        #endregion

        #region  Constructors

        public GraphTableViewModel(TableIndex tableIndex)
        {
            TableIndex = tableIndex;
            Graph = new TableIndexGraph(tableIndex);

            var deleteTableBinding = new CommandBinding(DeleteTableCommand, DeleteTableExecuted, DeleteTableCanExecute);

            //Register the binding to the class
            CommandManager.RegisterClassCommandBinding(typeof(GraphTableViewModel), deleteTableBinding);

            //Adds the binding to the CommandBindingCollection
            CommandBindings.Add(deleteTableBinding);
        }

        #endregion

        #region Properties

        public RelayCommand AddAllTablesCommand
        {
            get
            {
                return _addAllTablesCommand
                       ?? (_addAllTablesCommand = new RelayCommand(
                           () => { AddAllTables(); }));
            }
        }

        public RelayCommand ClearAllTablesCommand
        {
            get
            {
                return _clearAllTablesCommand
                       ?? (_clearAllTablesCommand = new RelayCommand(
                           () => { ClearAllTables(); }));
            }
        }

        public CommandBindingCollection CommandBindings { get; } = new CommandBindingCollection();

        public RelayCommand<DropArgs> DropCommand => _dropCommand
                                                     ?? (_dropCommand = new RelayCommand<DropArgs>(
                                                         OnObjectDropped, CanObjectBeDropped));

        public ICommand FindPathsCommand
        {
            get
            {
                return _findPathsCommand ??
                       (_findPathsCommand =
                           new RelayCommand(
                               () =>
                               {
                                   var vs = Graph.Vertices.Where(v => v.Selected).ToList();
                                   if (vs.Count != 2)
                                       return;
                                   ShowPath(vs[0], vs[1]);
                               },
                               () => Graph.Vertices.Count(v => v.Selected) == 2)
                       );
            }
        }

        public TableIndexGraph Graph
        {
            get => _graph;
            set => Set(ref _graph, value);
        }

        public IEnumerable<TableIndexEdge> Path
        {
            get => _path;
            set => Set(ref _path, value);
        }

        public IList<TableIndexEdge> Paths
        {
            get => _paths;
            set => Set(ref _paths, value);
        }

        public ICommand SelectConnectedCommand
        {
            get
            {
                return _selectConnectedCommand ??
                       (_selectConnectedCommand =
                           new RelayCommand(
                               () =>
                               {
                                   var vs = Graph.Vertices.Where(v => v.Selected).ToList();
                                   SelectConnected(vs);
                               },
                               () => Graph.Vertices.Count(v => v.Selected) > 0)
                       );
            }
        }

        public ISelectionDetector SelectionDetector => Graph;

        public ERGraphSettings Settings { get; } = new ERGraphSettings();

        public TableIndex TableIndex { get; }

        #endregion

        #region

        private void AddAllTables()
        {
            Graph = new TableIndexGraph(TableIndex);
        }

        private bool CanObjectBeDropped(DropArgs o)
        {
            if (!o.Data.GetDataPresent(typeof(Table))) return false;

            return true;
        }

        private void ClearAllTables()
        {
            Graph = new TableIndexGraph();
        }


        private void DeleteTableCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter == null)
            {
                e.CanExecute = Graph.Vertices.Any(v => v.Selected);
                e.ContinueRouting = false;
            }
            else
            {
                var value = e.Parameter as TableIndexVertex;
                if (value != null)
                {
                    e.CanExecute = Graph.Vertices.Contains(value);
                    e.ContinueRouting = false;
                }
            }
        }

        private void DeleteTableExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter == null)
            {
                var toRemove = Graph.Vertices.Where(v => v.Selected).ToList();

                foreach (var v in toRemove) Graph.RemoveVertex(v);
                e.Handled = true;
            }
            else
            {
                var v = e.Parameter as TableIndexVertex;
                if (v != null)
                {
                    Graph.RemoveVertex(v);
                    e.Handled = true;
                }
            }
        }


        private void OnObjectDropped(DropArgs o)
        {
            if (!o.Data.GetDataPresent(typeof(Table))) return;

            var table = o.Data.GetData(typeof(Table)) as Table;
            if (table == null) return;

            var vertex = Graph.GetVertex(table);

            if (vertex == null)
                Graph.AddTableAndEdges(table);
            else
                Settings.GraphSettings.InvokeTableWantsFocus(vertex);
        }

        private void SelectConnected(IEnumerable<TableIndexVertex> ts)
        {
            try
            {
                var graph = new UndirectedBidirectionalGraph<TableIndexVertex, TableIndexEdge>(Graph);

                var graph2 = new BidirectionalGraph<TableIndexVertex, TableIndexEdge>();
                graph2.AddVerticesAndEdgeRange(
                    graph.Edges.SelectMany(e => new[] {e, new TableIndexEdge(e.Target, e.Source, e.ForeignKey)})
                );

                var algo = new ConnectedComponentsAlgorithm<TableIndexVertex, TableIndexEdge>(graph);

                algo.Compute();

                var toSelectColors = new HashSet<int>(ts.Select(t => algo.Components[t]));

                foreach (var pair in algo.Components)
                    if (toSelectColors.Contains(pair.Value))
                        pair.Key.Selected = true;
                Settings.GraphSettings.InvokeRefreshSelected();
            }
            catch (Exception)
            {
//                throw;
            }
        }

        private void ShowPath(TableIndexVertex t1, TableIndexVertex t2)
        {
            try
            {
                var graph = new UndirectedBidirectionalGraph<TableIndexVertex, TableIndexEdge>(Graph);

                var graph2 = new BidirectionalGraph<TableIndexVertex, TableIndexEdge>();
                graph2.AddVerticesAndEdgeRange(
                    graph.Edges.SelectMany(e => new[] {e, new FakeTableIndexEdge(e)})
                );

                var algo = new HoffmanPavleyRankedShortestPathAlgorithm
                    <TableIndexVertex, TableIndexEdge>(
                        graph2,
                        e => 1f);

                algo.ShortestPathCount = 1000;

                Func<TableIndexEdge, TableIndexEdge> convertEdgeBack = e =>
                    (e as FakeTableIndexEdge)?.e ?? e;

                algo.Compute(t1, t2);

                var path1 = algo.ComputedShortestPaths.FirstOrDefault();
                if (path1 != null)
                    Path = path1.Select(convertEdgeBack
                    ).ToList();


                // Filter paths on no loops, i.e. never returning to an already visited vertice
                Func<IEnumerable<TableIndexEdge>, IEnumerable<TableIndexVertex>> getVertices =
                    p => p.Select(e => e.Target);

                var paths = algo.ComputedShortestPaths
                    .Where(
                        p =>
                        {
                            var leftFirst = getVertices(p);
                            var left = leftFirst.Distinct();
                            var right = p;
                            return left.Count() == right.Count();
                        }).Select(p => p.Select(convertEdgeBack)).ToList();

                Paths = paths.SelectMany(e => e).Distinct().ToList();
            }
            catch (Exception)
            {
                Path = null;
                Paths = null;
//                throw;
            }
        }

        #endregion

        #region Nested type: FakeTableIndexEdge

        private class FakeTableIndexEdge : TableIndexEdge
        {
            #region  Fields

            public readonly TableIndexEdge e;

            #endregion

            #region  Constructors

            public FakeTableIndexEdge(TableIndexEdge e)
                : base(e.Target, e.Source, e.ForeignKey)
            {
                this.e = e;
            }

            #endregion
        }

        #endregion
    }
}