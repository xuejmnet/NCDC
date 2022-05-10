using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Tree;
using ShardingConnector.AbstractParser;
using ShardingConnector.AbstractParser.Visitor;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Exceptions;

namespace ShardingConnector.ParserEngine.Core.Visitor
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 11:24:37
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ParseTreeVisitorFactory
    {
        private ParseTreeVisitorFactory()
        {
            
        }

        /** 
         * New instance of SQL visitor.
         * 
         * @param databaseTypeName name of database type
         * @param visitorRule visitor rule
         * @return parse tree visitor
         */
        public static IParseTreeVisitor<IASTNode> NewInstance(string dataSourceName, VisitorRuleEnum visitorRule)
        {
            var sqlParserConfigurations = NewInstanceServiceLoader.NewServiceInstances<ISqlParserConfiguration>();
            foreach (var configuration in sqlParserConfigurations)
            {
                if (configuration.GetDataSourceName().Equals(dataSourceName))
                    return CreateParseTreeVisitor(configuration, VisitorRule.Get(visitorRule).SqlCommandType);
            }


            throw new NotSupportedException($"Cannot support database type '{dataSourceName}'");
        }
    
        private static IParseTreeVisitor<IASTNode> CreateParseTreeVisitor(ISqlParserConfiguration configuration, SqlCommandTypeEnum type)
        {
            ISqlVisitorFacade visitorFacade = configuration.CreateVisitorFacade();
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
