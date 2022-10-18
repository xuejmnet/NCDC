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
        protected UpdateCommand(ITableSegment table, SetAssignmentSegment setAssignment)
        {
            Table = table;
            SetAssignment = setAssignment;
        }

        public ITableSegment Table { get;  }
    
        public SetAssignmentSegment SetAssignment{ get; }
    
        public WhereSegment? Where { get; set; }
    

    }
}