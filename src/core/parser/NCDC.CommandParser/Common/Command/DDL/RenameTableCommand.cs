using System;
using NCDC.CommandParser.Common.Segment.DDL.Table;

namespace NCDC.CommandParser.Common.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:57:34
* @Email: 326308290@qq.com
*/
    public abstract class RenameTableCommand:AbstractSqlCommand,IDDLCommand
    {
        public ICollection<RenameTableDefinitionSegment> RenameTables = new LinkedList<RenameTableDefinitionSegment>();
    }
}