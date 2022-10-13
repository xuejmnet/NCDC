using NCDC.CommandParser.Common.Command.DAL;

namespace NCDC.CommandParser.Dialect.Command.MySql.DAL
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:18:51
* @Email: 326308290@qq.com
*/
    public sealed class MySqlUseCommand:UseCommand,IMySqlCommand
    {
        public string Schema { get; }

        public MySqlUseCommand(string schema) : base(schema)
        {
            Schema = schema;
        }
    }
}