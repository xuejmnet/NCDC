using System;
using System.Collections.Generic;
using NCDC.CommandParser.Common.Segment.DDL.Column.Alter;
using NCDC.CommandParser.Common.Segment.DDL.Constraint;
using NCDC.CommandParser.Common.Segment.DDL.Constraint.Alter;
using NCDC.CommandParser.Common.Segment.DDL.Index;
using NCDC.CommandParser.Common.Segment.DDL.Table;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:38:49
* @Email: 326308290@qq.com
*/
    public abstract class AlterTableCommand:AbstractSqlCommand,IDDLCommand
    {
        
        public  SimpleTableSegment Table { get; }
        public SimpleTableSegment? RenameTable { get; set; }
        public ConvertTableDefinitionSegment? ConvertTableDefinition { get; set; }
    
        public  ICollection<AddColumnDefinitionSegment> AddColumnDefinitions = new LinkedList<AddColumnDefinitionSegment>();
    
        public  ICollection<ModifyColumnDefinitionSegment> ModifyColumnDefinitions = new LinkedList<ModifyColumnDefinitionSegment>();
        public  ICollection<ChangeColumnDefinitionSegment> ChangeColumnDefinitions = new LinkedList<ChangeColumnDefinitionSegment>();
    
        public  ICollection<DropColumnDefinitionSegment> DropColumnDefinitions = new LinkedList<DropColumnDefinitionSegment>();
    
        public  ICollection<ConstraintDefinitionSegment> AddConstraintDefinitions = new LinkedList<ConstraintDefinitionSegment>();
        public  ICollection<ValidateConstraintDefinitionSegment> ValidateConstraintDefinitions = new LinkedList<ValidateConstraintDefinitionSegment>();
        public  ICollection<ModifyConstraintDefinitionSegment> ModifyConstraintDefinitions = new LinkedList<ModifyConstraintDefinitionSegment>();
        public  ICollection<DropConstraintDefinitionSegment> DropConstraintDefinitions = new LinkedList<DropConstraintDefinitionSegment>();
        public  ICollection<DropIndexDefinitionSegment> DropIndexDefinitions = new LinkedList<DropIndexDefinitionSegment>();

        public AlterTableCommand(SimpleTableSegment table)
        {
            Table = table;
        }
    }
}