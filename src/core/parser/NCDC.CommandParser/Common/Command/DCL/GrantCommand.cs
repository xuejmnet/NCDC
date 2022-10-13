using System;
using System.Collections.Generic;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DCL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:29:43
* @Email: 326308290@qq.com
*/
    public abstract class GrantCommand:AbstractSqlCommand,IDCLCommand
    {
        public  ICollection<SimpleTableSegment> Tables = new LinkedList<SimpleTableSegment>();
    }
}