﻿using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Segment.DML.Column;

namespace ShardingConnector.CommandParser.Segment.DDL.Column.Position
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:00:17
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ColumnFirstPositionSegment:ColumnPositionSegment
    {
        public ColumnFirstPositionSegment(int startIndex, int stopIndex, ColumnSegment columnName) : base(startIndex, stopIndex, columnName)
        {
        }
    }
}
