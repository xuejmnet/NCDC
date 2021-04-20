using ShardingConnector.CommandParser.Segment.DML.Order.Item;

namespace ShardingConnector.ParserBinder.Segment.Select.OrderBy
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 15:26:15
* @Email: 326308290@qq.com
*/
    public sealed class OrderByItem
    {
        private readonly OrderByItemSegment _segment;

        private int index;

        public OrderByItem(OrderByItemSegment segment)
        {
            _segment = segment;
        }

        public OrderByItemSegment GetOrderByItemSegment()
        {
            return _segment;
        }

        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }

        public override bool Equals(object obj)
        {
            if (null == obj || !(obj is OrderByItem))
            {
                return false;
            }

            OrderByItem orderByItem = (OrderByItem) obj;
            return _segment.GetOrderDirection() == orderByItem.GetOrderByItemSegment().GetOrderDirection() && index == orderByItem.GetIndex();
        }

        private bool Equals(OrderByItem other)
        {
            return Equals(_segment, other._segment) && index == other.index;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_segment != null ? _segment.GetHashCode() : 0) * 397) ^ index;
            }
        }
    }
}