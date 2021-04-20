using System;
using System.Collections.Generic;
using ShardingConnector.CommandParser.Segment.DDL.Column;
using ShardingConnector.CommandParser.Segment.DDL.Constraint;
using ShardingConnector.CommandParser.Segment.DDL.Index;
using ShardingConnector.CommandParser.Segment.Generic.Table;

namespace ShardingConnector.CommandParser.Command.DDL
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