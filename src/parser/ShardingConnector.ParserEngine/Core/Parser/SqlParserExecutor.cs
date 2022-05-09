using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ShardingConnector.AbstractParser.SqlParser;
using ShardingConnector.Exceptions;
using ShardingConnector.Extensions;

namespace ShardingConnector.ParserEngine.Core.Parser
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
        private readonly string _dataSourceName;
        private readonly string _sql;

        public SqlParserExecutor(string dataSourceName, string sql)
        {
            _dataSourceName = dataSourceName;
            _sql = sql;
        }
        /**
         * Execute to parse SQL.
         *
         * @return AST node
         */
        public ParseASTNode Execute()
        {
            ParseASTNode result = TowPhaseParse();
            if (result.GetRootNode() is IErrorNode)
            {
                throw new ShardingSqlParsingException($"Unsupported SQL of `{_sql}`");
            }
            return result;
        }

        private ParseASTNode TowPhaseParse()
        {

            ISqlParser sqlParser = SqlParserFactory.NewInstance(_dataSourceName, _sql);
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
