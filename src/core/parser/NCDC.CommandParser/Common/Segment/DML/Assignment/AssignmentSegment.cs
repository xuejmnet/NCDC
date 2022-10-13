using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr;

namespace NCDC.CommandParser.Common.Segment.DML.Assignment
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 7:41:56
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public abstract class AssignmentSegment:ISqlSegment
    {
        public abstract List<ColumnSegment> GetColumns();

        public abstract IExpressionSegment GetValue();
        public abstract int StartIndex { get; }
        public abstract int StopIndex { get; }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}";
        }
    }
}
