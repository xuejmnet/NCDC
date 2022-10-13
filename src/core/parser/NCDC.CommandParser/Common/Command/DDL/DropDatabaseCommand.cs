using System;

namespace NCDC.CommandParser.Common.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:50:40
* @Email: 326308290@qq.com
*/
    public abstract class DropDatabaseCommand:AbstractSqlCommand,IDDLCommand
    {
        protected DropDatabaseCommand(string databaseName, bool ifExists)
        {
            DatabaseName = databaseName;
            IfExists = ifExists;
        }

        public string DatabaseName { get; }
        public bool IfExists { get;}
    }
}