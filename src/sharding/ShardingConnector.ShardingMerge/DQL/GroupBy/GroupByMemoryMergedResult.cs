using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using ShardingConnector.Base;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Segment.Generic.Table;
using ShardingConnector.Executor;
using ShardingConnector.Extensions;
using ShardingConnector.Merge.Reader.Memory;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ParserBinder.MetaData.Column;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.ParserBinder.MetaData.Table;
using ShardingConnector.ParserBinder.Segment.Select.Projection.Impl;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingMerge.DQL.GroupBy.Aggregation;

namespace ShardingConnector.ShardingMerge.DQL.GroupBy
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 22:37:22
* @Email: 326308290@qq.com
*/
    public sealed class GroupByMemoryMergedResult : MemoryMergedEnumerator<ShardingRule>
    {
        public GroupByMemoryMergedResult(List<IQueryEnumerator> queryEnumerators, SelectCommandContext sqlCommandContext, SchemaMetaData schemaMetaData) : base(null, schemaMetaData, sqlCommandContext, queryEnumerators)
        {
        }

        protected override List<MemoryQueryResultRow> Init(ShardingRule rule, SchemaMetaData schemaMetaData, ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IQueryEnumerator> queryEnumerators)
        {
            var selectCommandContext = (SelectCommandContext) sqlCommandContext;
            Map<GroupByValue, MemoryQueryResultRow> dataMap = new HashMap<>(1024);
            Map<GroupByValue, Map<AggregationProjection, AggregationUnit>> aggregationMap = new HashMap<>(1024);
            for (QueryResult each :
            queryResults) {
                while (each.next())
                {
                    GroupByValue groupByValue = new GroupByValue(each, selectStatementContext.getGroupByContext().getItems());
                    initForFirstGroupByValue(selectStatementContext, each, groupByValue, dataMap, aggregationMap);
                    aggregate(selectStatementContext, each, groupByValue, aggregationMap);
                }
            }
            setAggregationValueToMemoryRow(selectStatementContext, dataMap, aggregationMap);
            List<Boolean> valueCaseSensitive = queryResults.isEmpty() ? Collections.emptyList() : getValueCaseSensitive(queryResults.iterator().next(), selectStatementContext, schemaMetaData);
            return getMemoryResultSetRows(selectStatementContext, dataMap, valueCaseSensitive);
        }

        private void InitForFirstGroupByValue( SelectCommandContext selectCommandContext,  IQueryEnumerator queryResult,
         GroupByValue groupByValue,  IDictionary<GroupByValue, MemoryQueryResultRow> dataMap,
         IDictionary<GroupByValue, IDictionary<AggregationProjection, IAggregationUnit>> aggregationMap) {
            if (!dataMap.ContainsKey(groupByValue))
            {
                dataMap.Add(groupByValue, new MemoryQueryResultRow(queryResult));
            }

            if (!aggregationMap.ContainsKey(groupByValue))
            {
                selectCommandContext.GetProjectionsContext().GetAggregationProjections().ToDictionary()
                Map<AggregationProjection, AggregationUnit> map = Maps.toMap(selectStatementContext.getProjectionsContext().getAggregationProjections(),
                    input->AggregationUnitFactory.create(input.getType(), input instanceof AggregationDistinctProjection));
                aggregationMap.put(groupByValue, map);
            }
        }

        private void Aggregate( SelectCommandContext selectCommandContext,  IQueryEnumerator queryEnumerator,
         GroupByValue groupByValue,  IDictionary<GroupByValue, IDictionary<AggregationProjection, IAggregationUnit>> aggregationMap) {
            
            var aggregationProjections = selectCommandContext.GetProjectionsContext().GetAggregationProjections();
            foreach (var aggregationProjection in aggregationProjections)
            {
                List<IComparable> values = new List<IComparable>(2);
                if (aggregationProjection.GetDerivedAggregationProjections().IsEmpty())
                {
                    values.Add(GetAggregationValue(queryEnumerator, aggregationProjection));
                }
                else
                {
                    foreach (var derived in aggregationProjection.GetDerivedAggregationProjections())
                    {
                        values.Add(GetAggregationValue(queryEnumerator, derived));
                    }
                }

                aggregationMap.Get(groupByValue).Get(aggregationProjection).Merge(values);
            }
        }

        private IComparable GetAggregationValue( IQueryEnumerator queryResult,  AggregationProjection aggregationProjection) {
            object result = queryResult.GetValue(aggregationProjection.GetIndex());
            ShardingAssert.Else(null == result || result is IComparable, "Aggregation value must implements Comparable");
            return (IComparable) result;
        }

        private void SetAggregationValueToMemoryRow(SelectCommandContext selectCommandContext,
            IDictionary<GroupByValue, MemoryQueryResultRow> dataMap, IDictionary<GroupByValue, IDictionary<AggregationProjection, IAggregationUnit>> aggregationMap)
        {
            foreach (var dataKv in dataMap)
            {
                var aggregationProjections = selectCommandContext.GetProjectionsContext().GetAggregationProjections();
                foreach (var aggregationProjection in aggregationProjections)
                {
                    dataKv.Value.SetCell(aggregationProjection.GetIndex(), aggregationMap.Get(dataKv.Key).Get(aggregationProjection).GetResult());
                }
            }
        }

        private List<bool> GetValueCaseSensitive(IQueryEnumerator queryEnumerator, SelectCommandContext selectCommandContext, SchemaMetaData schemaMetaData)
        {
            List<bool> result = new List<bool>(queryEnumerator.ColumnCount + 1)
            {
                false
            };
            for (int columnIndex = 1; columnIndex <= queryEnumerator.ColumnCount; columnIndex++)
            {
                result.Add(GetValueCaseSensitiveFromTables(queryEnumerator, selectCommandContext, schemaMetaData, columnIndex));
            }

            return result;
        }

        private bool GetValueCaseSensitiveFromTables(IQueryEnumerator queryEnumerator, SelectCommandContext selectCommandContext,
            SchemaMetaData schemaMetaData, int columnIndex)
        {
            foreach (var simpleTableSegment in selectCommandContext.GetAllTables())
            {
                String tableName = simpleTableSegment.GetTableName().GetIdentifier().GetValue();
                TableMetaData tableMetaData = schemaMetaData.Get(tableName);
                IDictionary<String, ColumnMetaData> columns = tableMetaData.GetColumns();
                String columnName = queryEnumerator.GetColumnName(columnIndex);
                if (columns.ContainsKey(columnName))
                {
                    return columns[columnName].CaseSensitive;
                }
            }

            return false;
        }

        private List<MemoryQueryResultRow> GetMemoryResultSetRows(SelectCommandContext selectCommandContext,
            IDictionary<GroupByValue, MemoryQueryResultRow> dataMap, List<bool> valueCaseSensitive)
        {
            List<MemoryQueryResultRow> result = new List<MemoryQueryResultRow>(dataMap.Values);
            result.Sort(new GroupByRowComparator(selectCommandContext, valueCaseSensitive));
            return result;
        }
    }
}