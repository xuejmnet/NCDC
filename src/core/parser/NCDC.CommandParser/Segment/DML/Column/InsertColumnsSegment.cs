using System.Collections.Generic;
using System.Linq;

namespace NCDC.CommandParser.Segment.DML.Column
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 7:38:42
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class InsertColumnsSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public ICollection<ColumnSegment> Columns { get; }


        public InsertColumnsSegment(int startIndex, int stopIndex, ICollection<ColumnSegment> columns)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Columns = columns;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Columns)}: {Columns}";
        }
    }
}
