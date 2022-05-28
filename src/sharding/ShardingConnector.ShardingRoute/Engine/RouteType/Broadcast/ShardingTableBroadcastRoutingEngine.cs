using System;
using System.Collections.Generic;
using System.Linq;

using ShardingConnector.Base;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DDL;
using ShardingConnector.CommandParser.Segment.DDL.Index;
using ShardingConnector.Common.Rule;
using ShardingConnector.Extensions;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingRoute.Engine.RouteType.Broadcast
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 28 April 2021 21:33:36
* @Email: 326308290@qq.com
*/
    public sealed class ShardingTableBroadcastRoutingEngine : IShardingRouteEngine
    {
        private readonly SchemaMetaData schemaMetaData;

        private readonly ISqlCommandContext<ISqlCommand> sqlCommandContext;

        public ShardingTableBroadcastRoutingEngine(SchemaMetaData schemaMetaData, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            this.schemaMetaData = schemaMetaData;
            this.sqlCommandContext = sqlCommandContext;
        }

        public RouteResult Route(ShardingRule shardingRule)
        {
            RouteResult result = new RouteResult();
            var logicTableNames = GetLogicTableNames();
            foreach (var logicTableName in logicTableNames)
            {
                result.GetRouteUnits().AddAll(GetAllRouteUnits(shardingRule, logicTableName));
            }

            return result;
        }

        private ICollection<string> GetLogicTableNames()
        {
            if (sqlCommandContext.GetSqlCommand() is DropIndexCommand dropIndexCommand && dropIndexCommand.Indexes.Any())
            {
                return GetTableNamesFromMetaData(dropIndexCommand);
            }

            return sqlCommandContext.GetTablesContext().GetTableNames();
        }

        private ICollection<string> GetTableNamesFromMetaData(DropIndexCommand dropIndexCommand)
        {
            ICollection<string> result = new LinkedList<string>();
            foreach (var index in dropIndexCommand.Indexes)
            {
                var tableName = FindLogicTableNameFromMetaData(index.Identifier.GetValue());
                ShardingAssert.ShouldBeNotNull(tableName, $"Cannot find index name `{index.Identifier.GetValue()}`.");
                result.Add(tableName);
            }

            return result;
        }

        private string FindLogicTableNameFromMetaData(string logicIndexName)
        {
            foreach (var tableName in schemaMetaData.GetAllTableNames())
            {
                if (schemaMetaData.Get(tableName).GetIndexes().ContainsKey(logicIndexName))
                {
                    return tableName;
                }
            }

            return null;
        }

        private ICollection<RouteUnit> GetAllRouteUnits(ShardingRule shardingRule, String logicTableName)
        {
            ICollection<RouteUnit> result = new LinkedList<RouteUnit>();
            TableRule tableRule = shardingRule.GetTableRule(logicTableName);
            foreach (var dataNode in tableRule.ActualDataNodes)
            {
                RouteUnit routeUnit = new RouteUnit(new RouteMapper(dataNode.GetDataSourceName(), dataNode.GetDataSourceName()), new List<RouteMapper>() {new RouteMapper(logicTableName, dataNode.GetTableName())});
                result.Add(routeUnit);
            }

            return result;
        }
    }
}