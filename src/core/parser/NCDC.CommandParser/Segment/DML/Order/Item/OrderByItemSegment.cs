using NCDC.CommandParser.Constant;

namespace NCDC.CommandParser.Segment.DML.Order.Item
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:17:21
* @Email: 326308290@qq.com
*/
    public abstract class OrderByItemSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        private readonly OrderDirectionEnum _orderDirection;
    
        private readonly OrderDirectionEnum _nullOrderDirection;

        public OrderByItemSegment(int startIndex, int stopIndex, OrderDirectionEnum orderDirection, OrderDirectionEnum nullOrderDirection)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            _orderDirection = orderDirection;
            _nullOrderDirection = nullOrderDirection;
        }

        public OrderDirectionEnum GetOrderDirection()
        {
            return _orderDirection;
        }
        public OrderDirectionEnum GetNullOrderDirection()
        {
            return _nullOrderDirection;
        }
    }
}