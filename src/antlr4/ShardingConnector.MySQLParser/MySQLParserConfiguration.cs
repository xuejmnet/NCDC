using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using ShardingConnector.AbstractParser;
using ShardingConnector.AbstractParser.SqlLexer;
using ShardingConnector.AbstractParser.SqlParser;
using ShardingConnector.AbstractParser.Visitor;
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

        public ISqlParser CreateSqlParser(string sql)
        {
            var charStream = CharStreams.fromString(sql);
            var mySqlLexer = new MySQLLexer(charStream);
            var commonTokenStream = new CommonTokenStream(mySqlLexer);
            return new SqlParser.MySQLParser(commonTokenStream);
        }

        public ISqlVisitorFacade CreateVisitorFacade()
        {
            return new MySQLVisitorFacade();
        }
    }
}
