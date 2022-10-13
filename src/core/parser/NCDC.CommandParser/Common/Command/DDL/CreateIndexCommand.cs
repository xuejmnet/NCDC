using System;
using NCDC.CommandParser.Common.Segment.DDL.Index;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:43:57
* @Email: 326308290@qq.com
*/
    public abstract class CreateIndexCommand:AbstractSqlCommand,IDDLCommand
    {
        
        public IndexSegment Index { get; }
    
        public SimpleTableSegment Table{ get;}
        public ICollection<ColumnSegment> Columns = new LinkedList<ColumnSegment>();

        protected CreateIndexCommand(IndexSegment index, SimpleTableSegment table)
        {
            Index = index;
            Table = table;
        }
    }
}