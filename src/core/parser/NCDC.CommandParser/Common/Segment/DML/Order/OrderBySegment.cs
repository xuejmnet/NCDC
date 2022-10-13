using System.Collections.Generic;
using NCDC.CommandParser.Common.Segment.DML.Order.Item;

namespace NCDC.CommandParser.Common.Segment.DML.Order
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:20:27
* @Email: 326308290@qq.com
*/
    public sealed class OrderBySegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }

        private readonly ICollection<OrderByItemSegment> _orderByItems;

        public OrderBySegment(int startIndex, int stopIndex, ICollection<OrderByItemSegment> orderByItems)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            _orderByItems = orderByItems;
        }
        public ICollection<OrderByItemSegment> GetOrderByItems()
        {
            return _orderByItems;
        }

        public override string ToString()
        {
            return $"OrderByItems: {_orderByItems}, {nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}";
        }
    }
}