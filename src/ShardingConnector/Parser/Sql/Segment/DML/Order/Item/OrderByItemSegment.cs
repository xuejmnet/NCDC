using System;
using System.Collections.Generic;
using ShardingConnector.Parser.Sql.Constant;

namespace ShardingConnector.Parser.Sql.Segment.DML.Order.Item
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:17:21
* @Email: 326308290@qq.com
*/
    public abstract class OrderByItemSegment:ISqlSegment
    {
        private readonly int _startIndex;
    
        private readonly int _stopIndex;
        private readonly OrderDirectionEnum _orderDirection;
    
        private readonly OrderDirectionEnum _nullOrderDirection;

        public OrderByItemSegment(int startIndex, int stopIndex, OrderDirectionEnum orderDirection, OrderDirectionEnum nullOrderDirection)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _orderDirection = orderDirection;
            _nullOrderDirection = nullOrderDirection;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
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