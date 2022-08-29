using OpenConnector.CommandParser.Constant;

namespace OpenConnector.CommandParser.Segment.DML.Order.Item
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 20:05:02
* @Email: 326308290@qq.com
*/
    public sealed class IndexOrderByItemSegment:OrderByItemSegment
    {
        
        private readonly int _columnIndex;
        public IndexOrderByItemSegment(int startIndex, int stopIndex,int columnIndex, OrderDirectionEnum orderDirection, OrderDirectionEnum nullOrderDirection) 
            : base(startIndex, stopIndex, orderDirection, nullOrderDirection)
        {
            this._columnIndex = columnIndex;
        }
        public IndexOrderByItemSegment(int startIndex, int stopIndex,int columnIndex, OrderDirectionEnum orderDirection) 
            : base(startIndex, stopIndex, orderDirection, OrderDirectionEnum.ASC)
        {
            this._columnIndex = columnIndex;
        }

        public int GetColumnIndex()
        {
            return _columnIndex;
        }
    }
}