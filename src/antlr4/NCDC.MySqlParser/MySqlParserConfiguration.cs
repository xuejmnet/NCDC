using Antlr4.Runtime;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Abstractions.SqlParser;
using NCDC.CommandParser.Abstractions.Visitor;
using NCDC.Enums;
using NCDC.MySqlParser.SqlLexer;
using NCDC.MySqlParser.Visitor;

namespace NCDC.MySqlParser
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 9:49:26
    /// Email: 326308290@qq.com
    public sealed class MySqlParserConfiguration:ISqlParserConfiguration
    {
        private readonly ISqlVisitorCreator _sqlVisitorCreator;
        public MySqlParserConfiguration()
        {
            _sqlVisitorCreator=new MySqlVisitorCreator();
        }
        public ISqlParser CreateSqlParser(string sql)
        {
            var charStream = CharStreams.fromString(sql);
            var mySqlLexer = new MySqlLexer(charStream);
            var commonTokenStream = new CommonTokenStream(mySqlLexer);
            return new SqlParser.MySqlParser(commonTokenStream);
        }

        public ISqlVisitorCreator CreateVisitorCreator()
        {
            return _sqlVisitorCreator;
        }
    }
}
