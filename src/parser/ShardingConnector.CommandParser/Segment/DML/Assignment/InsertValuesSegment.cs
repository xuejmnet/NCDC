using System.Collections.Generic;
using System.Linq;
using ShardingConnector.CommandParser.Segment.DML.Expr;

namespace ShardingConnector.CommandParser.Segment.DML.Assignment
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

        public InsertValuesSegment(int startIndex, int stopIndex, ICollection<IExpressionSegment> values)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _values = values.ToList();
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
