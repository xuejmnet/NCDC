using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Common.Segment.DML.Column;

namespace NCDC.CommandParser.Common.Segment.DDL.Column.Alter
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:06:23
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class RenameColumnSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public ColumnSegment OldColumnName { get; }

        public ColumnSegment ColumnName { get; }

        public RenameColumnSegment(int startIndex, int stopIndex, ColumnSegment oldColumnName, ColumnSegment columnName)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            OldColumnName = oldColumnName;
            ColumnName = columnName;
        }

    }
}
