using System;
using System.Collections.Generic;
using OpenConnector.CommandParser.Segment.DDL.Column.Alter;
using OpenConnector.CommandParser.Segment.DDL.Constraint;
using OpenConnector.CommandParser.Segment.Generic.Table;

namespace OpenConnector.CommandParser.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:38:49
* @Email: 326308290@qq.com
*/
    public sealed class AlterTableCommand:DDLCommand
    {
        
        public  SimpleTableSegment Table { get; }
    
        public readonly ICollection<AddColumnDefinitionSegment> AddColumnDefinitions = new LinkedList<AddColumnDefinitionSegment>();
    
        public readonly ICollection<ModifyColumnDefinitionSegment> ModifyColumnDefinitions = new LinkedList<ModifyColumnDefinitionSegment>();
    
        public readonly ICollection<DropColumnDefinitionSegment> DropColumnDefinitions = new LinkedList<DropColumnDefinitionSegment>();
    
        public readonly ICollection<ConstraintDefinitionSegment> AddConstraintDefinitions = new LinkedList<ConstraintDefinitionSegment>();

        public AlterTableCommand(SimpleTableSegment table)
        {
            Table = table;
        }
    }
}