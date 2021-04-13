using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Parser.Sql.Segment.DML.Expr;

namespace ShardingConnector.Parser.Sql.Segment.DML.Assignment
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 7:49:32
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class InsertValuesSegment:ISqlSegment
    {
        private readonly int _startIndex;
        private readonly int _stopIndex;
        private readonly List<IExpressionSegment> _values;

        public InsertValuesSegment(int startIndex, int stopIndex, List<IExpressionSegment> values)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _values = values;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public List<IExpressionSegment> GetValues()
        {
            return _values;
        }
    }
}
