using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.DML.Column;

namespace NCDC.CommandParser.Common.Segment.DML.Order.Item
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 20:21:47
* @Email: 326308290@qq.com
*/
    public sealed class ColumnOrderByItemSegment:TextOrderByItemSegment
    {
        private readonly ColumnSegment _column;
        public ColumnOrderByItemSegment(ColumnSegment column,OrderDirectionEnum orderDirection, OrderDirectionEnum nullOrderDirection) 
            : base(column.StartIndex, column.StopIndex, orderDirection, nullOrderDirection)
        {
            this._column = column;
        }
        public ColumnOrderByItemSegment(ColumnSegment column,OrderDirectionEnum orderDirection) 
            : base(column.StartIndex, column.StopIndex, orderDirection, OrderDirectionEnum.ASC)
        {
            this._column = column;
        }

        public override string GetExpression()
        {
            return _column.GetQualifiedName();
        }

        public ColumnSegment GetColumn()
        {
            return _column;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Column: {_column}";
        }
    }
}