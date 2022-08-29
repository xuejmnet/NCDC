using System;
using System.Collections.Generic;
using OpenConnector.CommandParser.Segment.Generic.Table;

namespace OpenConnector.CommandParser.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:52:17
* @Email: 326308290@qq.com
*/
    public sealed class DropTableCommand:DDLCommand
    {
        
        public readonly ICollection<SimpleTableSegment> Tables = new LinkedList<SimpleTableSegment>();
    }
}