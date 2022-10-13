using System;

namespace NCDC.CommandParser.Common.Command.TCL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 22:17:36
* @Email: 326308290@qq.com
*/
    public abstract class SavepointCommand: AbstractSqlCommand, ITCLCommand
    {
        protected SavepointCommand(string savepointName)
        {
            SavepointName = savepointName;
        }

        public string SavepointName { get; }
    }
}