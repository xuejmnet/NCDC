// using OpenConnector.Base;
// using OpenConnector.CommandParser.Abstractions;
// using OpenConnector.CommandParser.Command.DML;
// using OpenConnector.CommandParserBinder;
// using OpenConnector.CommandParserBinder.Command;
// using OpenConnector.CommandParserBinder.Command.DML;
// using OpenConnector.CommandParserBinder.MetaData.Schema;
// using OpenConnector.Configuration.Connection.Abstractions;
// using OpenConnector.Configuration.Metadatas;
// using OpenConnector.Exceptions;
// using OpenConnector.Sharding.Contexts;
// using OpenConnector.Sharding.Metadatas;
// using OpenConnector.ShardingAdoNet;
//
// namespace OpenConnector.Sharding.Routes;
//
// public class RouteContextFactory:IRouteContextFactory
// {
//     private static readonly IShardingCommandValidator ShardingInsertCommandValidator = new ShardingInsertCommandValidator();
//     private static readonly IShardingCommandValidator ShardingUpdateCommandValidator = new ShardingUpdateCommandValidator();
//     private readonly ISqlCommandParser _sqlCommandParser;
//     private readonly ITableMetadataManager _tableMetadataManager;
//
//     public RouteContextFactory(ISqlCommandParser sqlCommandParser,ITableMetadataManager tableMetadataManager)
//     {
//         _sqlCommandParser = sqlCommandParser;
//         _tableMetadataManager = tableMetadataManager;
//     }
//     public RouteContext Create(IConnectionSession connectionSession,string sql, ParameterContext parameterContext)
//     {
//         var sqlCommand = _sqlCommandParser.Parse(sql,false);
//         var logicDatabase = connectionSession.LogicDatabase??throw new ShardingException("database not selected");
//         var sqlCommandContext = SqlCommandContextFactory.Create(logicDatabase.SchemaMetaData,sql,parameterContext,sqlCommand);
//         
//         // var tableNames = sqlStatementContext.GetTablesContext().GetTableNames();
//         // if (!shardingRule.TableRules.Any(o => tableNames.Any(t => o.LogicTable.EqualsIgnoreCase(t))))
//         // {
//         //     return routeContext;
//         // }
//         // var parameters = routeContext.GetParameterContext();
//         //todo 校验更新不允许更新分片字段
//         //todo INSERT INTO .... ON DUPLICATE KEY UPDATE can not support update for sharding column.
//         // ShardingCommandValidatorFactory.NewInstance(
//         //         sqlStatementContext.GetSqlCommand())
//         //     .IfPresent(validator=> validator.Validate(shardingRule, sqlStatementContext.GetSqlCommand(), parameters));
//         var shardingRuntimeContext = _shardingRuntimeContextManager.GetShardingRuntimeContext(logicDatabase.Name);
//         
//         ShardingConditions shardingConditions = GetShardingConditions(parameters, sqlStatementContext, metaData.Schema, shardingRuntimeContext);
//         var needMergeShardingValues = IsNeedMergeShardingValues(sqlStatementContext, shardingRule);
//         if (needMergeShardingValues && sqlStatementContext.GetSqlCommand() is DMLCommand)
//         {
//             CheckSubQueryShardingValues(sqlStatementContext, shardingRule, shardingConditions);
//             MergeShardingConditions(shardingConditions);
//         }
//         var shardingRouteEngine = ShardingRouteEngineFactory.NewInstance(shardingRule, metaData, sqlStatementContext, shardingConditions, properties);
//         RouteResult routeResult = shardingRouteEngine.Route(shardingRule);
//         if (needMergeShardingValues)
//         {
//             ShardingAssert.Else(1 == routeResult.GetRouteUnits().Count, "Must have one sharding with sub query.");
//         }
//         return new RouteContext(sqlStatementContext, parameters, routeResult);
//         
//     }
//      public RouteContext Decorate(RouteContext routeContext, OpenConnectorMetaData metaData, ShardingRule shardingRule,
//             ConfigurationProperties properties)
//         {
//             var sqlStatementContext = routeContext.GetSqlCommandContext();
//             // var tableNames = sqlStatementContext.GetTablesContext().GetTableNames();
//             // if (!shardingRule.TableRules.Any(o => tableNames.Any(t => o.LogicTable.EqualsIgnoreCase(t))))
//             // {
//             //     return routeContext;
//             // }
//             var parameters = routeContext.GetParameterContext();
//             ShardingCommandValidatorFactory.NewInstance(
//                 sqlStatementContext.GetSqlCommand())
//                 .IfPresent(validator=> validator.Validate(shardingRule, sqlStatementContext.GetSqlCommand(), parameters));
//             ShardingConditions shardingConditions = GetShardingConditions(parameters, sqlStatementContext, metaData.Schema);
//             var needMergeShardingValues = IsNeedMergeShardingValues(sqlStatementContext, shardingRule);
//             if (needMergeShardingValues && sqlStatementContext.GetSqlCommand() is DMLCommand)
//             {
//                 CheckSubQueryShardingValues(sqlStatementContext, shardingRule, shardingConditions);
//                 MergeShardingConditions(shardingConditions);
//             }
//             var shardingRouteEngine = ShardingRouteEngineFactory.NewInstance(shardingRule, metaData, sqlStatementContext, shardingConditions, properties);
//             RouteResult routeResult = shardingRouteEngine.Route(shardingRule);
//             if (needMergeShardingValues)
//             {
//                 ShardingAssert.Else(1 == routeResult.GetRouteUnits().Count, "Must have one sharding with sub query.");
//             }
//             return new RouteContext(sqlStatementContext, parameters, routeResult);
//         }
//
//         private ShardingConditions GetShardingConditions(ParameterContext parameterContext,
//                                                          ISqlCommandContext<ISqlCommand> sqlStatementContext, SchemaMetaData schemaMetaData)
//         {
//             if (sqlStatementContext.GetSqlCommand() is DMLCommand) {
//                 if (sqlStatementContext is InsertCommandContext insertCommandContext) {
//                     return new ShardingConditions(new InsertClauseShardingConditionEngine(shardingRule).CreateShardingConditions(insertCommandContext, parameterContext));
//                 }
//                 return new ShardingConditions(new WhereClauseShardingConditionEngine(shardingRule, schemaMetaData).CreateShardingConditions(sqlStatementContext, parameterContext));
//             }
//             return new ShardingConditions(new List<ShardingCondition>(0));
//         }
//
//         private bool IsNeedMergeShardingValues(ISqlCommandContext<ISqlCommand> sqlStatementContext, ShardingRule shardingRule)
//         {
//             return sqlStatementContext is SelectCommandContext selectCommandContext && selectCommandContext.IsContainsSubQuery()
//                     && shardingRule.GetShardingLogicTableNames(sqlStatementContext.GetTablesContext().GetTableNames()).Any();
//         }
//
//         private void CheckSubQueryShardingValues(ISqlCommandContext<ISqlCommand> sqlStatementContext, ShardingRule shardingRule, ShardingConditions shardingConditions)
//         {
//             foreach (var tableName in sqlStatementContext.GetTablesContext().GetTableNames())
//             {
//                 var tableRule = shardingRule.FindTableRule(tableName);
//                 if (tableRule!=null && IsRoutingByHint(shardingRule, tableRule)
//                                           && HintManager.GetDatabaseShardingValues(tableName).Any() && HintManager.GetTableShardingValues(tableName).Any())
//                 {
//                     return;
//                 }
//             }
//             ShardingAssert.If(shardingConditions.Conditions.IsEmpty(), "Must have sharding column with subquery.");
//             if (shardingConditions.Conditions.Count > 1)
//             {
//                 ShardingAssert.Else(IsSameShardingCondition(shardingRule, shardingConditions), "Sharding value must same with subquery.");
//             }
//         }
//
//         private bool IsRoutingByHint(ShardingRule shardingRule, TableRule tableRule)
//         {
//             return shardingRule.GetDatabaseShardingStrategy(tableRule) is HintShardingStrategy && shardingRule.GetTableShardingStrategy(tableRule) is HintShardingStrategy;
//         }
//
//         private bool IsSameShardingCondition(ShardingRule shardingRule, ShardingConditions shardingConditions)
//         {
//             ShardingCondition example = shardingConditions.Conditions.Last();
//             shardingConditions.Conditions.RemoveAt(shardingConditions.Conditions.Count-1);
//             foreach (var condition in shardingConditions.Conditions)
//             {
//                 if (!IsSameShardingCondition(shardingRule, example, condition))
//                 {
//                     return false;
//                 }
//             }
//             return true;
//         }
//
//         private bool IsSameShardingCondition(ShardingRule shardingRule, ShardingCondition shardingCondition1, ShardingCondition shardingCondition2)
//         {
//             if (shardingCondition1.RouteValues.Count != shardingCondition2.RouteValues.Count)
//             {
//                 return false;
//             }
//             for (int i = 0; i < shardingCondition1.RouteValues.Count; i++)
//             {
//                 var shardingValue1 = shardingCondition1.RouteValues.ElementAt(i);
//                 var shardingValue2 = shardingCondition2.RouteValues.ElementAt(i);
//                 if (!IsSameRouteValue(shardingRule, (ListRouteValue)shardingValue1, (ListRouteValue)shardingValue2))
//                 {
//                     return false;
//                 }
//             }
//             return true;
//         }
//
//         private bool IsSameRouteValue(ShardingRule shardingRule, ListRouteValue routeValue1, ListRouteValue routeValue2)
//         {
//             return IsSameLogicTable(shardingRule, routeValue1, routeValue2) && routeValue1.GetColumnName().Equals(routeValue2.GetColumnName()) && routeValue1.GetValues().SequenceEqual(routeValue2.GetValues());
//         }
//
//         private bool IsSameLogicTable(ShardingRule shardingRule, ListRouteValue shardingValue1, ListRouteValue shardingValue2)
//         {
//             return shardingValue1.GetTableName().Equals(shardingValue2.GetTableName()) || IsBindingTable(shardingRule, shardingValue1, shardingValue2);
//         }
//
//         private bool IsBindingTable(ShardingRule shardingRule, ListRouteValue shardingValue1, ListRouteValue shardingValue2)
//         {
//             var bindingRule = shardingRule.FindBindingTableRule(shardingValue1.GetTableName());
//             return bindingRule!=null && bindingRule.HasLogicTable(shardingValue2.GetTableName());
//         }
//
//         private void MergeShardingConditions(ShardingConditions shardingConditions)
//         {
//             if (shardingConditions.Conditions.Count > 1)
//             {
//                 ShardingCondition shardingCondition =
//                     shardingConditions.Conditions[shardingConditions.Conditions.Count - 1];
//                 shardingConditions.Conditions.RemoveAt(shardingConditions.Conditions.Count - 1);
//                 shardingConditions.Conditions.Clear();
//                 shardingConditions.Conditions.Add(shardingCondition);
//             }
//         }
// }