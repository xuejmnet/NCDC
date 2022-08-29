using System;
using System.Collections.Generic;
using OpenConnector.Common.MetaData.DataSource;
using OpenConnector.Route.Context;
using OpenConnector.ShardingCommon.Core.Rule;

namespace OpenConnector.ShardingRoute.Engine.RouteType.Broadcast
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 28 April 2021 21:31:11
* @Email: 326308290@qq.com
*/
    public sealed class ShardingMasterInstanceBroadcastRoutingEngine:IShardingRouteEngine
    {
        private readonly DataSourceMetas _dataSourceMetas;

        public ShardingMasterInstanceBroadcastRoutingEngine(DataSourceMetas dataSourceMetas)
        {
            this._dataSourceMetas = dataSourceMetas;
        }

        public RouteResult Route(ShardingRule shardingRule)
        {
            RouteResult result = new RouteResult();
            foreach (var dataSourceName in shardingRule.ShardingDataSourceNames.DataSourceNames)
            {
                if (_dataSourceMetas.GetAllInstanceDataSourceNames().Contains(dataSourceName)) {
                    MasterSlaveRule masterSlaveRule = shardingRule.FindMasterSlaveRule(dataSourceName);
                    if (masterSlaveRule==null || masterSlaveRule.MasterDataSourceName.Equals(dataSourceName)) {
                        result.GetRouteUnits().Add(new RouteUnit(new RouteMapper(dataSourceName, dataSourceName), new List<RouteMapper>(0)));
                    }
                }
            }
            return result;
        }
    }
}