using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Item;
using NCDC.CommandParser.Common.Segment.DML.Order.Item;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.ShardingParser.Segment.Select.Groupby;
using NCDC.ShardingParser.Segment.Select.OrderBy;
using NCDC.ShardingParser.Segment.Select.Projection.Impl;
using NCDC.Exceptions;
using NCDC.Extensions;
using NCDC.ShardingParser.MetaData;

namespace NCDC.ShardingParser.Segment.Select.Projection.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 9:18:53
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ProjectionsContextEngine
    {

        private readonly ProjectionEngine _projectionEngine;

        public ProjectionsContextEngine()
        {
            _projectionEngine = new ProjectionEngine();
        }
        public ProjectionsContext CreateProjectionsContext(ITableSegment table, ProjectionsSegment? projectionsSegment,
            GroupByContext groupByContext, OrderByContext orderByContext)
        {
            if (projectionsSegment != null)
            {
                List<IProjection> projections =GetProjections(table, projectionsSegment).ToList();
                ProjectionsContext result = new ProjectionsContext(projectionsSegment.StartIndex,
                    projectionsSegment.StopIndex, projectionsSegment.DistinctRow, projections);
                result.GetProjections().AddAll(GetDerivedGroupByColumns(projections, groupByContext));
                result.GetProjections().AddAll(GetDerivedOrderByColumns(projections, orderByContext));
                return result;
            }
            else
            {
                return new ProjectionsContext();
            }
        }
        private IEnumerable<IProjection> GetProjections(ITableSegment table,  ProjectionsSegment projectionsSegment) {
            
                foreach (var projectionsSegmentProjection in projectionsSegment.Projections)
                {
                    var projection = _projectionEngine.CreateProjection(table,projectionsSegmentProjection);
                    if (projection is not null)
                    {
                        yield return projection;
                    }
                }
        }
        // public ProjectionsContext CreateProjectionsContext(string sql, SelectCommand selectCommand,
        //     GroupByContext groupByContext, OrderByContext orderByContext)
        // {
        //     ProjectionsSegment projectionsSegment = selectCommand.Projections;
        //     ICollection<IProjection> projections =
        //         GetProjections(sql, selectCommand.GetSimpleTableSegments(), projectionsSegment);
        //     ProjectionsContext result = new ProjectionsContext(projectionsSegment.GetStartIndex(),
        //         projectionsSegment.GetStopIndex(), projectionsSegment.IsDistinctRow(), projections);
        //     result.GetProjections().AddAll(GetDerivedGroupByColumns(projections, groupByContext, selectCommand));
        //     result.GetProjections().AddAll(GetDerivedOrderByColumns(projections, orderByContext, selectCommand));
        //     return result;
        // }

        // private ICollection<IProjection> GetProjections(string sql, ICollection<SimpleTableSegment> tableSegments,
        //     ProjectionsSegment projectionsSegment)
        // {
        //     List<IProjection> result = new List<IProjection>(projectionsSegment.GetProjections().Count());
        //     foreach (var projection in projectionsSegment.GetProjections())
        //     {
        //         var p = _projectionEngine.CreateProjection(sql, tableSegments, projection);
        //         if (p != null)
        //         {
        //             result.Add(p);
        //         }
        //     }
        //
        //     return result;
        // }

        private ICollection<IProjection> GetDerivedGroupByColumns(ICollection<IProjection> projections,
            GroupByContext groupByContext)
        {
            return GetDerivedOrderColumns(groupByContext.GetItems(),
                DerivedColumn.Get(DerivedColumnEnum.GROUP_BY_ALIAS), projections);
        }

        private ICollection<IProjection> GetDerivedOrderByColumns(ICollection<IProjection> projections,
            OrderByContext orderByContext)
        {
            return GetDerivedOrderColumns(orderByContext.GetItems(),
                DerivedColumn.Get(DerivedColumnEnum.ORDER_BY_ALIAS), projections);
        }

        private ICollection<IProjection> GetDerivedOrderColumns(ICollection<OrderByItem> orderItems, DerivedColumn derivedColumn,ICollection<IProjection> projections)
        {
            ICollection<IProjection> result = new LinkedList<IProjection>();
            int derivedColumnOffset = 0;
            foreach (var orderItem in orderItems)
            {
                if (!ContainsProjection(projections, orderItem.GetSegment()))
                {
                    result.Add(new DerivedProjection(((TextOrderByItemSegment)orderItem.GetSegment()).GetExpression(),
                        derivedColumn.GetDerivedColumnAlias(derivedColumnOffset++)));
                }
            }

            return result;
        }

        private bool ContainsProjection(ICollection<IProjection> projections, OrderByItemSegment orderItem)
        {
            if (projections.IsNotEmpty() && orderItem is IndexOrderByItemSegment)
                return true;

            foreach (var projection in projections)
            {
                if (IsSameAlias(projection, (TextOrderByItemSegment)orderItem) ||
                    IsSameQualifiedName(projection, (TextOrderByItemSegment)orderItem))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsSameAlias(IProjection projection, TextOrderByItemSegment orderItem)
        {
            return projection.GetAlias() != null &&
                   (orderItem.GetExpression().Equals(projection.GetAlias(), StringComparison.OrdinalIgnoreCase) || orderItem
                       .GetExpression().Equals(projection.GetExpression(), StringComparison.OrdinalIgnoreCase));
        }

        private bool IsSameQualifiedName(IProjection projection, TextOrderByItemSegment orderItem)
        {
            return projection.GetAlias() == null && projection.GetExpression()
                .Equals(orderItem.GetExpression(), StringComparison.OrdinalIgnoreCase);
        }
    }
}