using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.Base;
using ShardingConnector.Extensions;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingRoute.Engine.RouteType.Broadcast
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 15:40:02
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingDataSourceGroupBroadcastRoutingEngine:IShardingRouteEngine
    {
        public RouteResult Route(ShardingRule shardingRule)
        {
            RouteResult result = new RouteResult();
            ICollection<ISet<string>> broadcastDataSourceGroup = GetBroadcastDataSourceGroup(getDataSourceGroup(shardingRule));
            foreach (var broadcastDataSource in broadcastDataSourceGroup)
            {
                var dataSourceName = GetRandomDataSourceName(broadcastDataSource);
                result.GetRouteUnits().Add(new RouteUnit(new RouteMapper(dataSourceName, dataSourceName), new List<RouteMapper>(0)));
            }
            return result;
        }

        private ICollection<ISet<string>> GetBroadcastDataSourceGroup(ICollection<ISet<string>> dataSourceGroup)
        {
            ICollection<ISet<string>> result = new LinkedList<ISet<string>>();
            foreach (var dataSource in dataSourceGroup)
            {
                result=GetCandidateDataSourceGroup(result, dataSource);
            }
            return result;
        }

        private ICollection<ISet<string>> getDataSourceGroup(ShardingRule shardingRule)
        {
            ICollection<ISet<string>> result = new LinkedList<ISet<string>>();
            foreach (var tableRule in shardingRule.TableRules)
            {
                result.Add(tableRule.GetDataNodeGroups().Keys.ToHashSet());
            }
            if (null != shardingRule.ShardingDataSourceNames.GetDefaultDataSourceName())
            {
                result.Add(new HashSet<string>(){shardingRule.ShardingDataSourceNames.GetDefaultDataSourceName()});
            }
            return result;
        }

        private ICollection<ISet<string>> GetCandidateDataSourceGroup(ICollection<ISet<string>> dataSourceSetGroup, ISet<string> compareSet)
        {
            ICollection<ISet<string>> result = new LinkedList<ISet<string>>();
            if (dataSourceSetGroup.IsEmpty())
            {
                result.Add(compareSet);
                return result;
            }
            bool hasIntersection = false;
            foreach (var dataSourceSet in dataSourceSetGroup)
            {
                ISet<String> intersectionSet = dataSourceSet.Intersect(compareSet).ToHashSet();
                if (intersectionSet.Any())
                {
                    result.Add(intersectionSet);
                    hasIntersection = true;
                }
                else
                {
                    result.Add(intersectionSet);
                }
            }
            if (!hasIntersection)
            {
                result.Add(compareSet);
            }
            return result;
        }

        private string GetRandomDataSourceName(ICollection<string> dataSourceNames)
        {
            return dataSourceNames.ElementAt(ThreadLocalRandom.Next(dataSourceNames.Count));
        }
    }
}
