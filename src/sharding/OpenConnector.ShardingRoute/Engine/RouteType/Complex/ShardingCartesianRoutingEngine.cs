using System;
using System.Collections.Generic;
using System.Linq;
using OpenConnector.Extensions;
using OpenConnector.Route.Context;
using OpenConnector.ShardingCommon.Core.Rule;

namespace OpenConnector.ShardingRoute.Engine.RouteType.Complex
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 28 April 2021 21:40:52
* @Email: 326308290@qq.com
*/
    public sealed class ShardingCartesianRoutingEngine : IShardingRouteEngine
    {
        private readonly ICollection<RouteResult> routeResults;

        public ShardingCartesianRoutingEngine(ICollection<RouteResult> routeResults)
        {
            this.routeResults = routeResults;
        }

        public RouteResult Route(ShardingRule shardingRule)
        {
            RouteResult result = new RouteResult();
            var dataSourceLogicTablesMap = GetDataSourceLogicTablesMap();
            foreach (var dataSourceLogicTableMap in dataSourceLogicTablesMap)
            {
                List<ISet<String>> actualTableGroups = GetActualTableGroups(dataSourceLogicTableMap.Key, dataSourceLogicTableMap.Value);
                List<ISet<RouteMapper>> routingTableGroups = ToRoutingTableGroups(dataSourceLogicTableMap.Key, actualTableGroups);
                result.GetRouteUnits().AddAll(GetRouteUnits(dataSourceLogicTableMap.Key, routingTableGroups.Cartesian().Select(o => o.ToList()).ToHashSet()));
            }

            return result;
        }

        private IDictionary<String, ISet<String>> GetDataSourceLogicTablesMap()
        {
            ICollection<String> intersectionDataSources = GetIntersectionDataSources();
            IDictionary<String, ISet<String>> result = new Dictionary<string, ISet<string>>(routeResults.Count);
            foreach (var routeResult in routeResults)
            {
                var dataSourceLogicTablesMap = routeResult.GetDataSourceLogicTablesMap(intersectionDataSources);
                foreach (var dataSourceLogicTableMap in dataSourceLogicTablesMap)
                {
                    if (result.ContainsKey(dataSourceLogicTableMap.Key))
                    {
                        result[dataSourceLogicTableMap.Key].AddAll(dataSourceLogicTableMap.Value);
                    }
                    else
                    {
                        result.Add(dataSourceLogicTableMap.Key, dataSourceLogicTableMap.Value);
                    }
                }
            }

            return result;
        }

        private ICollection<String> GetIntersectionDataSources()
        {
            ISet<String> result = new HashSet<String>();
            foreach (var routeResult in routeResults)
            {
                if (result.IsEmpty())
                {
                    result.AddAll(routeResult.GetActualDataSourceNames());
                }

                result.IntersectWith(routeResult.GetActualDataSourceNames());
            }

            return result;
        }

        private List<ISet<String>> GetActualTableGroups(String dataSourceName, ISet<String> logicTables)
        {
            List<ISet<String>> result = new List<ISet<string>>(logicTables.Count);
            foreach (var routeResult in routeResults)
            {
                result.AddAll(routeResult.GetActualTableNameGroups(dataSourceName, logicTables));
            }

            return result;
        }

        private List<ISet<RouteMapper>> ToRoutingTableGroups(String dataSource, List<ISet<String>> actualTableGroups)
        {
            List<ISet<RouteMapper>> result = new List<ISet<RouteMapper>>(actualTableGroups.Count);
            foreach (var actualTableGroup in actualTableGroups)
            {
                result.Add(new HashSet<RouteMapper>(actualTableGroup.Select(input => FindRoutingTable(dataSource, input))));
            }

            return result;
        }

        private RouteMapper FindRoutingTable(String dataSource, String actualTable)
        {
            foreach (var routeResult in routeResults)
            {
                RouteMapper result = routeResult.FindTableMapper(dataSource, actualTable);
                if (null != result)
                {
                    return result;
                }
            }

            throw new InvalidOperationException($"Cannot found routing table factor, data source: {dataSource}, actual table: {actualTable}");
        }

        private ICollection<RouteUnit> GetRouteUnits(String dataSource, ISet<List<RouteMapper>> cartesianRoutingTableGroups)
        {
            ICollection<RouteUnit> result = new HashSet<RouteUnit>();
            foreach (var cartesianRoutingTableGroup in cartesianRoutingTableGroups)
            {
                result.Add(new RouteUnit(new RouteMapper(dataSource, dataSource), cartesianRoutingTableGroup));
            }

            return result;
        }
    }
}