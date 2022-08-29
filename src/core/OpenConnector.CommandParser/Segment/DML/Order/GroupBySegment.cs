using System.Collections.Generic;
using OpenConnector.CommandParser.Segment.DML.Order.Item;

namespace OpenConnector.CommandParser.Segment.DML.Order
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:16:37
* @Email: 326308290@qq.com
*/
    public sealed class GroupBySegment:ISqlSegment
    {
        
        private readonly int _startIndex;
    
        private readonly int _stopIndex;
    
        private readonly ICollection<OrderByItemSegment> _groupByItems;

        public GroupBySegment(int startIndex, int stopIndex, ICollection<OrderByItemSegment> groupByItems)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _groupByItems = groupByItems;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public ICollection<OrderByItemSegment> GetGroupByItems()
        {
            return _groupByItems;
        }
    }
}