using NCDC.CommandParser.Command.DML;
using NCDC.CommandParser.Segment.DML.Item;
using NCDC.CommandParser.Segment.DML.Order.Item;
using NCDC.CommandParser.Segment.Generic.Table;
using NCDC.ShardingParser.MetaData;
using NCDC.ShardingParser.MetaData.Schema;
using NCDC.ShardingParser.Segment.Select.Groupby;
using NCDC.ShardingParser.Segment.Select.OrderBy;
using NCDC.ShardingParser.Segment.Select.Projection.Impl;
using NCDC.Basic.TableMetadataManagers;
using OpenConnector.Exceptions;
using OpenConnector.Extensions;

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
        private readonly ITableMetadataManager _tableMetadataManager;

        private readonly ProjectionEngine _projectionEngine;

        public ProjectionsContextEngine(ITableMetadataManager tableMetadataManager)
        {
            _tableMetadataManager = tableMetadataManager;
            _projectionEngine = new ProjectionEngine(tableMetadataManager);
        }
        /// <summary>
        /// Create projections context.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="selectCommand"></param>
        /// <param name="groupByContext"></param>
        /// <param name="orderByContext"></param>
        /// <returns></returns>
        public ProjectionsContext CreateProjectionsContext(string sql, SelectCommand selectCommand, GroupByContext groupByContext, OrderByContext orderByContext)
        {
            ProjectionsSegment projectionsSegment = selectCommand.Projections;
            ICollection<IProjection> projections = GetProjections(sql, selectCommand.GetSimpleTableSegments(), projectionsSegment);
            ProjectionsContext result = new ProjectionsContext(projectionsSegment.GetStartIndex(), projectionsSegment.GetStopIndex(), projectionsSegment.IsDistinctRow(), projections);
            result.GetProjections().AddAll(GetDerivedGroupByColumns(projections, groupByContext, selectCommand));
            result.GetProjections().AddAll(GetDerivedOrderByColumns(projections, orderByContext, selectCommand));
            return result;
        }

        private ICollection<IProjection> GetProjections(string sql, ICollection<SimpleTableSegment> tableSegments, ProjectionsSegment projectionsSegment)
        {
            List<IProjection> result = new List<IProjection>(projectionsSegment.GetProjections().Count());
            foreach (var projection in projectionsSegment.GetProjections())
            {
                var p = _projectionEngine.CreateProjection(sql, tableSegments, projection);
                if (p != null)
                {
                    result.Add(p);
                }
            }
            return result;
        }

        private ICollection<IProjection> GetDerivedGroupByColumns(ICollection<IProjection> projections, GroupByContext groupByContext, SelectCommand selectCommand)
        {
            return GetDerivedOrderColumns(projections, groupByContext.GetItems(), DerivedColumn.Get(DerivedColumnEnum.GROUP_BY_ALIAS), selectCommand);
        }

        private ICollection<IProjection> GetDerivedOrderByColumns(ICollection<IProjection> projections, OrderByContext orderByContext, SelectCommand selectCommand)
        {
            return GetDerivedOrderColumns(projections, orderByContext.GetItems(), DerivedColumn.Get(DerivedColumnEnum.ORDER_BY_ALIAS), selectCommand);
        }

        private ICollection<IProjection> GetDerivedOrderColumns(ICollection<IProjection> projections, ICollection<OrderByItem> orderItems, DerivedColumn derivedColumn, SelectCommand selectCommand)
        {
            if (orderItems.IsEmpty())
            {
                return new LinkedList<IProjection>();
            }
            ICollection<IProjection> result = new LinkedList<IProjection>();
            int derivedColumnOffset = 0;
            foreach (var orderItem in orderItems)
            {
                if (!ContainsProjection(projections, orderItem.GetSegment(), selectCommand))
                {
                    result.Add(new DerivedProjection(((TextOrderByItemSegment)orderItem.GetSegment()).GetText(), derivedColumn.GetDerivedColumnAlias(derivedColumnOffset++)));
                }
            }
            return result;
        }

        private bool ContainsProjection(ICollection<IProjection> projections, OrderByItemSegment orderByItemSegment, SelectCommand selectCommand)
        {
            return orderByItemSegment is IndexOrderByItemSegment
                || ContainsItemInShorthandProjection(projections, orderByItemSegment, selectCommand) || ContainsProjection(projections, orderByItemSegment);
        }

        private bool ContainsProjection(ICollection<IProjection> projections, OrderByItemSegment orderItem)
        {
            if (projections.IsNotEmpty() && orderItem is IndexOrderByItemSegment)
                return true;

            foreach (var projection in projections)
            {
                if (IsSameAlias(projection, (TextOrderByItemSegment)orderItem) || IsSameQualifiedName(projection, (TextOrderByItemSegment)orderItem))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsSameAlias(IProjection projection, TextOrderByItemSegment orderItem)
        {
            return projection.GetAlias() != null && (orderItem.GetText().Equals(projection.GetAlias(), StringComparison.OrdinalIgnoreCase) || orderItem.GetText().Equals(projection.GetExpression(), StringComparison.OrdinalIgnoreCase));
        }

        private bool IsSameQualifiedName(IProjection projection, TextOrderByItemSegment orderItem)
        {
            return projection.GetAlias() == null && projection.GetExpression().Equals(orderItem.GetText(), StringComparison.OrdinalIgnoreCase);
        }
        private bool ContainsItemInShorthandProjection(ICollection<IProjection> projections, OrderByItemSegment orderByItemSegment, SelectCommand selectCommand)
        {
            return IsUnqualifiedShorthandProjection(projections) || ContainsItemWithOwnerInShorthandProjections(projections, orderByItemSegment, selectCommand)
                    || ContainsItemWithoutOwnerInShorthandProjections(projections, orderByItemSegment, selectCommand);
        }

        private bool IsUnqualifiedShorthandProjection(ICollection<IProjection> projections)
        {
            if (1 != projections.Count)
            {
                return false;
            }
            IProjection projection = projections.First();
            return projection is ShorthandProjection shorthandProjection && shorthandProjection.GetOwner() == null;
        }

        private bool ContainsItemWithOwnerInShorthandProjections(ICollection<IProjection> projections, OrderByItemSegment orderItem, SelectCommand selectCommand)
        {
            return orderItem is ColumnOrderByItemSegment columnOrderByItemSegment && columnOrderByItemSegment.GetColumn().GetOwner() != null
                    && FindShorthandProjection(projections, columnOrderByItemSegment.GetColumn().GetOwner().GetIdentifier().GetValue(), selectCommand) != null;
        }

        private ShorthandProjection FindShorthandProjection(ICollection<IProjection> projections, string tableNameOrAlias, SelectCommand selectCommand)
        {
            SimpleTableSegment tableSegment = Find(tableNameOrAlias, selectCommand);
            foreach (var projection in projections)
            {
                if (!(projection is ShorthandProjection))
                {
                    continue;
                }
                ShorthandProjection shorthandProjection = (ShorthandProjection)projection;
                if (shorthandProjection.GetOwner() != null && Find(
                        shorthandProjection.GetOwner(), selectCommand).GetTableName().GetIdentifier().GetValue().Equals(tableSegment.GetTableName().GetIdentifier().GetValue(), StringComparison.OrdinalIgnoreCase))
                {
                    return shorthandProjection;
                }
            }
            return null;
        }

        private SimpleTableSegment Find(string tableNameOrAlias, SelectCommand selectCommand)
        {
            foreach (var simpleTableSegment in selectCommand.GetSimpleTableSegments())
            {
                if (tableNameOrAlias.Equals(simpleTableSegment.GetTableName().GetIdentifier().GetValue()) ||
                    tableNameOrAlias.Equals(simpleTableSegment.GetAlias()))
                {
                    return simpleTableSegment;
                }
            }
            throw new ShardingException("can not find owner from table.");
        }
        /// <summary>
        /// 判断order by的字段是否存在于select的projection里面
        /// </summary>
        /// <param name="projections"></param>
        /// <param name="orderItem"></param>
        /// <param name="selectCommand"></param>
        /// <returns></returns>
        private bool ContainsItemWithoutOwnerInShorthandProjections(ICollection<IProjection> projections, OrderByItemSegment orderItem, SelectCommand selectCommand)
        {
            if (orderItem is ColumnOrderByItemSegment columnOrderByItemSegment)
            {
                if (columnOrderByItemSegment.GetColumn().GetOwner() == null)
                {
                    foreach (var shorthandProjection in GetQualifiedShorthandProjections(projections))
                    {
                        if (IsSameProjection(shorthandProjection, (ColumnOrderByItemSegment)orderItem, selectCommand))
                        {
                            return true;
                        }
                    }
                }
            }


            return false;
        }
        private bool IsSameProjection(ShorthandProjection shorthandProjection, ColumnOrderByItemSegment orderItem, SelectCommand selectCommand)
        {
            if (shorthandProjection.GetOwner() == null)
                throw new ShardingException("shorthandProjection can not found owner");
            SimpleTableSegment tableSegment = Find(shorthandProjection.GetOwner(), selectCommand);
            return _tableMetadataManager.ContainsColumn(tableSegment.GetTableName().GetIdentifier().GetValue(),
                orderItem.GetColumn().GetIdentifier().GetValue());
        }

        private ICollection<ShorthandProjection> GetQualifiedShorthandProjections(ICollection<IProjection> projections)
        {
            ICollection<ShorthandProjection> result = new LinkedList<ShorthandProjection>();

            foreach (var projection in projections)
            {
                if (projection is ShorthandProjection shorthandProjection && shorthandProjection.GetOwner() != null)
                {
                    result.Add(shorthandProjection);
                }
            }

            return result;
        }

    }
}
