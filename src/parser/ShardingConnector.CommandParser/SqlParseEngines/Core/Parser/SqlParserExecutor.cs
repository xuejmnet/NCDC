using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ShardingConnector.Exceptions;
using ShardingConnector.ParserEngine.Core;
using ShardingConnector.Parsers;
using ShardingConnector.Parsers.SqlParser;

namespace ShardingConnector.CommandParser.SqlParseEngines.Core.Parser
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/7 8:29:31
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlParserExecutor
    {
        private readonly ISqlParserConfiguration _sqlParserConfiguration;

        public SqlParserExecutor(ISqlParserConfiguration sqlParserConfiguration)
        {
            _sqlParserConfiguration = sqlParserConfiguration;
        }
        /**
         * Parse to parse SQL.
         *
         * @return AST node
         */
        public ParseASTNode Parse(string sql)
        {
            ParseASTNode result = Parse0(sql);
            if (result.GetRootNode() is IErrorNode)
            {
                throw new ShardingSqlParsingException($"Unsupported SQL of `{sql}`");
            }
            return result;
        }

        private ParseASTNode Parse0(string sql)
        {

            ISqlParser sqlParser = _sqlParserConfiguration.CreateSqlParser(sql);
            try
            {
                ((Antlr4.Runtime.Parser)sqlParser).ErrorHandler = new BailErrorStrategy();
                ((Antlr4.Runtime.Parser)sqlParser).Interpreter.PredictionMode = PredictionMode.LL;
                return (ParseASTNode)sqlParser.Parse();
            }
            catch (ParseCanceledException ex)
            {
                ((Antlr4.Runtime.Parser)sqlParser).Reset();
                ((Antlr4.Runtime.Parser)sqlParser).ErrorHandler = new DefaultErrorStrategy();
                ((Antlr4.Runtime.Parser)sqlParser).Interpreter.PredictionMode = PredictionMode.LL;
                return (ParseASTNode)sqlParser.Parse();
            }
        }
    }
}
