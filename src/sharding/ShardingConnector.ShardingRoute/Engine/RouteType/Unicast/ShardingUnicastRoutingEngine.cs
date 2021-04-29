using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using ShardingConnector.Common.Rule;
using ShardingConnector.Exceptions;
using ShardingConnector.Extensions;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingRoute.Engine.RouteType.Unicast
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/29 12:44:06
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingUnicastRoutingEngine:IShardingRouteEngine
    {
        private readonly ICollection<string> logicTables;

        public ShardingUnicastRoutingEngine(ICollection<string> logicTables)
        {
            this.logicTables = logicTables;
        }

        public RouteResult Route(ShardingRule shardingRule)
        {
            RouteResult result = new RouteResult();
            string dataSourceName = shardingRule.ShardingDataSourceNames.GetRandomDataSourceName();
            RouteMapper dataSourceMapper = new RouteMapper(dataSourceName, dataSourceName);
            if (shardingRule.IsAllBroadcastTables(logicTables))
            {
                List<RouteMapper> tableMappers = new List<RouteMapper>(logicTables.Count);
                foreach (var logicTable in logicTables)
                {
                    tableMappers.Add(new RouteMapper(logicTable, logicTable));
                }
                result.GetRouteUnits().Add(new RouteUnit(dataSourceMapper, tableMappers));
            }
            else if (logicTables.IsEmpty())
            {
                result.GetRouteUnits().Add(new RouteUnit(dataSourceMapper, new List<RouteMapper>(0)));
            }
            else if (1 == logicTables.Count)
            {
                string logicTableName = logicTables.First();
                if (shardingRule.FindTableRule(logicTableName)==null)
                {
                    result.GetRouteUnits().Add(new RouteUnit(dataSourceMapper, new List<RouteMapper>(0)));
                    return result;
                }
                DataNode dataNode = shardingRule.GetDataNode(logicTableName);
                result.GetRouteUnits().Add(new RouteUnit(dataSourceMapper, new List<RouteMapper>(){ new RouteMapper(logicTableName, dataNode.GetTableName()) }));
            }
            else
            {
                List<RouteMapper> tableMappers = new List<RouteMapper>(logicTables.Count);
                ISet<string> availableDatasourceNames = null;
                bool first = true;
                foreach (var logicTable in logicTables)
                {
                    TableRule tableRule = shardingRule.GetTableRule(logicTable);
                    DataNode dataNode = tableRule.ActualDataNodes[0];
                    tableMappers.Add(new RouteMapper(logicTable, dataNode.GetTableName()));
                    ISet<string> currentDataSourceNames = new HashSet<string>();
                    foreach (var actualDataNode in tableRule.ActualDataNodes)
                    {
                        currentDataSourceNames.Add(actualDataNode.GetDataSourceName());
                    }
                    if (first)
                    {
                        availableDatasourceNames = currentDataSourceNames;
                        first = false;
                    }
                    else
                    {
                        availableDatasourceNames = availableDatasourceNames.Intersect(currentDataSourceNames).ToHashSet();
                    }
                }
                if (availableDatasourceNames.IsEmpty())
                {
                    throw new ShardingException($"Cannot find actual dataSource intersection for logic tables: {logicTables}");
                }
                dataSourceName = shardingRule.ShardingDataSourceNames.GetRandomDataSourceName(availableDatasourceNames);
                result.GetRouteUnits().Add(new RouteUnit(new RouteMapper(dataSourceName, dataSourceName), tableMappers));
            }
            return result;
        }
    }
}
