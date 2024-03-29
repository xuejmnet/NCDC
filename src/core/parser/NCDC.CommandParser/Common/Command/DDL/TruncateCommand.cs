using System;
using System.Collections.Generic;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:57:53
* @Email: 326308290@qq.com
*/
    public abstract class TruncateCommand:AbstractSqlCommand,IDDLCommand
    {
        public ICollection<SimpleTableSegment> Tables = new LinkedList<SimpleTableSegment>();

    }
}