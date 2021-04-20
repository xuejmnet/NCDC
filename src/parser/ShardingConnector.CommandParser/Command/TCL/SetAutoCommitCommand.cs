using System;

namespace ShardingConnector.CommandParser.Command.TCL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 22:17:53
* @Email: 326308290@qq.com
*/
    public sealed class SetAutoCommitCommand:TCLCommand
    {
        public bool AutoCommit { get; set; }
    }
}