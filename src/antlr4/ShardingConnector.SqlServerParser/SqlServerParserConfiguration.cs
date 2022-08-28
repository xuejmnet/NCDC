using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using ShardingConnector.Parsers;
using ShardingConnector.Parsers.SqlParser;
using ShardingConnector.Parsers.Visitor;
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
        public ISqlParser CreateSqlParser(string sql)
        {
            var charStream = CharStreams.fromString(sql);
            var mySqlLexer = new SqlServerLexer(charStream);
            var commonTokenStream = new CommonTokenStream(mySqlLexer);
            return new SqlParser.SqlServerParser(commonTokenStream);
        }

        public ISqlVisitorCreator CreateVisitorCreator()
        {
            return new SqlServerVisitorCreator();
        }

    }
}
