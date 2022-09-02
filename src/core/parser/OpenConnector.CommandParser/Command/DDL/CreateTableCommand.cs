using System;
using System.Collections.Generic;
using OpenConnector.CommandParser.Segment.DDL.Column;
using OpenConnector.CommandParser.Segment.DDL.Constraint;
using OpenConnector.CommandParser.Segment.DDL.Index;
using OpenConnector.CommandParser.Segment.Generic.Table;

namespace OpenConnector.CommandParser.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:44:34
* @Email: 326308290@qq.com
*/
    public sealed class CreateTableCommand:DDLCommand
    {
        public  SimpleTableSegment Table { get; }
    
        public readonly ICollection<ColumnDefinitionSegment> ColumnDefinitions = new LinkedList<ColumnDefinitionSegment>();
    
        public readonly ICollection<ConstraintDefinitionSegment> ConstraintDefinitions = new LinkedList<ConstraintDefinitionSegment>();
    
        public readonly ICollection<IndexSegment> Indexes = new LinkedList<IndexSegment>();

        public CreateTableCommand(SimpleTableSegment table)
        {
            Table = table;
        }
    }
}