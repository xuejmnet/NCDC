using System;
using System.Collections.Generic;
using NCDC.CommandParser.Segment.Generic.Table;

namespace NCDC.CommandParser.Command.DCL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:29:43
* @Email: 326308290@qq.com
*/
    public sealed class GrantCommand:DCLCommand
    {
        public readonly ICollection<SimpleTableSegment> Tables = new LinkedList<SimpleTableSegment>();
    }
}