using System;
using System.Collections.Generic;
using NCDC.CommandParser.Common.Segment.DML.Assignment;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DML
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 22:10:03
* @Email: 326308290@qq.com
*/
    public abstract class UpdateCommand: AbstractSqlCommand, IDMLCommand
    {
        
        public ITableSegment? Table { get; set; }
    
        public SetAssignmentSegment? SetAssignment{ get; set; }
    
        public WhereSegment? Where { get; set; }
    

    }
}