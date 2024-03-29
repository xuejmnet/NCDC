using NCDC.CommandParser.Common.Segment.DML.Pagination;
using NCDC.CommandParser.Common.Segment.DML.Pagination.limit;
using NCDC.Exceptions;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingAdoNet;

namespace NCDC.ShardingParser.Segment.Select.Pagination
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Sunday, 11 April 2021 15:59:10
    * @Email: 326308290@qq.com
    */
    public sealed class PaginationContext
    {

        private readonly bool _hasPagination;

        private readonly IPaginationValueSegment? _offsetSegment;

        private readonly IPaginationValueSegment? _rowCountSegment;

        private readonly long _actualOffset;

        private readonly long? _actualRowCount;

        public PaginationContext(IPaginationValueSegment? offsetSegment, IPaginationValueSegment? rowCountSegment, ParameterContext parameterContext)
        {
            _hasPagination = null != offsetSegment || null != rowCountSegment;
            this._offsetSegment = offsetSegment;
            this._rowCountSegment = rowCountSegment;
            _actualOffset = null == offsetSegment ? 0 : GetValue(offsetSegment, parameterContext);
            _actualRowCount = null == rowCountSegment ? (long?)null : GetValue(rowCountSegment, parameterContext);
        }

        private long GetValue(IPaginationValueSegment paginationValueSegment, ParameterContext parameterContext)
        {
            if (paginationValueSegment is IParameterMarkerPaginationValueSegment parameterMarkerPaginationValueSegment)
            {
                if (parameterContext.TryGetParameterValue(parameterMarkerPaginationValueSegment.ParameterName,out var parameterValue))
                {
                    return parameterValue is long l ? l : (int)parameterValue;
                }
                else
                {
                    throw new ShardingException(
                        $"{nameof(IParameterMarkerPaginationValueSegment)} cant get value that parameter name:[{parameterMarkerPaginationValueSegment.ParameterName}]  ");
                }

            }
            else
            {
                return ((INumberLiteralPaginationValueSegment)paginationValueSegment).Value;
            }
        }

        /**
         * Get offset segment.
         * 
         * @return offset segment
         */
        public IPaginationValueSegment? GetOffsetSegment()
        {
            return _offsetSegment;
        }

        /**
         * Get row count segment.
         *
         * @return row count segment
         */
        public IPaginationValueSegment? GetRowCountSegment()
        {
            return _rowCountSegment;
        }

        public long GetActualOffset()
        {
            if (null == _offsetSegment)
            {
                return 0L;
            }
            return _offsetSegment.IsBoundOpened() ? _actualOffset - 1 : _actualOffset;
        }

        public long? GetActualRowCount()
        {
            if (null == _rowCountSegment)
            {
                return null;
            }
            return _rowCountSegment.IsBoundOpened() ? _actualRowCount + 1 : _actualRowCount;
        }

        public int? GetOffsetParameterIndex()
        {
            return _offsetSegment is IParameterMarkerPaginationValueSegment offset
                ? offset.ParameterIndex : (int?)null;
        }

        public int? GetRowCountParameterIndex()
        {
            return _rowCountSegment is IParameterMarkerPaginationValueSegment rowCount
                    ? rowCount.ParameterIndex : (int?)null;
        }

        public long GetRevisedOffset()
        {
            return 0L;
        }

        /**
         * Get revised row count.
         * 
         * @param shardingStatement sharding optimized statement
         * @return revised row count
         */
        public long GetRevisedRowCount(SelectCommandContext shardingCommand)
        {
            if (IsMaxRowCount(shardingCommand))
            {
                return int.MaxValue;
            }
            return _rowCountSegment is LimitValueSegment ? _actualOffset + _actualRowCount.GetValueOrDefault() : _actualRowCount.GetValueOrDefault();
        }

        private bool IsMaxRowCount(SelectCommandContext shardingCommand)
        {
            return (shardingCommand.GetGroupByContext().GetItems().Any()
                    || shardingCommand.GetProjectionsContext().GetAggregationProjections().Any()) && !shardingCommand.IsSameGroupByAndOrderByItems();
        }

        public bool HasPagination()
        {
            return _hasPagination;
        }
    }
}