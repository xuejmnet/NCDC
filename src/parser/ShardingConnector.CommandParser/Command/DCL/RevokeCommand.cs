using System;
using System.Collections.Generic;
using ShardingConnector.CommandParser.Segment.Generic.Table;

namespace ShardingConnector.CommandParser.Command.DCL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:30:45
* @Email: 326308290@qq.com
*/
    public sealed class RevokeCommand:DCLCommand
    {
        public readonly ICollection<SimpleTableSegment> Tables = new LinkedList<SimpleTableSegment>();
    }
}