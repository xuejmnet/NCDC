using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.AbstractParser;
using ShardingConnector.SqlServerParser.SqlLexer;
using ShardingConnector.SqlServerParser.Visitor;

namespace ShardingConnector.SqlServerParser
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/21 13:22:08
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlServerParserConfiguration : ISqlParserConfiguration

    {
        public string GetDataSourceName()
        {
            return "SqlServer";
        }

        public Type GetLexerType()
        {
            return typeof(SqlServerLexer);
        }

        public Type GetParserType()
        {
            return typeof(SqlParser.SqlServerParser);
        }

        public Type GetVisitorFacadeType()
        {
            return typeof(SqlServerVisitorFacade);
        }
    }
}
