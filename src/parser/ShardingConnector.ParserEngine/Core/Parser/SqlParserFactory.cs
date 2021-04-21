using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using ShardingConnector.AbstractParser;
using ShardingConnector.AbstractParser.SqlParser;

namespace ShardingConnector.ParserEngine.Core.Parser
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 10:11:44
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlParserFactory
    {
        static SqlParserFactory()
        {
            NewInstanceServiceLoader.Register<ISqlParserConfiguration>();
        }
        private SqlParserFactory()
        {

        }
        public static ISqlParser NewInstance(string dataSourceName, string sql)
        {
            var sqlParserConfigurations = NewInstanceServiceLoader.NewServiceInstances<ISqlParserConfiguration>();

            foreach (var configuration in sqlParserConfigurations)
            {
                if(configuration.GetDataSourceName().Equals(dataSourceName))
                    return CreateSqlParser(sql, configuration);
}
            throw new NotSupportedException($"Cannot support database type '{dataSourceName}'");
        }
    
        private static ISqlParser CreateSqlParser(string sql, ISqlParserConfiguration configuration)
        {
            var lexerType = configuration.GetLexerType();
            Lexer lexer = (Lexer)lexerType.GetConstructor(new[] { typeof(ICharStream) })?.Invoke(new object[] { CharStreams.fromString(sql) });
            var parserType = configuration.GetParserType();
            return (ISqlParser)parserType.GetConstructor(new[] { typeof(ITokenStream) })?.Invoke(new object[] { new CommonTokenStream(lexer) });
        }
    }
}
