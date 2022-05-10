using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using ShardingConnector.AbstractParser;
using ShardingConnector.AbstractParser.SqlParser;
using ShardingConnector.ParserEngine.Core;
using ShardingConnector.SqlServerParser;

namespace ShardingConnector.MySQLParser.SqlParser
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 8:08:30
    /// Email: 326308290@qq.com
    public sealed class MySQLParser:MySQLCommandParser,ISqlParser
    {
        public MySQLParser(ITokenStream input) : base(input)
        {
        }

        public MySQLParser(ITokenStream input, TextWriter output, TextWriter errorOutput) : base(input, output, errorOutput)
        {
        }

        public IASTNode Parse()
        {
            return new ParseASTNode(execute());
        }
    }
}
