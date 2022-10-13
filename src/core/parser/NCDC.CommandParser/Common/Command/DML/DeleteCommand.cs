using System;
using System.Collections.Generic;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DML
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 22:00:55
* @Email: 326308290@qq.com
*/
    public abstract class DeleteCommand : AbstractSqlCommand, IDMLCommand
    {
        protected DeleteCommand(SimpleTableSegment table)
        {
            Table = table;
        }

        public SimpleTableSegment Table { get; }
        public WhereSegment? Where { get; set; }
    }
}