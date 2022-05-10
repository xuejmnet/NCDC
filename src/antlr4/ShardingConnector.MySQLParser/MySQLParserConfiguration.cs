using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.AbstractParser;
using ShardingConnector.MySQLParser.SqlLexer;
using ShardingConnector.MySQLParser.Visitor;

namespace ShardingConnector.MySQLParser
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 9:49:26
    /// Email: 326308290@qq.com
    public sealed class MySQLParserConfiguration:ISqlParserConfiguration

    {
        public string GetDataSourceName()
        {
            return "MySQL";
        }

        public Type GetLexerType()
        {
            return typeof(MySQLLexer);
        }

        public Type GetParserType()
        {
            return typeof(SqlParser.MySQLParser);
        }

        public Type GetVisitorFacadeType()
        {
            return typeof(MySqlVisitorFacade);
        }
    }
}
