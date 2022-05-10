using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using ShardingConnector.AbstractParser.SqlLexer;
using ShardingConnector.SqlServerParser;

namespace ShardingConnector.MySQLParser.SqlLexer
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 8:07:46
    /// Email: 326308290@qq.com
    public sealed class MySQLLexer:MySQLCommandLexer, ISqlLexer
    {
        public MySQLLexer(ICharStream input) : base(input)
        {
        }

        public MySQLLexer(ICharStream input, TextWriter output, TextWriter errorOutput) : base(input, output, errorOutput)
        {
        }
    }
}
