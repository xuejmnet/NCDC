using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Abstractions.SqlParser;
using OpenConnector.CommandParser.Abstractions.Visitor;

using OpenConnector.SqlServerParser.SqlLexer;
using OpenConnector.SqlServerParser.Visitor;

namespace OpenConnector.SqlServerParser
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
