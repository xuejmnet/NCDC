using System;
using System.Collections.Generic;
using OpenConnector.CommandParser.Segment.DML.Assignment;
using OpenConnector.CommandParser.Segment.Generic.Table;
using OpenConnector.CommandParser.Segment.Predicate;

namespace OpenConnector.CommandParser.Command.DML
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