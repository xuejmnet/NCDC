using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using ShardingConnector.Base;
using ShardingConnector.Extensions;
using ShardingConnector.CommandParserBinder.Command.DML;
using ShardingConnector.CommandParserBinder.MetaData.Schema;
using ShardingConnector.CommandParserBinder.Segment.Select.Projection.Impl;
using ShardingConnector.ShardingMerge.DQL.GroupBy.Aggregation;
using ShardingConnector.ShardingMerge.DQL.OrderBy;
using ShardingConnector.StreamDataReaders;

namespace ShardingConnector.ShardingMerge.DQL.GroupBy
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Wednesday, 05 May 2021 21:42:19
    * @Email: 326308290@qq.com
    */
    public sealed class GroupByStreamMergedDataReader : OrderByStreamMergedDataReader
    {
        private readonly SelectCommandContext _selectCommandContext;

        private readonly List<object> _currentRow;

        private List<object> _currentGroupByValues;
        public GroupByStreamMergedDataReader(IDictionary<string, int> labelAndIndexMap, List<IStreamDataReader> streamDataReaders, SelectCommandContext selectCommandContext, SchemaMetaData schemaMetaData) : base(streamDataReaders, selectCommandContext, schemaMetaData)
        {
            this._selectCommandContext = selectCommandContext;
            _currentRow = new List<object>(labelAndIndexMap.Count);
            _currentGroupByValues = OrderByValuesQueue.IsEmpty()
                ? new List<object>(0) : new GroupByValue(GetCurrentStreamDataReader(), selectCommandContext.GetGroupByContext().GetItems()).GetGroupValues();

        }

        public override bool Read()
        {
            _currentRow.Clear();
            if (OrderByValuesQueue.IsEmpty())
            {
                return false;
            }
            if (IsFirstNext)
            {
                base.Read();
            }
            if (AggregateCurrentGroupByRowAndNext())
            {
                _currentGroupByValues = new GroupByValue(GetCurrentStreamDataReader(), _selectCommandContext.GetGroupByContext().GetItems()).GetGroupValues();
            }
            return true;
        }

        private bool AggregateCurrentGroupByRowAndNext()
        {
            bool result = false;
            IDictionary<AggregationProjection, IAggregationUnit> aggregationUnitMap =
                _selectCommandContext.GetProjectionsContext().GetAggregationProjections().ToDictionary(o => o,
                    o => AggregationUnitFactory.Create(o.GetAggregationType(), o is AggregationDistinctProjection));
            while (_currentGroupByValues.SequenceEqual(new GroupByValue(GetCurrentStreamDataReader(), _selectCommandContext.GetGroupByContext().GetItems()).GetGroupValues()))
            {
                Aggregate(aggregationUnitMap);
                CacheCurrentRow();
                result = base.Read();
                if (!result)
                {
                    break;
                }
            }
            SetAggregationValueToCurrentRow(aggregationUnitMap);
            return result;
        }

        private void Aggregate(IDictionary<AggregationProjection, IAggregationUnit> aggregationUnitMap)
        {
            foreach (var aggregationUnitKv in aggregationUnitMap)
            {
                List<IComparable> values = new List<IComparable>(2);
                if (aggregationUnitKv.Key.GetDerivedAggregationProjections().IsEmpty())
                {
                    values.Add(GetAggregationValue(aggregationUnitKv.Key));
                }
                else
                {
                    foreach (var aggregationProjection in aggregationUnitKv.Key.GetDerivedAggregationProjections())
                    {
                        values.Add(GetAggregationValue(aggregationProjection));
                    }
                }
                aggregationUnitKv.Value.Merge(values);
            }
        }

        private void CacheCurrentRow()
        {
            for (int i = 0; i < GetCurrentStreamDataReader().ColumnCount; i++)
            {
                _currentRow.Add(GetCurrentStreamDataReader().GetValue(i));
            }
        }

        private IComparable GetAggregationValue(AggregationProjection aggregationProjection)
        {
            object result = GetCurrentStreamDataReader().GetValue(aggregationProjection.GetIndex());
            ShardingAssert.Else(null == result || result is IComparable, "Aggregation value must implements Comparable");
            return (IComparable)result;
        }

        private void SetAggregationValueToCurrentRow(IDictionary<AggregationProjection, IAggregationUnit> aggregationUnitMap)
        {
            foreach (var aggregationUnitKv in aggregationUnitMap)
            {
                _currentRow[aggregationUnitKv.Key.GetIndex()] = aggregationUnitKv.Value.GetResult();
            }
        }

        public override object GetValue(int columnIndex)
        {
            return _currentRow[columnIndex];
        }

        public override bool IsDBNull(int columnIndex)
        {
            return _currentRow[columnIndex] == null;
        }
    }
}