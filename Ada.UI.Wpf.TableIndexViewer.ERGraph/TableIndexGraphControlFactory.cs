#region Header

// Author 
// Created 10

#endregion

namespace Ada.UI.Wpf.TableIndexViewer.ERGraph
{
    #region Namespace Using

    using System.Linq;
    using System.Windows;
    using Controls;
    using GraphX;
    using GraphX.Controls;
    using GraphX.Controls.Models;
    using Models;

    #endregion

    public class TableIndexGraphControlFactory : GraphControlFactory
    {
        #region  Constructors

        public TableIndexGraphControlFactory(GraphAreaBase graphArea) : base(graphArea)
        {
        }

        #endregion

        #region

        public override EdgeControl CreateEdgeControl(VertexControl source, VertexControl target, object edge, bool showLabels = false, bool showArrows = true, Visibility visibility = Visibility.Visible)
        {
            var edgectrl = new TableEdgeControl(source, target, edge, showLabels, showArrows) {RootArea = FactoryRootArea};
            edgectrl.SetCurrentValue(UIElement.VisibilityProperty, visibility);


            var tableIndexEdge = edge as TableIndexEdge;
            if (tableIndexEdge != null)
            {
                var sourceTableVertexControl = source as TableVertexControl;
                if (sourceTableVertexControl != null)
                {
                    var sourceColumnName = tableIndexEdge.ForeignKey.References.FirstOrDefault()?.Column;
                    tableIndexEdge.SourceConnectionPointId =
                        sourceTableVertexControl.GetConnectionPointFromName(
                            sourceColumnName);
                }

                var targetTableVertexControl = target as TableVertexControl;
                if (targetTableVertexControl != null)
                {
                    var targetColumnName = tableIndexEdge.ForeignKey.References.FirstOrDefault()?.Referenced;
                    tableIndexEdge.TargetConnectionPointId = targetTableVertexControl.GetConnectionPointFromName(
                        targetColumnName);
                }
            }


            return edgectrl;
        }

        public override VertexControl CreateVertexControl(object vertexData)
        {
            return new TableVertexControl(vertexData) {RootArea = FactoryRootArea};
        }

        #endregion
    }
}