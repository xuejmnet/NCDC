using System;
using OpenConnector.CommandParser.Segment.Generic.Table;

namespace OpenConnector.CommandParser.Command.DCL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:28:10
* @Email: 326308290@qq.com
*/
    public sealed class DenyUserCommand:DCLCommand
    {
        public SimpleTableSegment Table { get; set; }
    }
}