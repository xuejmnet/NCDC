using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Predicate;
using NCDC.CommandParser.Common.Segment.DML.Item;
using NCDC.CommandParser.Common.Segment.DML.Order.Item;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.ShardingParser.Segment.Select.Groupby;
using NCDC.ShardingParser.Segment.Select.Groupby.Engine;
using NCDC.ShardingParser.Segment.Select.OrderBy;
using NCDC.ShardingParser.Segment.Select.OrderBy.Engine;
using NCDC.ShardingParser.Segment.Select.Pagination;
using NCDC.ShardingParser.Segment.Select.Pagination.Engine;
using NCDC.ShardingParser.Segment.Select.Projection;
using NCDC.ShardingParser.Segment.Select.Projection.Engine;
using NCDC.ShardingParser.Segment.Table;
using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Common.Util;
using NCDC.Exceptions;
using NCDC.Extensions;
using NCDC.Extensions;
using NCDC.ShardingAdoNet;

namespace NCDC.ShardingParser.Command.DML
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 9:43:59
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SelectCommandContext : GenericSqlCommandContext<SelectCommand>, ITableAvailable, IWhereAvailable
    {
        private readonly TablesContext _tablesContext;

        private readonly ProjectionsContext _projectionsContext;

        private readonly GroupByContext _groupByContext;

        private readonly OrderByContext _orderByContext;

        private readonly PaginationContext _paginationContext;

        private readonly bool _containsSubQuery;

        public SelectCommandContext(ITableMetadataManager tableMetadataManager, string sql, ParameterContext parameterContext, SelectCommand sqlCommand) : base(sqlCommand)
        {
            _tablesContext = new TablesContext(sqlCommand.GetSimpleTableSegments());
            _groupByContext = new GroupByContextEngine().CreateGroupByContext(sqlCommand);
            _orderByContext = new OrderByContextEngine().CreateOrderBy(sqlCommand, _groupByContext);
            _projectionsContext = new ProjectionsContextEngine(tableMetadataManager).CreateProjectionsContext(sql, sqlCommand, _groupByContext, _orderByContext);
            _paginationContext = new PaginationContextEngine().CreatePaginationContext(sqlCommand, _projectionsContext, parameterContext);
            _containsSubQuery = ContainsSubQuery();
        }
        private bool ContainsSubQuery()
        {
            // FIXME process subquery
            //        Collection<SubqueryPredicateSegment> subqueryPredicateSegments = getSqlStatement().findSQLSegments(SubqueryPredicateSegment.class);
            //        for (SubqueryPredicateSegment each : subqueryPredicateSegments) {
            //            if (!each.getAndPredicates().isEmpty()) {
            //                return true;
            //            }
            //        }
            return false;
        }

        /**
         * Set indexes.
         *
         * @param columnLabelIndexMap map for column label and index
         */
        public void SetIndexes(IDictionary<string, int> columnLabelIndexMap)
        {
            SetIndexForAggregationProjection(columnLabelIndexMap);
            SetIndexForOrderItem(columnLabelIndexMap, _orderByContext.GetItems());
            SetIndexForOrderItem(columnLabelIndexMap, _groupByContext.GetItems());
        }

        private void SetIndexForAggregationProjection(IDictionary<string, int> columnLabelIndexMap)
        {

            foreach (var aggregationProjection in _projectionsContext.GetAggregationProjections())
            {
                if (!columnLabelIndexMap.ContainsKey(aggregationProjection.GetColumnLabel()))
                    throw new ShardingException(
                        $"Can't find index: {aggregationProjection}, please add alias for aggregate selections");

                aggregationProjection.SetIndex(columnLabelIndexMap[aggregationProjection.GetColumnLabel()]);

                foreach (var derived in aggregationProjection.GetDerivedAggregationProjections())
                {
                    if (!columnLabelIndexMap.ContainsKey(derived.GetColumnLabel()))
                        throw new ShardingException($"Can't find index: {derived}");
                    derived.SetIndex(columnLabelIndexMap[derived.GetColumnLabel()]);
                }
            }
        }

        private void SetIndexForOrderItem(IDictionary<string, int> columnLabelIndexMap, ICollection<OrderByItem> orderByItems)
        {
            foreach (var orderByItem in orderByItems)
            {
                var orderByItemSegment = orderByItem.GetSegment();

                if (orderByItemSegment is IndexOrderByItemSegment indexOrderByItemSegment)
                {
                    orderByItem.SetIndex(indexOrderByItemSegment.GetColumnIndex());
                    continue;
                }

                if (orderByItemSegment is ColumnOrderByItemSegment columnOrderByItemSegment && columnOrderByItemSegment.GetColumn().GetOwner() != null)
                {
                    var itemIndex = _projectionsContext.FindProjectionIndex(columnOrderByItemSegment.GetText());
                    if (itemIndex.HasValue)
                    {
                        orderByItem.SetIndex(itemIndex.Value);
                        continue;
                    }
                }

                var columnLabel = GetAlias(((TextOrderByItemSegment)orderByItem.GetSegment()).GetText());
                if (columnLabel == null)
                {
                    columnLabel = GetOrderItemText((TextOrderByItemSegment)orderByItem.GetSegment());
                }

                if (!columnLabelIndexMap.ContainsKey(columnLabel))
                {
                    throw new ShardingException($"Can't find index: {orderByItem}");
                }
                if (columnLabelIndexMap.ContainsKey(columnLabel))
                {
                    orderByItem.SetIndex(columnLabelIndexMap[columnLabel]);
                }
            }
        }

        private string GetAlias(string name)
        {
            if (_projectionsContext.IsUnqualifiedShorthandProjection())
            {
                return null;
            }
            string rawName = SqlUtil.GetExactlyValue(name);
            foreach (var projection in _projectionsContext.GetProjections())
            {
                if (SqlUtil.GetExactlyExpression(rawName).EqualsIgnoreCase(SqlUtil.GetExactlyExpression(SqlUtil.GetExactlyValue(projection.GetExpression()))))
                {
                    return projection.GetAlias();
                }
                if (rawName.EqualsIgnoreCase(projection.GetAlias()))
                {
                    return rawName;
                }
            }

            return null;
        }

        private String GetOrderItemText(TextOrderByItemSegment orderByItemSegment)
        {
            if (orderByItemSegment is ColumnOrderByItemSegment columnOrderByItemSegment)
            {
                return columnOrderByItemSegment.GetColumn().GetIdentifier().GetValue();
            }
            return ((ExpressionOrderByItemSegment)orderByItemSegment).GetExpression();
        }

        /// <summary>
        /// order by是否和group by一致
        /// 一致就说明可以stream merge
        /// </summary>
        /// <returns></returns>
        public bool IsSameGroupByAndOrderByItems()
        {
            return _groupByContext.GetItems().Any() && _groupByContext.GetItems().SequenceEqual(_orderByContext.GetItems());
        }

        public ICollection<SimpleTableSegment> GetAllTables()
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>(GetSqlCommand().GetSimpleTableSegments());
            if (GetSqlCommand().Where != null)
            {
                result.AddAll(GetAllTablesFromWhere(GetSqlCommand().Where!));
            }
            result.AddAll(GetAllTablesFromProjections(GetSqlCommand().Projections));
            if (GetSqlCommand().GroupBy!=null)
            {
                result.AddAll(GetAllTablesFromOrderByItems(GetSqlCommand().GroupBy!.GetGroupByItems()));
            }
            if (GetSqlCommand().OrderBy!=null)
            {
                result.AddAll(GetAllTablesFromOrderByItems(GetSqlCommand().OrderBy!.GetOrderByItems()));
            }
            return result;
        }

        private ICollection<SimpleTableSegment> GetAllTablesFromWhere(WhereSegment where)
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>();
            foreach (var andPredicate in where.GetAndPredicates())
            {
                foreach (var predicate in andPredicate.GetPredicates())
                {
                    result.AddAll(new PredicateExtractor(GetSqlCommand().GetSimpleTableSegments(), predicate)
                        .ExtractTables());
                }
            }
            return result;
        }

        private ICollection<SimpleTableSegment> GetAllTablesFromProjections(ProjectionsSegment projections)
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>();
            foreach (var projection in projections.GetProjections())
            {
                var table = GetTableSegment(projection);
                if (table != null)
                    result.Add(table);
            }
            return result;
        }

        private SimpleTableSegment GetTableSegment(IProjectionSegment projection)
        {
            var owner = GetTableOwner(projection);
            if (owner != null && IsTable(owner, GetSqlCommand().GetSimpleTableSegments()))
            {
                return new SimpleTableSegment(owner.GetStartIndex(), owner.GetStopIndex(), owner.GetIdentifier());
            }
            return null;
        }

        private OwnerSegment GetTableOwner(IProjectionSegment projection)
        {
            if (projection is IOwnerAvailable ownerAvailable)
            {
                return ownerAvailable.GetOwner();
            }
            if (projection is ColumnProjectionSegment columnProjectionSegment)
            {
                return columnProjectionSegment.GetColumn().GetOwner();
            }
            return null;
        }

        private ICollection<SimpleTableSegment> GetAllTablesFromOrderByItems(ICollection<OrderByItemSegment> orderByItems)
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>();
            foreach (var orderByItem in orderByItems)
            {
                if (orderByItem is ColumnOrderByItemSegment columnOrderByItemSegment)
                {
                    var owner = columnOrderByItemSegment.GetColumn().GetOwner();
                    if (owner != null && IsTable(owner, GetSqlCommand().GetSimpleTableSegments()))
                    {
                        if (columnOrderByItemSegment.GetColumn().GetOwner() == null)
                            throw new ShardingException("cant found column order by item owner");
                        var segment = columnOrderByItemSegment.GetColumn().GetOwner();
                        result.Add(new SimpleTableSegment(segment.GetStartIndex(), segment.GetStopIndex(), segment.GetIdentifier()));
                    }
                }

            }
            return result;
        }

        private bool IsTable(OwnerSegment owner, ICollection<SimpleTableSegment> tables)
        {
            var value = owner.GetIdentifier().GetValue();
            return !tables.Any(table => value.Equals(table.GetAlias()));
        }

        public WhereSegment GetWhere()
        {
            return GetSqlCommand().Where;
        }

        public ProjectionsContext GetProjectionsContext()
        {
            return _projectionsContext;
        }
        public GroupByContext GetGroupByContext()
        {
            return _groupByContext;
        }

        public PaginationContext GetPaginationContext()
        {
            return _paginationContext;
        }
        public OrderByContext GetOrderByContext()
        {
            return _orderByContext;
        }
        public bool IsContainsSubQuery()
        {
            return _containsSubQuery;
        }
        public override TablesContext GetTablesContext()
        {
            return _tablesContext;
        }

    }
}
