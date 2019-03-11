namespace Ada.UI.Wpf.TableIndexViewer.ERGraph.Controls
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using GraphX.Controls;
    using JetBrains.Annotations;
    using Models;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableVertexControl : VertexControl
    {
        #region  Fields

        [NotNull] private readonly Dictionary<string, int?> connectionDic;

        #endregion

        #region  Constructors

        public TableVertexControl(object vertexData, bool tracePositionChange = true, bool bindToDataObject = true)
            : base(vertexData, tracePositionChange, bindToDataObject)
        {
            var tableIndexVertex = vertexData as TableIndexVertex;
            connectionDic = tableIndexVertex?.Table.Columns.Select((c, i) => new {c.Name, i})
                .ToDictionary(p => p.Name, p => (int?) p.i + 1, EqualityComparer<string>.Default);

            connectionDic = connectionDic ?? new Dictionary<string, int?>();
        }

        #endregion

        #region

        public void AddConnectionPointFromColumn(StaticVertexConnectionPoint connectionPoint, Column col, bool? getIsRight)
        {
            var res = connectionDic.GetOrDefault(col.Name);

            if (!res.HasValue)
                return;

            res = res.Value * (getIsRight == true ? 1 : -1);
            connectionPoint.Id = res.Value;

            var existingConnectionPoint = VertexConnectionPointsList.FirstOrDefault(c => c.Id == res);
            if (existingConnectionPoint != null)
                VertexConnectionPointsList.Remove(existingConnectionPoint);
            VertexConnectionPointsList.Add(connectionPoint);
        }

        public int? GetConnectionPointFromName(string col)
        {
            var res = connectionDic.GetOrDefault(col);

            var result = VertexConnectionPointsList.FirstOrDefault(a => a.Id == res);
            if (result == null)
                return null;

            if ((result as Control)?.IsVisible == false)
                return null;

            return res;
        }

        public int? GetSourceVertexConnectionPointId(TableIndexEdge edge)
        {
            return GetConnectionPointFromName(edge.ForeignKey.References.FirstOrDefault()?.Column);
        }

        #endregion
    }
}