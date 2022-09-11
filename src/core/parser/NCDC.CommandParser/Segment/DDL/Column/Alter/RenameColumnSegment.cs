using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Segment.DML.Column;

namespace NCDC.CommandParser.Segment.DDL.Column.Alter
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
        private readonly int _startIndex;

        private readonly int _stopIndex;

        public ColumnSegment OldColumnName { get; }

        public ColumnSegment ColumnName { get; }

        public RenameColumnSegment(int startIndex, int stopIndex, ColumnSegment oldColumnName, ColumnSegment columnName)
        {
            this._startIndex = startIndex;
            this._stopIndex = stopIndex;
            OldColumnName = oldColumnName;
            ColumnName = columnName;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }
    }
}
