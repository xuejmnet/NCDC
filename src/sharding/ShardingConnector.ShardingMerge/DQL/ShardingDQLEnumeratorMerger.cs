using System;
using System.Collections.Generic;

using ShardingConnector.Api.Database.DatabaseType;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Constant;
using ShardingConnector.CommandParser.Segment.DML.Order.Item;
using ShardingConnector.CommandParser.Util;
using ShardingConnector.Extensions;
using ShardingConnector.Merge.Engine.Merger;
using ShardingConnector.CommandParserBinder.Command;
using ShardingConnector.CommandParserBinder.Command.DML;
using ShardingConnector.CommandParserBinder.MetaData.Schema;
using ShardingConnector.CommandParserBinder.Segment.Select.OrderBy;
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
    public sealed class ShardingDQLEnumeratorMerger : IDataReaderMerger
    {
        private readonly IDatabaseType _databaseType;

        public ShardingDQLEnumeratorMerger(IDatabaseType databaseType)
        {
            _databaseType = databaseType;
        }

        public IStreamDataReader Merge(List<IStreamDataReader> streamDataReaders, ISqlCommandContext<ISqlCommand> sqlCommandContext, SchemaMetaData schemaMetaData)
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