#region Header

// Author 
// Created 14

#endregion

namespace Ada.UI.Wpf.TableIndexViewer.ERGraph.Controls
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using GraphX;
    using GraphX.Controls;
    using GraphX.PCL.Common.Enums;
    using GraphX.PCL.Common.Exceptions;
    using GraphX.PCL.Common.Interfaces;
    using JetBrains.Annotations;
    using Models;
    using QuickGraph;
    using Point = GraphX.Measure.Point;
    using SysRect = System.Windows.Rect;

    #endregion

    public class TableEdgeControl : EdgeControl
    {
        #region  Constructors

        public TableEdgeControl()
        {
        }

        public TableEdgeControl
            (VertexControl source, VertexControl target, object edge, bool showLabels = false, bool showArrows = true)
            : base(source, target, edge, showLabels, showArrows)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets if this edge is self looped (have same Source and Target)
        /// </summary>
        public override bool IsSelfLooped
        {
            get => Source != null && Target != null && Source.Vertex == Target.Vertex &&
                   ((Edge as IGraphXCommonEdge)?.SourceConnectionPointId == null
                    || (Edge as IGraphXCommonEdge)?.TargetConnectionPointId == null);
            protected set => base.IsSelfLooped = value;
        }


        [CanBeNull]
        private GraphArea<TableIndexVertex, TableIndexEdge, BidirectionalGraph<TableIndexVertex, TableIndexEdge>>
            RootAreaAs
            => RootArea as
                GraphArea<TableIndexVertex, TableIndexEdge, BidirectionalGraph<TableIndexVertex, TableIndexEdge>>;

        #endregion

        #region

        // Stolen and modified from GraphX.Controls.EdgeControlBase  i.e. the bsae control itself
        public override void PrepareEdgePath(bool useCurrentCoords = false, Point[] externalRoutingPoints = null, bool updateLabel = true)
        {
            //do not calculate invisible edges
            if (Visibility != Visibility.Visible && !IsHiddenEdgesUpdated && Source == null || Target == null || ManualDrawing || !IsTemplateLoaded) return;

            #region Get the inputs

            //get the size of the source
            var sourceSize = new Size
            {
                Width = Source.ActualWidth,
                Height = Source.ActualHeight
            };
            if (CustomHelper.IsInDesignMode(this)) sourceSize = new Size(80, 20);

            //get the position center of the source
            var sourcePos = new System.Windows.Point
            {
                X = (useCurrentCoords ? GraphAreaBase.GetX(Source) : GraphAreaBase.GetFinalX(Source)) + sourceSize.Width * .5,
                Y = (useCurrentCoords ? GraphAreaBase.GetY(Source) : GraphAreaBase.GetFinalY(Source)) + sourceSize.Height * .5
            };

            //get the size of the target
            var targetSize = new Size
            {
                Width = Target.ActualWidth,
                Height = Target.ActualHeight
            };
            if (CustomHelper.IsInDesignMode(this))
                targetSize = new Size(80, 20);

            //get the position center of the target
            var targetPos = new System.Windows.Point
            {
                X = (useCurrentCoords ? GraphAreaBase.GetX(Target) : GraphAreaBase.GetFinalX(Target)) + targetSize.Width * .5,
                Y = (useCurrentCoords ? GraphAreaBase.GetY(Target) : GraphAreaBase.GetFinalY(Target)) + targetSize.Height * .5
            };

            var routedEdge = Edge as IRoutingInfo;
            if (routedEdge == null)
                throw new GX_InvalidDataException("Edge must implement IRoutingInfo interface");

            //get the route informations
            var routeInformation = externalRoutingPoints ?? routedEdge.RoutingPoints;

            // Get the TopLeft position of the Source Vertex.
            var sourcePos1 = new System.Windows.Point
            {
                X = useCurrentCoords ? GraphAreaBase.GetX(Source) : GraphAreaBase.GetFinalX(Source),
                Y = useCurrentCoords ? GraphAreaBase.GetY(Source) : GraphAreaBase.GetFinalY(Source)
            };
            // Get the TopLeft position of the Target Vertex.
            var targetPos1 = new System.Windows.Point
            {
                X = useCurrentCoords ? GraphAreaBase.GetX(Target) : GraphAreaBase.GetFinalX(Target),
                Y = useCurrentCoords ? GraphAreaBase.GetY(Target) : GraphAreaBase.GetFinalY(Target)
            };

            var hasEpSource = EdgePointerForSource != null;
            var hasEpTarget = EdgePointerForTarget != null;

            #endregion

            //check if we have some edge route data
            var hasRouteInfo = routeInformation != null && routeInformation.Length > 1;


            /* Rectangular shapes implementation by bleibold */

            var gEdge = Edge as IGraphXCommonEdge;
            System.Windows.Point p1;
            System.Windows.Point p2;


            //if self looped edge
            if (IsSelfLooped)
            {
                PrepareSelfLoopedEdge(sourcePos1);

                Linegeometry = new PathGeometry();

                GeometryHelper.TryFreeze(Linegeometry);
                // might want to move this to a less called code place
                // or only call on change?
                UpdateSelfLoopedEdgeData();
                return;
            }

            UpdateSelfLoopedEdgeData();

            //calculate edge source (p1) and target (p2) endpoints based on different settings
            if (gEdge?.SourceConnectionPointId != null)
            {
                var sourceCp = Source.GetConnectionPointById(gEdge.SourceConnectionPointId.Value, true);
                if (sourceCp == null)
                    throw new GX_ObjectNotFoundException(string.Format("Can't find source vertex VCP by edge source connection point Id({1}) : {0}", Source, gEdge.SourceConnectionPointId));
                if (sourceCp.Shape == VertexShape.None)
                {
                    p1 = sourceCp.RectangularSize.Center();
                }
                else
                {
                    var targetCpPos = gEdge.TargetConnectionPointId.HasValue ? Target.GetConnectionPointById(gEdge.TargetConnectionPointId.Value, true).RectangularSize.Center() : hasRouteInfo ? routeInformation[1].ToWindows() : targetPos;
                    p1 = GeometryHelper.GetEdgeEndpoint(sourceCp.RectangularSize.Center(), sourceCp.RectangularSize, targetCpPos, sourceCp.Shape);
                }
            }
            else
            {
                p1 = GeometryHelper.GetEdgeEndpoint(sourcePos, new SysRect(sourcePos1, sourceSize), hasRouteInfo ? routeInformation[1].ToWindows() : targetPos, Source.VertexShape);
            }

            if (gEdge?.TargetConnectionPointId != null)
            {
                var targetCp = Target.GetConnectionPointById(gEdge.TargetConnectionPointId.Value, true);
                if (targetCp == null)
                    throw new GX_ObjectNotFoundException(string.Format("Can't find target vertex VCP by edge target connection point Id({1}) : {0}", Target, gEdge.TargetConnectionPointId));
                if (targetCp.Shape == VertexShape.None)
                {
                    p2 = targetCp.RectangularSize.Center();
                }
                else
                {
                    var sourceCpPos = gEdge.SourceConnectionPointId.HasValue ? Source.GetConnectionPointById(gEdge.SourceConnectionPointId.Value, true).RectangularSize.Center() : hasRouteInfo ? routeInformation[routeInformation.Length - 2].ToWindows() : sourcePos;
                    p2 = GeometryHelper.GetEdgeEndpoint(targetCp.RectangularSize.Center(), targetCp.RectangularSize, sourceCpPos, targetCp.Shape);
                }
            }
            else
            {
                p2 = GeometryHelper.GetEdgeEndpoint(targetPos, new SysRect(targetPos1, targetSize), hasRouteInfo ? routeInformation[routeInformation.Length - 2].ToWindows() : sourcePos, Target.VertexShape);
            }

            SourceConnectionPoint = p1;
            TargetConnectionPoint = p2;

            Linegeometry = new PathGeometry();
            PathFigure lineFigure;

            List<System.Windows.Point> routePoints;
            //if we have route and route consist of 2 or more points
            if (RootArea != null && hasRouteInfo)
            {
                routePoints = routeInformation.ToWindows().ToList();

                //replace start and end points with accurate ones
                routePoints.Remove(routePoints.First());
                routePoints.Remove(routePoints.Last());
                routePoints.Insert(0, p1);
                routePoints.Add(p2);
            }
            else

            {
                routePoints = new List<System.Windows.Point> {p1, p2};
            }


            // MAJOR CHANGE from original
            // Add a bit of path to and from the connection points
            if (gEdge?.SourceConnectionPointId != null) routePoints.Insert(0, p1 + new Vector((gEdge.SourceConnectionPointId > 0 ? 1 : -1) * 10, 0));
            if (gEdge?.TargetConnectionPointId != null) routePoints.Add(p2 + new Vector((gEdge.TargetConnectionPointId > 0 ? 1 : -1) * 10, 0));


            if (externalRoutingPoints == null && routedEdge.RoutingPoints != null)
                routedEdge.RoutingPoints = routePoints.ToArray().ToGraphX();

            if (RootAreaAs?.LogicCore?.EdgeCurvingEnabled ?? false)
            {
                var oPolyLineSegment = GeometryHelper.GetCurveThroughPoints(routePoints.ToArray(), 0.5, RootAreaAs?.LogicCore?.EdgeCurvingTolerance ?? 0);

                if (hasEpSource && oPolyLineSegment.Points.Count > 1) UpdateSourceEpData(routePoints[0], oPolyLineSegment.Points[1]);
                if (hasEpTarget && oPolyLineSegment.Points.Count > 1) UpdateTargetEpData(oPolyLineSegment.Points[oPolyLineSegment.Points.Count - 1], oPolyLineSegment.Points[oPolyLineSegment.Points.Count - 2]);

                lineFigure = GeometryHelper.GetPathFigureFromPathSegments(routePoints[0], true, true, oPolyLineSegment);

                //freeze and create resulting geometry
                GeometryHelper.TryFreeze(oPolyLineSegment);
            }
            else
            {
                // Not good given hack to have small vertical lines
                if (hasEpSource)
                    UpdateSourceEpData(routePoints[0], routePoints[1]);
                if (hasEpTarget)
                    UpdateTargetEpData(routePoints[routePoints.Count - 1], routePoints[routePoints.Count - 2]);

                // Reverse the path if specified.
                if (gEdge.ReversePath)
                    routePoints.Reverse();

                var pcol = new PointCollection();
                routePoints.ForEach(a => pcol.Add(a));

                lineFigure = new PathFigure {StartPoint = routePoints[0], Segments = new PathSegmentCollection {new PolyLineSegment {Points = pcol}}, IsClosed = false};
            }

            ((PathGeometry) Linegeometry).Figures.Add(lineFigure);

            GeometryHelper.TryFreeze(lineFigure);
            GeometryHelper.TryFreeze(Linegeometry);

            if (ShowLabel && EdgeLabelControl != null && UpdateLabelPosition && updateLabel)
                EdgeLabelControl.UpdatePosition();
        }

        public override void UpdateEdge(bool updateLabel = true)
        {
            // updating connectionpoints
            var tableIndexEdge = Edge as TableIndexEdge;

            if (tableIndexEdge != null && Source != null && Target != null)
            {
                var sourceColumnName = tableIndexEdge.ForeignKey.References.FirstOrDefault()?.Column;
                var sId = (Source as TableVertexControl)?.GetConnectionPointFromName(sourceColumnName);
                if (Source.GetPosition().X + Source.ActualWidth / 2 < Target.GetPosition().X)
                    tableIndexEdge.SourceConnectionPointId = -sId;
                else
                    tableIndexEdge.SourceConnectionPointId = sId;

                var targetColumnName = tableIndexEdge.ForeignKey.References.FirstOrDefault()?.Referenced;
                var tId = (Target as TableVertexControl)?.GetConnectionPointFromName(targetColumnName);
                if (Target.GetPosition().X + Target.ActualWidth / 2 < Source.GetPosition().X)
                    tableIndexEdge.TargetConnectionPointId = -tId;
                else
                    tableIndexEdge.TargetConnectionPointId = tId;
            }

            base.UpdateEdge(updateLabel);
        }


        private System.Windows.Point UpdateSourceEpData(System.Windows.Point from, System.Windows.Point to, bool remainHidden = false)
        {
            var dir = MathHelper.GetDirection(from, to);
            if (from == to)
            {
                if (HideEdgePointerOnVertexOverlap) EdgePointerForSource.Hide();
                else dir = new Vector(0, 0);
            }
            else if (!remainHidden)
            {
                EdgePointerForSource.Show();
            }

            var result = EdgePointerForSource.Update(from, dir, EdgePointerForSource.NeedRotation ? -MathHelper.GetAngleBetweenPoints(from, to).ToDegrees() : 0);
            return EdgePointerForSource.Visibility == Visibility.Visible ? result : new System.Windows.Point();
        }

        private System.Windows.Point UpdateTargetEpData(System.Windows.Point from, System.Windows.Point to, bool remainHidden = false)
        {
            var dir = MathHelper.GetDirection(from, to);
            if (from == to)
            {
                if (HideEdgePointerOnVertexOverlap) EdgePointerForTarget.Hide();
                else dir = new Vector(0, 0);
            }
            else if (!remainHidden)
            {
                EdgePointerForTarget.Show();
            }

            var result = EdgePointerForTarget.Update(from, dir, EdgePointerForTarget.NeedRotation ? -MathHelper.GetAngleBetweenPoints(from, to).ToDegrees() : 0);
            return EdgePointerForTarget.Visibility == Visibility.Visible ? result : new System.Windows.Point();
        }

        #endregion
    }
}