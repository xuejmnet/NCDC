using System;
using System.Collections.Generic;
using NCDC.CommandParser.Common.Segment.DDL.Index;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:51:08
* @Email: 326308290@qq.com
*/
    public abstract class DropIndexCommand:AbstractSqlCommand,IDDLCommand
    {
        public  ICollection<IndexSegment> Indexes = new LinkedList<IndexSegment>();
    }
}