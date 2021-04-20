using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Segment.DML.Expr.Simple;

namespace ShardingConnector.CommandParser.Segment.DML.Expr.SubQuery
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:24:59
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SubQueryExpressionSegment:ISimpleExpressionSegment
    {
        public SubQueryExpressionSegment(SubQuerySegment subQuery)
        {
            SubQuery = subQuery;
        }

        public  SubQuerySegment SubQuery { get;}
        public int GetStartIndex()
        {
            return SubQuery.GetStartIndex();
        }

        public int GetStopIndex()
        {
            return SubQuery.GetStopIndex();
        }
    }
}
