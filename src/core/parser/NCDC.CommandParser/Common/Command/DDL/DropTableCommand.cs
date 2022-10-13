using System;
using System.Collections.Generic;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:52:17
* @Email: 326308290@qq.com
*/
    public abstract class DropTableCommand:AbstractSqlCommand,IDDLCommand
    {
        
        public  ICollection<SimpleTableSegment> Tables = new LinkedList<SimpleTableSegment>();
    }
}