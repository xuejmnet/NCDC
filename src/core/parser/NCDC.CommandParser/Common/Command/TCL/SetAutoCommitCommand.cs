using System;

namespace NCDC.CommandParser.Common.Command.TCL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 22:17:53
* @Email: 326308290@qq.com
*/
    public abstract class SetAutoCommitCommand: AbstractSqlCommand, ITCLCommand
    {
        public bool AutoCommit { get; set; }
    }
}