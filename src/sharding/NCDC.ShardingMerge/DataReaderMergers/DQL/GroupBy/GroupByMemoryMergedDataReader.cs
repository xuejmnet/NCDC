using NCDC.Base;
using NCDC.Basic.Metadatas;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.Extensions;
using NCDC.ShardingMerge.DataReaderMergers.DQL.GroupBy.Aggregation;
using NCDC.ShardingMerge.DataReaders.Memory;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingParser.MetaData;
using NCDC.ShardingParser.Segment.Select.Projection.Impl;
using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.DataReaderMergers.DQL.GroupBy
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Friday, 07 May 2021 22:37:22
    * @Email: 326308290@qq.com
    */
    public sealed class GroupByMemoryMergedDataReader : MemoryMergedDataReader
    {
        public GroupByMemoryMergedDataReader(List<IStreamDataReader> queryDataReaders, SelectCommandContext sqlCommandContext, ITableMetadataManager tableMetadataManager) : base(tableMetadataManager, sqlCommandContext, queryDataReaders)
        {
        }

        protected override List<MemoryQueryResultRow> Init(ITableMetadataManager tableMetadataManager, ISqlCommandContext<ISqlCommand> sqlCommandContext, List<IStreamDataReader> streamDataReaders)
        {
            var selectCommandContext = (SelectCommandContext)sqlCommandContext;
            IDictionary<GroupByValue, MemoryQueryResultRow> dataMap = new Dictionary<GroupByValue, MemoryQueryResultRow>(1024);
            IDictionary<GroupByValue, IDictionary<AggregationProjection, IAggregationUnit>> aggregationMap = new Dictionary<GroupByValue, IDictionary<AggregationProjection, IAggregationUnit>>(1024);


            foreach (var streamDataReader in streamDataReaders)
            {
                while (streamDataReader.Read())
                {
                    GroupByValue groupByValue = new GroupByValue(streamDataReader, selectCommandContext.GetGroupByContext().GetItems());
                    InitForFirstGroupByValue(selectCommandContext, streamDataReader, groupByValue, dataMap, aggregationMap);
                    Aggregate(selectCommandContext, streamDataReader, groupByValue, aggregationMap);
                }
            }
            SetAggregationValueToMemoryRow(selectCommandContext, dataMap, aggregationMap);
            List<bool> valueCaseSensitive = streamDataReaders.IsEmpty() ? new List<bool>(0) : GetValueCaseSensitive(streamDataReaders.First(), selectCommandContext, tableMetadataManager);
            return GetMemoryResultSetRows(selectCommandContext, dataMap, valueCaseSensitive);
        }

        private void InitForFirstGroupByValue(SelectCommandContext selectCommandContext, IStreamDataReader streamDataReader,
         GroupByValue groupByValue, IDictionary<GroupByValue, MemoryQueryResultRow> dataMap,
         IDictionary<GroupByValue, IDictionary<AggregationProjection, IAggregationUnit>> aggregationMap)
        {
            if (!dataMap.ContainsKey(groupByValue))
            {
                dataMap.Add(groupByValue, new MemoryQueryResultRow(streamDataReader));
            }

            if (!aggregationMap.ContainsKey(groupByValue))
            {
                var map = selectCommandContext.GetProjectionsContext().GetAggregationProjections().ToDictionary(o => o,
                    o => AggregationUnitFactory.Create(o.GetAggregationType(), o is AggregationDistinctProjection));

                aggregationMap.Add(groupByValue, map);
            }
        }

        private void Aggregate(SelectCommandContext selectCommandContext, IStreamDataReader streamDataReader,
         GroupByValue groupByValue, IDictionary<GroupByValue, IDictionary<AggregationProjection, IAggregationUnit>> aggregationMap)
        {

            var aggregationProjections = selectCommandContext.GetProjectionsContext().GetAggregationProjections();
            foreach (var aggregationProjection in aggregationProjections)
            {
                List<IComparable> values = new List<IComparable>(2);
                if (aggregationProjection.GetDerivedAggregationProjections().IsEmpty())
                {
                    values.Add(GetAggregationValue(streamDataReader, aggregationProjection));
                }
                else
                {
                    foreach (var derived in aggregationProjection.GetDerivedAggregationProjections())
                    {
                        values.Add(GetAggregationValue(streamDataReader, derived));
                    }
                }

                aggregationMap.Get(groupByValue).Get(aggregationProjection).Merge(values);
            }
        }

        private IComparable GetAggregationValue(IStreamDataReader streamDataReader, AggregationProjection aggregationProjection)
        {
            object result = streamDataReader.GetValue(aggregationProjection.GetIndex());
            ShardingAssert.Else(null == result || result is IComparable, "Aggregation value must implements Comparable");
            return (IComparable)result;
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

        private List<bool> GetValueCaseSensitive(IStreamDataReader streamDataReader, SelectCommandContext selectCommandContext, ITableMetadataManager tableMetadataManager)
        {
            List<bool> result = new List<bool>(streamDataReader.ColumnCount + 1)
            {
                false
            };
            for (int columnIndex = 0; columnIndex < streamDataReader.ColumnCount; columnIndex++)
            {
                result.Add(GetValueCaseSensitiveFromTables(streamDataReader, selectCommandContext, tableMetadataManager, columnIndex));
            }

            return result;
        }

        private bool GetValueCaseSensitiveFromTables(IStreamDataReader streamDataReader, SelectCommandContext selectCommandContext,
            ITableMetadataManager tableMetadataManager, int columnIndex)
        {
            foreach (var simpleTableSegment in selectCommandContext.GetAllTables())
            {
                String tableName = simpleTableSegment.TableName.IdentifierValue.Value;
                TableMetadata tableMetaData = tableMetadataManager.Get(tableName);
                IReadOnlyDictionary<String, ColumnMetadata> columns = tableMetaData.Columns;
                String columnName = streamDataReader.GetColumnName(columnIndex);
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