using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Segment.DML.Column;

namespace NCDC.CommandParser.Segment.DDL.Column.Position
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:04:35
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ColumnAfterPositionSegment:ColumnPositionSegment
    {
        public ColumnAfterPositionSegment(int startIndex, int stopIndex, ColumnSegment columnName) : base(startIndex, stopIndex, columnName)
        {
        }
    }
}
