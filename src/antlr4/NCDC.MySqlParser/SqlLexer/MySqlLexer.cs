﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using NCDC.CommandParser.Abstractions.SqlLexer;

namespace NCDC.MySqlParser.SqlLexer
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 8:07:46
    /// Email: 326308290@qq.com
    public sealed class MySqlLexer:MySqlCommandLexer, ISqlLexer
    {
        public MySqlLexer(ICharStream input) : base(input)
        {
        }

        public MySqlLexer(ICharStream input, TextWriter output, TextWriter errorOutput) : base(input, output, errorOutput)
        {
        }
    }
}
