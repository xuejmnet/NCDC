using System;
using ShardingConnector.Parser.Sql.Constant;
using ShardingConnector.Parser.Sql.Segment.DML.Column;

namespace ShardingConnector.Parser.Sql.Segment.DML.Order.Item
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
            : base(column.GetStartIndex(), column.GetStopIndex(), orderDirection, nullOrderDirection)
        {
            this._column = column;
        }
        public ColumnOrderByItemSegment(ColumnSegment column,OrderDirectionEnum orderDirection) 
            : base(column.GetStartIndex(), column.GetStopIndex(), orderDirection, OrderDirectionEnum.ASC)
        {
            this._column = column;
        }

        public override string GetText()
        {
            return _column.GetQualifiedName();
        }

        public ColumnSegment GetColumn()
        {
            return _column;
        }
    }
}