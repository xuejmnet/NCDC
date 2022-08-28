using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using ShardingConnector.MySqlParser.SqlLexer;
using ShardingConnector.MySqlParser.Visitor;
using ShardingConnector.Parsers;
using ShardingConnector.Parsers.SqlParser;
using ShardingConnector.Parsers.Visitor;

namespace ShardingConnector.MySqlParser
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 9:49:26
    /// Email: 326308290@qq.com
    public sealed class MySqlParserConfiguration:ISqlParserConfiguration
    {

        public ISqlParser CreateSqlParser(string sql)
        {
            var charStream = CharStreams.fromString(sql);
            var mySqlLexer = new MySqlLexer(charStream);
            var commonTokenStream = new CommonTokenStream(mySqlLexer);
            return new SqlParser.MySqlParser(commonTokenStream);
        }

        public ISqlVisitorCreator CreateVisitorCreator()
        {
            return new MySqlVisitorCreator();
        }

    }
}
