using System.Collections.Generic;
using NCDC.CommandParser.Segment.DML.Order.Item;

namespace NCDC.CommandParser.Segment.DML.Order
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:16:37
* @Email: 326308290@qq.com
*/
    public sealed class GroupBySegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }

        private readonly ICollection<OrderByItemSegment> _groupByItems;

        public GroupBySegment(int startIndex, int stopIndex, ICollection<OrderByItemSegment> groupByItems)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            _groupByItems = groupByItems;
        }

        public ICollection<OrderByItemSegment> GetGroupByItems()
        {
            return _groupByItems;
        }

        public override string ToString()
        {
            return $"GroupByItems: {_groupByItems}, {nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}";
        }
    }
}