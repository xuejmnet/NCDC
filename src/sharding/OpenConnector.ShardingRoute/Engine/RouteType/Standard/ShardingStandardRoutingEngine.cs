using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;

using OpenConnector.Base;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParser.Command.DML;
using OpenConnector.Common.Config.Properties;
using OpenConnector.Common.Rule;
using OpenConnector.Exceptions;
using OpenConnector.Extensions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.Route.Context;
using OpenConnector.ShardingApi.Api.Hint;
using OpenConnector.ShardingCommon.Core.Rule;
using OpenConnector.ShardingCommon.Core.Strategy.Route;
using OpenConnector.ShardingCommon.Core.Strategy.Route.Hint;
using OpenConnector.ShardingCommon.Core.Strategy.Route.Value;
using OpenConnector.ShardingRoute.Engine.Condition;

namespace OpenConnector.ShardingRoute.Engine.RouteType.Standard
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Wednesday, 28 April 2021 22:15:46
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingStandardRoutingEngine : IShardingRouteEngine
    {
        public  String LogicTableName { get; }

        public ISqlCommandContext<ISqlCommand> SqlCommandContext { get; }

        public ShardingConditions ShardingConditions { get; }

        public ConfigurationProperties Properties { get; }

        public readonly ICollection<ICollection<DataNode>> OriginalDataNodes = new LinkedList<ICollection<DataNode>>();

        public ShardingStandardRoutingEngine(string logicTableName, ISqlCommandContext<ISqlCommand> sqlCommandContext, ShardingConditions shardingConditions, ConfigurationProperties properties)
        {
            this.LogicTableName = logicTableName;
            this.SqlCommandContext = sqlCommandContext;
            this.ShardingConditions = shardingConditions;
            this.Properties = properties;
        }

        public RouteResult Route(ShardingRule shardingRule)
        {
            if (IsDMLForModify(SqlCommandContext) && 1 != ((ITableAvailable)SqlCommandContext).GetAllTables().Count)
            {
                throw new ShardingException($"Cannot support Multiple-Table for '{SqlCommandContext.GetSqlCommand()}'.");
            }
            return GenerateRouteResult(GetDataNodes(shardingRule, shardingRule.GetTableRule(LogicTableName)));
        }

        private bool IsDMLForModify(ISqlCommandContext<ISqlCommand> sqlStatementContext)
        {
            return sqlStatementContext is InsertCommandContext || sqlStatementContext is UpdateCommandContext || sqlStatementContext is DeleteCommandContext;
        }

        private RouteResult GenerateRouteResult(ICollection<DataNode> routedDataNodes)
        {
            RouteResult result = new RouteResult();
            result.GetOriginalDataNodes().AddAll(OriginalDataNodes);
            foreach (var routedDataNode in routedDataNodes)
            {
                result.GetRouteUnits().Add(
                        new RouteUnit(new RouteMapper(routedDataNode.GetDataSourceName(), routedDataNode.GetDataSourceName()), new List<RouteMapper>(){ new RouteMapper(LogicTableName, routedDataNode.GetTableName()) }));

            }
            return result;
        }

        private ICollection<DataNode> GetDataNodes(ShardingRule shardingRule, TableRule tableRule)
        {
            if (IsRoutingByHint(shardingRule, tableRule))
            {
                return RouteByHint(shardingRule, tableRule);
            }
            if (IsRoutingByShardingConditions(shardingRule, tableRule))
            {
                return RouteByShardingConditions(shardingRule, tableRule);
            }
            return RouteByMixedConditions(shardingRule, tableRule);
        }

        private bool IsRoutingByHint(ShardingRule shardingRule, TableRule tableRule)
        {
            return shardingRule.GetDatabaseShardingStrategy(tableRule) is HintShardingStrategy && shardingRule.GetTableShardingStrategy(tableRule) is HintShardingStrategy;
        }

        private ICollection<DataNode> RouteByHint(ShardingRule shardingRule, TableRule tableRule)
        {
            return Route0(shardingRule, tableRule, GetDatabaseShardingValuesFromHint(), GetTableShardingValuesFromHint());
        }

        private bool IsRoutingByShardingConditions(ShardingRule shardingRule, TableRule tableRule)
        {
            return !(shardingRule.GetDatabaseShardingStrategy(tableRule) is HintShardingStrategy  || shardingRule.GetTableShardingStrategy(tableRule) is HintShardingStrategy);
        }

        private ICollection<DataNode> RouteByShardingConditions(ShardingRule shardingRule, TableRule tableRule)
        {
            return ShardingConditions.Conditions.IsEmpty()
                    ? Route0(shardingRule, tableRule, new List<IRouteValue>(0), new ArrayList<IRouteValue>(0)) : RouteByShardingConditionsWithCondition(shardingRule, tableRule);
        }

        private ICollection<DataNode> RouteByShardingConditionsWithCondition(ShardingRule shardingRule, TableRule tableRule)
        {
            ICollection<DataNode> result = new LinkedList<DataNode>();
            foreach (var condition in ShardingConditions.Conditions)
            {
                ICollection<DataNode> dataNodes = Route0(shardingRule, tableRule,
                    GetShardingValuesFromShardingConditions(shardingRule, shardingRule.GetDatabaseShardingStrategy(tableRule).GetShardingColumns(), condition),
                    GetShardingValuesFromShardingConditions(shardingRule, shardingRule.GetTableShardingStrategy(tableRule).GetShardingColumns(), condition));
                result.AddAll(dataNodes);
                OriginalDataNodes.Add(dataNodes);
            }
            return result;
        }

        private ICollection<DataNode> RouteByMixedConditions(ShardingRule shardingRule, TableRule tableRule)
        {
            return ShardingConditions.Conditions.IsEmpty() ? RouteByMixedConditionsWithHint(shardingRule, tableRule) :RouteByMixedConditionsWithCondition(shardingRule, tableRule);
        }

        private ICollection<DataNode> RouteByMixedConditionsWithCondition(ShardingRule shardingRule, TableRule tableRule)
        {
            ICollection<DataNode> result = new LinkedList<DataNode>();
            foreach (var condition in ShardingConditions.Conditions)
            {
                ICollection<DataNode> dataNodes = Route0(shardingRule, tableRule, GetDatabaseShardingValues(shardingRule, tableRule, condition), GetTableShardingValues(shardingRule, tableRule, condition));
                result.AddAll(dataNodes);
                OriginalDataNodes.Add(dataNodes);
            }
            return result;
        }

        private ICollection<DataNode> RouteByMixedConditionsWithHint(ShardingRule shardingRule, TableRule tableRule)
        {
            if (shardingRule.GetDatabaseShardingStrategy(tableRule) is HintShardingStrategy) {
                return Route0(shardingRule, tableRule, GetDatabaseShardingValuesFromHint(), new List<IRouteValue>(0));
            }
            return Route0(shardingRule, tableRule, new ArrayList<IRouteValue>(0), GetTableShardingValuesFromHint());
        }

        private List<IRouteValue> GetDatabaseShardingValues(ShardingRule shardingRule, TableRule tableRule, ShardingCondition shardingCondition)
        {
            var dataBaseShardingStrategy = shardingRule.GetDatabaseShardingStrategy(tableRule);
            return IsGettingShardingValuesFromHint(dataBaseShardingStrategy)
                    ? GetDatabaseShardingValuesFromHint() : GetShardingValuesFromShardingConditions(shardingRule, dataBaseShardingStrategy.GetShardingColumns(), shardingCondition);
        }

        private List<IRouteValue> GetTableShardingValues(ShardingRule shardingRule, TableRule tableRule, ShardingCondition shardingCondition)
        {
            IShardingStrategy tableShardingStrategy = shardingRule.GetTableShardingStrategy(tableRule);
            return IsGettingShardingValuesFromHint(tableShardingStrategy)
                    ? GetTableShardingValuesFromHint() : GetShardingValuesFromShardingConditions(shardingRule, tableShardingStrategy.GetShardingColumns(), shardingCondition);
        }

        private bool IsGettingShardingValuesFromHint(IShardingStrategy shardingStrategy)
        {
            return shardingStrategy is HintShardingStrategy;
        }

        private List<IRouteValue> GetDatabaseShardingValuesFromHint()
        {
            return GetRouteValues(HintManager.IsDatabaseShardingOnly() ? HintManager.GetDatabaseShardingValues() : HintManager.GetDatabaseShardingValues(LogicTableName));
        }

        private List<IRouteValue> GetTableShardingValuesFromHint()
        {
            return GetRouteValues(HintManager.GetTableShardingValues(LogicTableName));
        }

        private List<IRouteValue> GetRouteValues(ICollection<IComparable> shardingValue)
        {
            return shardingValue.IsEmpty() ?new List<IRouteValue>(0) : new List<IRouteValue>(){ new ListRouteValue("", LogicTableName, shardingValue) };
        }

        private List<IRouteValue> GetShardingValuesFromShardingConditions(ShardingRule shardingRule, ICollection<string> shardingColumns, ShardingCondition shardingCondition)
        {
            List<IRouteValue> result = new List<IRouteValue>(shardingColumns.Count);
            foreach (var routeValue in shardingCondition.RouteValues)
            {
                var bindingTableRule = shardingRule.FindBindingTableRule(LogicTableName);
                if ((LogicTableName.Equals(routeValue.GetTableName()) || bindingTableRule!=null && bindingTableRule.HasLogicTable(LogicTableName))
                    && shardingColumns.Contains(routeValue.GetColumnName()))
                {
                    result.Add(routeValue);
                }
            }
            return result;
        }

        private ICollection<DataNode> Route0(ShardingRule shardingRule, TableRule tableRule, List<IRouteValue> databaseShardingValues, List<IRouteValue> tableShardingValues)
        {
            ICollection<String> routedDataSources = RouteDataSources(shardingRule, tableRule, databaseShardingValues);
            ICollection<DataNode> result = new LinkedList<DataNode>();
            foreach (var routedDataSource in routedDataSources)
            {
                result.AddAll(RouteTables(shardingRule, tableRule, routedDataSource, tableShardingValues));
            }
            return result;
        }

        private ICollection<string> RouteDataSources(ShardingRule shardingRule, TableRule tableRule, List<IRouteValue> databaseShardingValues)
        {
            if (databaseShardingValues.IsEmpty())
            {
                return tableRule.GetActualDatasourceNames();
            }
            ICollection<string> result = new HashSet<string>(shardingRule.GetDatabaseShardingStrategy(tableRule).DoSharding(tableRule.GetActualDatasourceNames(), databaseShardingValues, this.Properties));
            ShardingAssert.If(result.IsEmpty(), "no database route info");
            ShardingAssert.Else(tableRule.GetActualDatasourceNames().All(o=>result.Contains(o)), $"Some routed data sources do not belong to configured data sources. routed data sources: `{result}`, configured data sources: `{tableRule.GetActualDatasourceNames()}`");
           
            return result;
        }

        private ICollection<DataNode> RouteTables(ShardingRule shardingRule, TableRule tableRule, string routedDataSource, List<IRouteValue> tableShardingValues)
        {
            ICollection<string> availableTargetTables = tableRule.GetActualTableNames(routedDataSource);
            ICollection<string> routedTables = new HashSet<string>(tableShardingValues.IsEmpty() ? availableTargetTables
                    : shardingRule.GetTableShardingStrategy(tableRule).DoSharding(availableTargetTables, tableShardingValues, this.Properties));
            ShardingAssert.If(routedTables.IsEmpty(), "no table route info");
            ICollection<DataNode> result = new LinkedList<DataNode>();
            foreach (var routedTable in routedTables)
            {
                result.Add(new DataNode(routedDataSource, routedTable));
            }
            return result;
        }
    }
}