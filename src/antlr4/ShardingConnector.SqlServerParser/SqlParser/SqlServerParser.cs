using System;
using System.IO;
using Antlr4.Runtime;
using ShardingConnector.ParserEngine.Core;
using ShardingConnector.Parsers;
using ShardingConnector.Parsers.SqlParser;

namespace ShardingConnector.SqlServerParser.SqlParser
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 8:58:41
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlServerParser:SqlServerCommandParser,ISqlParser
    {
        public SqlServerParser(ITokenStream input) : base(input)
        {
        }

        public SqlServerParser(ITokenStream input, TextWriter output, TextWriter errorOutput) : base(input, output, errorOutput)
        {
        }

        public IASTNode Parse()
        {
            return new ParseASTNode(execute());
        }
    }
}
