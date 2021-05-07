using System;
using System.Collections.Generic;
using ShardingConnector.Api.Database.DatabaseType;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Segment.DML.Order.Item;
using ShardingConnector.Executor;
using ShardingConnector.Extensions;
using ShardingConnector.Merge.Engine.Merger;
using ShardingConnector.Merge.Reader;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.ParserBinder.Segment.Select.OrderBy;
using ShardingConnector.ShardingMerge.DQL.GroupBy;
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
    public sealed class ShardingDQLEnumeratorMerger:IResultMerger
    {
        private readonly IDatabaseType databaseType;

        public ShardingDQLEnumeratorMerger(IDatabaseType databaseType)
        {
            this.databaseType = databaseType;
        }

        public IMergedEnumerator Merge(List<IQueryEnumerator> queryEnumerators, ISqlCommandContext<ISqlCommand> sqlCommandContext, SchemaMetaData schemaMetaData)
        {
            if (1 == queryResults.size()) {
            return new IteratorStreamMergedResult(queryResults);
        }
        Map<String, Integer> columnLabelIndexMap = getColumnLabelIndexMap(queryResults.get(0));
        SelectStatementContext selectStatementContext = (SelectStatementContext) sqlStatementContext;
        selectStatementContext.setIndexes(columnLabelIndexMap);
        MergedResult mergedResult = build(queryResults, selectStatementContext, columnLabelIndexMap, schemaMetaData);
        return decorate(queryResults, selectStatementContext, mergedResult);
    }
    
    private Map<String, Integer> getColumnLabelIndexMap( QueryResult queryResult) {
        Map<String, Integer> result = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        for (int i = queryResult.getColumnCount(); i > 0; i--) {
            result.put(SQLUtil.getExactlyValue(queryResult.getColumnLabel(i)), i);
        }
        return result;
    }
    
    private MergedResult build( List<QueryResult> queryResults,  SelectStatementContext selectStatementContext,
                                Map<String, Integer> columnLabelIndexMap,  SchemaMetaData schemaMetaData){
        if (isNeedProcessGroupBy(selectStatementContext)) {
            return getGroupByMergedResult(queryResults, selectStatementContext, columnLabelIndexMap, schemaMetaData);
        }
        if (isNeedProcessDistinctRow(selectStatementContext)) {
            setGroupByForDistinctRow(selectStatementContext);
            return getGroupByMergedResult(queryResults, selectStatementContext, columnLabelIndexMap, schemaMetaData);
        }
        if (isNeedProcessOrderBy(selectStatementContext)) {
            return new OrderByStreamMergedResult(queryResults, selectStatementContext, schemaMetaData);
        }
        return new IteratorStreamMergedResult(queryResults);
    }
    
    private boolean isNeedProcessGroupBy( SelectStatementContext selectStatementContext) {
        return !selectStatementContext.getGroupByContext().getItems().isEmpty() || !selectStatementContext.getProjectionsContext().getAggregationProjections().isEmpty();
    }
    
    private boolean isNeedProcessDistinctRow( SelectStatementContext selectStatementContext) {
        return selectStatementContext.getProjectionsContext().isDistinctRow();
    }
    
    private void setGroupByForDistinctRow( SelectStatementContext selectStatementContext) {
        for (int index = 1; index <= selectStatementContext.getProjectionsContext().getExpandProjections().size(); index++) {
            OrderByItem orderByItem = new OrderByItem(new IndexOrderByItemSegment(-1, -1, index, OrderDirection.ASC, OrderDirection.ASC));
            orderByItem.setIndex(index);
            selectStatementContext.getGroupByContext().getItems().add(orderByItem);
        }
    }
    
    private MergedResult getGroupByMergedResult( List<QueryResult> queryResults,  SelectStatementContext selectStatementContext,
                                                 Map<String, Integer> columnLabelIndexMap,  SchemaMetaData schemaMetaData) {
        return selectStatementContext.isSameGroupByAndOrderByItems()
                ? new GroupByStreamMergedEnumerator(columnLabelIndexMap, queryResults, selectStatementContext, schemaMetaData)
                : new GroupByMemoryMerged(queryResults, selectStatementContext, schemaMetaData);
    }
    
    private bool IsNeedProcessOrderBy( SelectCommandContext selectCommandContext) {
        return !selectCommandContext.GetOrderByContext().GetItems().IsEmpty();
    }
    
    private IMergedEnumerator Decorate(List<IQueryEnumerator> queryResults,  SelectCommandContext selectCommandContext,  IMergedEnumerator mergedResult){
        var paginationContext = selectCommandContext.GetPaginationContext();
        if (!paginationContext.HasPagination() || 1 == queryResults.Count) {
            return mergedResult;
        }
        String trunkDatabaseName = DatabaseTypes.GetTrunkDatabaseType(databaseType.GetName()).GetName();
        if ("MySQL".Equals(trunkDatabaseName) || "PostgreSQL".Equals(trunkDatabaseName)) {
            return new LimitDecoratorMergedEnumerator(mergedResult, paginationContext);
        }
        if ("Oracle".Equals(trunkDatabaseName)) {
            return new RowNumberDecoratorMergedEnumerator(mergedResult, paginationContext);
        }
        if ("SQLServer".Equals(trunkDatabaseName)) {
            return new TopAndRowNumberDecoratorMergedEnumerator(mergedResult, paginationContext);
        }
        return mergedResult;
    }
    }
}