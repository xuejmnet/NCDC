using System.Collections.Generic;
using NCDC.CommandParser.Segment.DML.Order.Item;

namespace NCDC.CommandParser.Segment.DML.Order
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:20:27
* @Email: 326308290@qq.com
*/
    public sealed class OrderBySegment:ISqlSegment
    {
        
        private readonly int _startIndex;
    
        private readonly int _stopIndex;
    
        private readonly ICollection<OrderByItemSegment> _orderByItems;

        public OrderBySegment(int startIndex, int stopIndex, ICollection<OrderByItemSegment> orderByItems)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _orderByItems = orderByItems;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public ICollection<OrderByItemSegment> GetOrderByItems()
        {
            return _orderByItems;
        }
    }
}