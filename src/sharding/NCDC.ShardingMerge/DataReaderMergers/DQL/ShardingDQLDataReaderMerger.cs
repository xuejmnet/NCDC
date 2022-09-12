using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Constant;
using NCDC.CommandParser.Segment.DML.Order.Item;
using NCDC.CommandParser.Util;
using NCDC.Enums;
using NCDC.Extensions;
using NCDC.ShardingMerge.Abstractions;
using NCDC.ShardingMerge.DataReaderMergers.DQL.GroupBy;
using NCDC.ShardingMerge.DataReaderMergers.DQL.Iterator;
using NCDC.ShardingMerge.DataReaderMergers.DQL.OrderBy;
using NCDC.ShardingMerge.DataReaderMergers.DQL.Pagination;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingParser.MetaData.Schema;
using NCDC.ShardingParser.Segment.Select.OrderBy;
using NCDC.StreamDataReaders;
using OpenConnector.ShardingMerge.DQL.GroupBy;

namespace NCDC.ShardingMerge.DataReaderMergers.DQL
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Friday, 07 May 2021 22:26:43
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingDQLDataReaderMerger : IDataReaderMerger
    {
        private readonly DatabaseTypeEnum _databaseType;
        private readonly ITableMetadataManager _tableMetadataManager;

        public ShardingDQLDataReaderMerger(DatabaseTypeEnum databaseType,ITableMetadataManager tableMetadataManager)
        {
            _databaseType = databaseType;
            _tableMetadataManager = tableMetadataManager;
        }

        public IStreamDataReader Merge(List<IStreamDataReader> streamDataReaders, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            if (1 == streamDataReaders.Count)
            {
                return new IteratorStreamMergedDataReader(streamDataReaders);
            }
            IDictionary<string, int> columnLabelIndexMap = GetColumnLabelIndexMap(streamDataReaders[0]);
            var selectCommandContext = (SelectCommandContext)sqlCommandContext;
            selectCommandContext.SetIndexes(columnLabelIndexMap);
            var mergedEnumerator = Build(streamDataReaders, selectCommandContext, columnLabelIndexMap, schemaMetaData);
            return Decorate(streamDataReaders, selectCommandContext, mergedEnumerator);
        }

        private IDictionary<string, int> GetColumnLabelIndexMap(IStreamDataReader queryResult)
        {
            IDictionary<string, int> result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = queryResult.ColumnCount-1; i >= 0; i--)
            {
                result.Add(SqlUtil.GetExactlyValue(queryResult.GetColumnLabel(i)), i);
            }
            return result;
        }

        private IStreamDataReader Build(List<IStreamDataReader> streamDataReaders, SelectCommandContext selectCommandContext,
                                    IDictionary<string, int> columnLabelIndexMap, SchemaMetaData schemaMetaData)
        {
            if (IsNeedProcessGroupBy(selectCommandContext))
            {
                return GetGroupByMergedResult(streamDataReaders, selectCommandContext, columnLabelIndexMap, schemaMetaData);
            }
            if (IsNeedProcessDistinctRow(selectCommandContext))
            {
                SetGroupByForDistinctRow(selectCommandContext);
                return GetGroupByMergedResult(streamDataReaders, selectCommandContext, columnLabelIndexMap, schemaMetaData);
            }
            if (IsNeedProcessOrderBy(selectCommandContext))
            {
                return new OrderByStreamMergedDataReader(streamDataReaders, selectCommandContext, schemaMetaData);
            }
            return new IteratorStreamMergedDataReader(streamDataReaders);
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

        private IStreamDataReader GetGroupByMergedResult(List<IStreamDataReader> streamDataReaders, SelectCommandContext selectCommandContext,
                                                     IDictionary<string, int> columnLabelIndexMap, SchemaMetaData schemaMetaData)
        {
            if (selectCommandContext.IsSameGroupByAndOrderByItems())
            {
                return new GroupByStreamMergedDataReader(columnLabelIndexMap, streamDataReaders, selectCommandContext,
                    schemaMetaData);
            }
            return new GroupByMemoryMergedDataReader(streamDataReaders, selectCommandContext, schemaMetaData);
        }

        private bool IsNeedProcessOrderBy(SelectCommandContext selectCommandContext)
        {
            return !selectCommandContext.GetOrderByContext().GetItems().IsEmpty();
        }

        private IStreamDataReader Decorate(List<IStreamDataReader> streamDataReaders, SelectCommandContext selectCommandContext, IStreamDataReader mergedStreamDataReader)
        {
            var paginationContext = selectCommandContext.GetPaginationContext();
            if (!paginationContext.HasPagination() || 1 == streamDataReaders.Count)
            {
                return mergedStreamDataReader;
            }
            String trunkDatabaseName = DatabaseTypes.GetTrunkDatabaseType(_databaseType.GetName()).GetName();
            if ("MySql".Equals(trunkDatabaseName) || "PostgreSQL".Equals(trunkDatabaseName))
            {
                return new LimitDecoratorStreamDataReader(mergedStreamDataReader, paginationContext);
            }
            if ("Oracle".Equals(trunkDatabaseName))
            {
                return new RowNumberDecoratorStreamDataReader(mergedStreamDataReader, paginationContext);
            }
            if ("SQLServer".Equals(trunkDatabaseName))
            {
                return new TopAndRowNumberDecoratorStreamDataReader(mergedStreamDataReader, paginationContext);
            }
            return mergedStreamDataReader;
        }
    }
}