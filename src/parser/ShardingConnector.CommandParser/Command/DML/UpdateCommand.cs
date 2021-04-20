using System;
using System.Collections.Generic;
using ShardingConnector.CommandParser.Segment.DML.Assignment;
using ShardingConnector.CommandParser.Segment.Generic.Table;
using ShardingConnector.CommandParser.Segment.Predicate;

namespace ShardingConnector.CommandParser.Command.DML
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 22:10:03
* @Email: 326308290@qq.com
*/
    public sealed class UpdateCommand:DMLCommand
    {
        
        public readonly ICollection<SimpleTableSegment> Tables = new LinkedList<SimpleTableSegment>();
    
        public SetAssignmentSegment SetAssignment{ get; set; }
    
        public WhereSegment Where { get; set; }
    

    }
}