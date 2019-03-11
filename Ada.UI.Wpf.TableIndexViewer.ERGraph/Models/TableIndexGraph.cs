namespace Ada.UI.Wpf.TableIndexViewer.ERGraph.Models
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using QuickGraph;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public sealed class TableIndexGraph : BidirectionalGraph<TableIndexVertex, TableIndexEdge>, ISelectionDetector
    {
        #region  Constructors

        public TableIndexGraph()
        {
        }


        public TableIndexGraph(TableIndex tableIndex)
        {
            foreach (var table in tableIndex.Tables) AddVertex(new TableIndexVertex(table));

            var vlist = Vertices.ToList();

            foreach (var vertex in vlist)
            foreach (var edge in GetVertexEdge(vertex))
                AddEdge(edge);
        }

        #endregion

        #region ISelectionDetector Members

        public event Action SelectionChanged;

        public bool IsSelected(object o)
        {
            if (o is Table)
                foreach (var vertex in Vertices)
                    if (vertex.Table == o)
                        return vertex.Selected;
            var s = o as string;
            if (s != null)
                foreach (var vertex in Vertices)
                    if (vertex.Table.Name == s)
                        return vertex.Selected;
            return false;
        }

        #endregion

        #region

        public int AddTableAndEdges(Table table)
        {
            var newVertex = new TableIndexVertex(table);
            return AddTableAndEdges(newVertex);
        }

        public int AddTableAndEdges(TableIndexVertex newVertex)
        {
            var table = newVertex.Table;

            var newEdges = new List<TableIndexEdge>();

            foreach (var vertex in Vertices)
            {
                foreach (var foreignKey in vertex.Table.ForeignKeys)
                {
                    if (!string.Equals(table.Name, foreignKey.ReferencedTable)) continue;

                    var edgeData = new TableIndexEdge(vertex, newVertex, foreignKey);
                    newEdges.Add(edgeData);
                }

                foreach (var foreignKey in table.ForeignKeys)
                {
                    if (!string.Equals(vertex.Table.Name, foreignKey.ReferencedTable)) continue;

                    var edgeData = new TableIndexEdge(newVertex, vertex, foreignKey);
                    newEdges.Add(edgeData);
                }
            }

            var res = 0;

            if (!newEdges.Any())
                res = AddVertex(newVertex) ? 1 : 0;
            else
                res = AddVerticesAndEdgeRange(newEdges);
            return res;
        }

        public override bool AddVertex(TableIndexVertex v)
        {
            var ret = base.AddVertex(v);
            if (ret)
                v.SelectedChange += OnSelectionChanged;
            return ret;
        }

        public bool ContainsTable(Table table)
        {
            return GetVertex(table) != null;
        }

        public TableIndexVertex GetVertex(Table table)
        {
            return Vertices.FirstOrDefault(v => v.Table.Name == table.Name);
        }

        private IEnumerable<TableIndexEdge> GetVertexEdge(TableIndexVertex vertex)
        {
            foreach (var foreignKey in vertex.Table.ForeignKeys)
            {
                var tableIndexVertex = Vertices.FirstOrDefault(v => v.Table.Name == foreignKey.ReferencedTable);
                if (tableIndexVertex == null)
                    continue;
                var dataEdge = new TableIndexEdge(vertex, tableIndexVertex, foreignKey);

                yield return dataEdge;
            }
        }

        private void OnSelectionChanged()
        {
            SelectionChanged?.Invoke();
        }

        #endregion
    }
}