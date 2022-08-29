using System;
using System.Collections.Generic;
using OpenConnector.CommandParser.Segment.DDL.Index;
using OpenConnector.CommandParser.Segment.Generic.Table;

namespace OpenConnector.CommandParser.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:51:08
* @Email: 326308290@qq.com
*/
    public sealed class DropIndexCommand:DDLCommand
    {
        public readonly ICollection<IndexSegment> Indexes = new LinkedList<IndexSegment>();
    
        public SimpleTableSegment Table { get; set; }
    }
}