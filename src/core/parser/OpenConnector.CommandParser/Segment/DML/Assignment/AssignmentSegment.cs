using OpenConnector.CommandParser.Segment.DML.Column;
using OpenConnector.CommandParser.Segment.DML.Expr;

namespace OpenConnector.CommandParser.Segment.DML.Assignment
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 7:41:56
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class AssignmentSegment:ISqlSegment
    {
        private readonly int _startIndex;
        private readonly int _stopIndex;
        private readonly ColumnSegment _column;
    
        private readonly IExpressionSegment _value;

        public AssignmentSegment(int startIndex, int stopIndex, ColumnSegment column, IExpressionSegment value)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _column = column;
            _value = value;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public ColumnSegment GetColumn()
        {
            return _column;
        }

        public IExpressionSegment GetValue()
        {
            return _value;
        }
    }
}
