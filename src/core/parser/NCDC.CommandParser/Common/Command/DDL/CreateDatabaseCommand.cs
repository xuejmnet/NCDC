using System;

namespace NCDC.CommandParser.Common.Command.DDL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:42:43
* @Email: 326308290@qq.com
*/
    public abstract class CreateDatabaseCommand:AbstractSqlCommand,IDDLCommand
    {
        protected CreateDatabaseCommand(string databaseName, bool ifNotExists)
        {
            DatabaseName = databaseName;
            IfNotExists = ifNotExists;
        }

        public string DatabaseName { get; }
        public bool IfNotExists { get; }
    }
}