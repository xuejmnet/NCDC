using System;
using System.Collections.Generic;
using NCDC.CommandParser.Common.Segment.DDL.Column;
using NCDC.CommandParser.Common.Segment.DDL.Constraint;
using NCDC.CommandParser.Common.Segment.DDL.Index;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:44:34
* @Email: 326308290@qq.com
*/
    public abstract class CreateTableCommand:AbstractSqlCommand,IDDLCommand
    {
        public  SimpleTableSegment Table { get;}
    
        public readonly ICollection<ColumnDefinitionSegment> ColumnDefinitions = new LinkedList<ColumnDefinitionSegment>();
    
        public readonly ICollection<ConstraintDefinitionSegment> ConstraintDefinitions = new LinkedList<ConstraintDefinitionSegment>();

        protected CreateTableCommand(SimpleTableSegment table)
        {
            Table = table;
        }
    }
}