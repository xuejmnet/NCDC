using System;
using System.Collections.Generic;
using ShardingConnector.Api.Database.DatabaseType;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Constant;
using ShardingConnector.CommandParser.Segment.DML.Order.Item;
using ShardingConnector.CommandParser.Util;
using ShardingConnector.Executor;
using ShardingConnector.Extensions;
using ShardingConnector.Merge.Engine.Merger;
using ShardingConnector.Merge.Reader;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.ParserBinder.Segment.Select.OrderBy;
using ShardingConnector.ShardingMerge.DQL.GroupBy;
using ShardingConnector.ShardingMerge.DQL.Iterator;
using ShardingConnector.ShardingMerge.DQL.OrderBy;
using ShardingConnector.ShardingMerge.DQL.Pagination;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.ShardingMerge.DQL
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Friday, 07 May 2021 22:26:43
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingDQLEnumeratorMerger : IResultMerger
    {
        private readonly IDatabaseType databaseType;

        public ShardingDQLEnumeratorMerger(IDatabaseType databaseType)
        {
            this.databaseType = databaseType;
        }

        public IMergedEnumerator Merge(List<IQueryEnumerator> queryEnumerators, ISqlCommandContext<ISqlCommand> sqlCommandContext, SchemaMetaData schemaMetaData)
        {
            if (1 == queryEnumerators.Count)
            {
                return new IteratorStreamMergedEnumerator(queryEnumerators);
            }
            IDictionary<string, int> columnLabelIndexMap = GetColumnLabelIndexMap(queryEnumerators[0]);
            var selectCommandContext = (SelectCommandContext)sqlCommandContext;
            selectCommandContext.SetIndexes(columnLabelIndexMap);
            var mergedEnumerator = Build(queryEnumerators, selectCommandContext, columnLabelIndexMap, schemaMetaData);
            return Decorate(queryEnumerators, selectCommandContext, mergedEnumerator);
        }

        private IDictionary<string, int> GetColumnLabelIndexMap(IQueryEnumerator queryResult)
        {
            IDictionary<string, int> result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = queryResult.ColumnCount-1; i >= 0; i--)
            {
                result.Add(SqlUtil.GetExactlyValue(queryResult.GetColumnLabel(i)), i);
            }
            return result;
        }

        private IMergedEnumerator Build(List<IQueryEnumerator> queryEnumerators, SelectCommandContext selectCommandContext,
                                    IDictionary<string, int> columnLabelIndexMap, SchemaMetaData schemaMetaData)
        {
            if (IsNeedProcessGroupBy(selectCommandContext))
            {
                return GetGroupByMergedResult(queryEnumerators, selectCommandContext, columnLabelIndexMap, schemaMetaData);
            }
            if (IsNeedProcessDistinctRow(selectCommandContext))
            {
                SetGroupByForDistinctRow(selectCommandContext);
                return GetGroupByMergedResult(queryEnumerators, selectCommandContext, columnLabelIndexMap, schemaMetaData);
            }
            if (IsNeedProcessOrderBy(selectCommandContext))
            {
                return new OrderByStreamMergedEnumerator(queryEnumerators, selectCommandContext, schemaMetaData);
            }
            return new IteratorStreamMergedEnumerator(queryEnumerators);
        }

        private bool IsNeedProcessGroupBy(SelectCommandContext selectCommandContext)
        {
            return !selectCommandContext.GetGroupByContext().GetItems().IsEmpty() || !selectCommandContext.GetProjectionsContext().GetAggregationProjections().IsEmpty();
        }

        private bool IsNeedProcessDistinctRow(SelectCommandContext selectCommandContext)
        {
            return selectCommandContext.GetProjectionsContext().IsDistinctRow();
        }

        private void SetGroupByForDistinctRow(SelectCommandContext selectCommandContext)
        {
            for (int index = 0; index < selectCommandContext.GetProjectionsContext().GetExpandProjections().Count; index++)
            {
                OrderByItem orderByItem = new OrderByItem(new IndexOrderByItemSegment(-1, -1, index, OrderDirectionEnum.ASC, OrderDirectionEnum.ASC));
                orderByItem.SetIndex(index);
                selectCommandContext.GetGroupByContext().GetItems().Add(orderByItem);
            }
        }

        private IMergedEnumerator GetGroupByMergedResult(List<IQueryEnumerator> queryResults, SelectCommandContext selectCommandContext,
                                                     IDictionary<string, int> columnLabelIndexMap, SchemaMetaData schemaMetaData)
        {
            if (selectCommandContext.IsSameGroupByAndOrderByItems())
            {
                return new GroupByStreamMergedEnumerator(columnLabelIndexMap, queryResults, selectCommandContext,
                    schemaMetaData);
            }
            return new GroupByMemoryMergedEnumerator(queryResults, selectCommandContext, schemaMetaData);
        }

        private bool IsNeedProcessOrderBy(SelectCommandContext selectCommandContext)
        {
            return !selectCommandContext.GetOrderByContext().GetItems().IsEmpty();
        }

        private IMergedEnumerator Decorate(List<IQueryEnumerator> queryResults, SelectCommandContext selectCommandContext, IMergedEnumerator mergedResult)
        {
            var paginationContext = selectCommandContext.GetPaginationContext();
            if (!paginationContext.HasPagination() || 1 == queryResults.Count)
            {
                return mergedResult;
            }
            String trunkDatabaseName = DatabaseTypes.GetTrunkDatabaseType(databaseType.GetName()).GetName();
            if ("MySQL".Equals(trunkDatabaseName) || "PostgreSQL".Equals(trunkDatabaseName))
            {
                return new LimitDecoratorMergedEnumerator(mergedResult, paginationContext);
            }
            if ("Oracle".Equals(trunkDatabaseName))
            {
                return new RowNumberDecoratorMergedEnumerator(mergedResult, paginationContext);
            }
            if ("SQLServer".Equals(trunkDatabaseName))
            {
                return new TopAndRowNumberDecoratorMergedEnumerator(mergedResult, paginationContext);
            }
            return mergedResult;
        }
    }
}