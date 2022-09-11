using System;
using System.Collections.Generic;
using NCDC.CommandParser.Segment.DML.Assignment;
using NCDC.CommandParser.Segment.DML.Predicate;
using NCDC.CommandParser.Segment.Generic.Table;

namespace NCDC.CommandParser.Command.DML
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