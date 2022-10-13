using System;
using NCDC.CommandParser.Common.Segment.DDL.Index;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:37:55
* @Email: 326308290@qq.com
*/
    public abstract class AlterIndexCommand:AbstractSqlCommand,IDDLCommand
    {
        
        public IndexSegment? Index { get; set; }
    }
}