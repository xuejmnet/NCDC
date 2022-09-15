using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Abstractions.SqlParser;
using NCDC.CommandParser.SqlParseEngines.Core;


namespace NCDC.MySqlParser.SqlParser
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 8:08:30
    /// Email: 326308290@qq.com
    public sealed class MySqlParser:MySqlCommandParser,ISqlParser
    {
        public MySqlParser(ITokenStream input) : base(input)
        {
        }

        public MySqlParser(ITokenStream input, TextWriter output, TextWriter errorOutput) : base(input, output, errorOutput)
        {
        }

        public IASTNode Parse()
        {
            return new ParseASTNode(execute());
        }
    }
}
