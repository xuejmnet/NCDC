using System;

namespace NCDC.CommandParser.Common.Command.TCL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 22:17:19
* @Email: 326308290@qq.com
*/
    public abstract class RollbackCommand: AbstractSqlCommand, ITCLCommand
    {
        public string? SavepointName { get; set; }
    }
}