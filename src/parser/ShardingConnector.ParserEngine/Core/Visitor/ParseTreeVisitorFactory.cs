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
        public static IParseTreeVisitor<object> NewInstance(string dataSourceName, VisitorRuleEnum visitorRule)
        {
            var sqlParserConfigurations = NewInstanceServiceLoader.NewServiceInstances<ISqlParserConfiguration>();
            foreach (var configuration in sqlParserConfigurations)
            {
                if (configuration.GetDataSourceName().Equals(dataSourceName))
                    return CreateParseTreeVisitor(configuration, VisitorRule.Get(visitorRule).SqlCommandType);
            }


            throw new NotSupportedException($"Cannot support database type '{dataSourceName}'");
        }
    
        private static IParseTreeVisitor<object> CreateParseTreeVisitor(ISqlParserConfiguration configuration, SqlCommandTypeEnum type)
        {
            ISqlVisitorFacade visitorFacade = (ISqlVisitorFacade) Activator.CreateInstance(configuration.GetVisitorFacadeType());
            switch (type)
            {
                case SqlCommandTypeEnum.DML:
                    return (IParseTreeVisitor<object>)Activator.CreateInstance(visitorFacade.GetDMLVisitorType());
                case SqlCommandTypeEnum.DDL:
                    return (IParseTreeVisitor<object>)Activator.CreateInstance(visitorFacade.GetDDLVisitorType());
                case SqlCommandTypeEnum.TCL:
                    return (IParseTreeVisitor<object>)Activator.CreateInstance(visitorFacade.GetTCLVisitorType());
                case SqlCommandTypeEnum.DCL:
                    return (IParseTreeVisitor<object>)Activator.CreateInstance(visitorFacade.GetDCLVisitorType());
                case SqlCommandTypeEnum.DAL:
                    return (IParseTreeVisitor<object>)Activator.CreateInstance(visitorFacade.GetDALVisitorType());
                case SqlCommandTypeEnum.RL:
                    return (IParseTreeVisitor<object>)Activator.CreateInstance(visitorFacade.GetRLVisitorType());
                default:
                    throw new ShardingSqlParsingException($"Can not support SQL statement type: `{type}`");
            }
        }
    }
}
