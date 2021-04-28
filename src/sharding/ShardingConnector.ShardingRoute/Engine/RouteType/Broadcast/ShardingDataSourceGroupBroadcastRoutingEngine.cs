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
            ICollection<ISet<string>> broadcastDataSourceGroup = getBroadcastDataSourceGroup(getDataSourceGroup(shardingRule));
            for (Set<String> each : broadcastDataSourceGroup)
            {
                String dataSourceName = getRandomDataSourceName(each);
                result.getRouteUnits().add(new RouteUnit(new RouteMapper(dataSourceName, dataSourceName), Collections.emptyList()));
            }
            return result;
        }

        private ICollection<ISet<String>> getBroadcastDataSourceGroup(final ICollection<Set<String>> dataSourceGroup)
        {
            ICollection<ISet<String>> result = new LinkedList<>();
            for (Set<String> each : dataSourceGroup)
            {
                result = getCandidateDataSourceGroup(result, each);
            }
            return result;
        }

        private ICollection<ISet<String>> getDataSourceGroup(final ShardingRule shardingRule)
        {
            ICollection<ISet<String>> result = new LinkedList<>();
            for (TableRule each : shardingRule.getTableRules())
            {
                result.add(each.getDataNodeGroups().keySet());
            }
            if (null != shardingRule.getShardingDataSourceNames().getDefaultDataSourceName())
            {
                result.add(Sets.newHashSet(shardingRule.getShardingDataSourceNames().getDefaultDataSourceName()));
            }
            return result;
        }

        private ICollection<ISet<String>> GetCandidateDataSourceGroup(ICollection<ISet<String>> dataSourceSetGroup, ISet<String> compareSet)
        {
            ICollection<ISet<String>> result = new LinkedList<ISet<String>>();
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
