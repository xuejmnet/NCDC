using System;
using Antlr4.Runtime.Tree;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Abstractions.Visitor;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParser.SqlParseEngines.Core.Parser;
using OpenConnector.Exceptions;
using OpenConnector.ParserEngine.Core.Visitor;
using OpenConnector.ParserEngine.Hook;


namespace OpenConnector.CommandParser.SqlParseEngines
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/7 8:22:10
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class SqlCommandParser:ISqlCommandParser
    {
        private readonly ISqlParserConfiguration _sqlParserConfiguration;
        // private static readonly SqlParseResultCache _cache = new SqlParseResultCache();
        private readonly SqlParserExecutor _sqlParserExecutor;
        public SqlCommandParser(ISqlParserConfiguration sqlParserConfiguration)
        {
            _sqlParserConfiguration = sqlParserConfiguration;
            _sqlParserExecutor = new SqlParserExecutor(sqlParserConfiguration);
        }
        public ISqlCommand Parse(string sql,bool useCache)
        {
            var parsingHookManager = ParsingHookManager.GetInstance();
            parsingHookManager.Start(sql);
            try
            {
                ISqlCommand result = Parse0(sql, useCache);
                parsingHookManager.FinishSuccess(result);
                return result;

            }
            catch(Exception e)
            {
                parsingHookManager.FinishFailure(e);
                throw e;
            }
        }

        private ISqlCommand Parse0(string sql, bool useCache)
        {
            // if (useCache)
            // {
            //     var sqlCommand = _cache.GetSqlCommand(sql);
            //     if (sqlCommand != null)
            //         return sqlCommand;
            // }
            IParseTree parseTree = _sqlParserExecutor.Parse(sql).GetRootNode();
            var visitorRule = VisitorRule.ValueOf(parseTree.GetType());
            var parseTreeVisitor = CreateParseTreeVisitor(_sqlParserConfiguration,VisitorRule.Get(visitorRule).SqlCommandType);
            ISqlCommand result = (ISqlCommand)parseTreeVisitor.Visit(parseTree);
            // if (useCache)
            // {
            //     _cache.Add(sql, result);
            // }
            return result;
        }
        private  IParseTreeVisitor<IASTNode> CreateParseTreeVisitor(ISqlParserConfiguration configuration, SqlCommandTypeEnum type)
        {
            ISqlVisitorCreator visitorFacade = configuration.CreateVisitorCreator();
            switch (type)
            {
                case SqlCommandTypeEnum.DML:
                    return (IParseTreeVisitor<IASTNode>)visitorFacade.CreateDMLVisitor();
                case SqlCommandTypeEnum.DDL:
                    return (IParseTreeVisitor<IASTNode>)visitorFacade.CreateDDLVisitor();
                case SqlCommandTypeEnum.TCL:
                    return (IParseTreeVisitor<IASTNode>)visitorFacade.CreateTCLVisitor();
                case SqlCommandTypeEnum.DCL:
                    return (IParseTreeVisitor<IASTNode>)visitorFacade.CreateDCLVisitor();
                case SqlCommandTypeEnum.DAL:
                    return (IParseTreeVisitor<IASTNode>)visitorFacade.CreateDALVisitor();
                case SqlCommandTypeEnum.RL:
                    return (IParseTreeVisitor<IASTNode>)visitorFacade.CreateRLVisitor();
                default:
                    throw new ShardingSqlParsingException($"Can not support SQL statement type: `{type}`");
            }
        }
        
    }
}
