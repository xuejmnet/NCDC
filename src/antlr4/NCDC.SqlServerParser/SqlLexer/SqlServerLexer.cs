using System.IO;
using Antlr4.Runtime;
using NCDC.CommandParser.Abstractions.SqlLexer;

namespace OpenConnector.SqlServerParser.SqlLexer
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 8:57:12
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlServerLexer:SqlServerCommandLexer,ISqlLexer
    {
        public SqlServerLexer(ICharStream input) : base(input)
        {
        }

        public SqlServerLexer(ICharStream input, TextWriter output, TextWriter errorOutput) : base(input, output, errorOutput)
        {
        }
    }
}
