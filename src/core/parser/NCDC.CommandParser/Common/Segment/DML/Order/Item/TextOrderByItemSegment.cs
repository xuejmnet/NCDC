using NCDC.CommandParser.Common.Constant;

namespace NCDC.CommandParser.Common.Segment.DML.Order.Item
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 20:22:03
* @Email: 326308290@qq.com
*/
    public abstract class TextOrderByItemSegment:OrderByItemSegment
    {
        protected TextOrderByItemSegment(int startIndex, int stopIndex, OrderDirectionEnum orderDirection, OrderDirectionEnum nullOrderDirection) 
            : base(startIndex, stopIndex, orderDirection, nullOrderDirection)
        {
        }
        
        public abstract string GetExpression();
    }
}