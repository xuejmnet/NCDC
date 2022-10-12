using System.Collections.Generic;
using System.Linq;
using NCDC.CommandParser.Segment.DML.Expr;

namespace NCDC.CommandParser.Segment.DML.Assignment
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
        public int StartIndex { get; }
        public int StopIndex { get; }
        public List<IExpressionSegment> Values { get; }

        public InsertValuesSegment(int startIndex, int stopIndex, List<IExpressionSegment> values)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Values = values;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Values)}: {Values}";
        }
    }
}
