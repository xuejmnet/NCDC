using System;
using System.Collections.Generic;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.Rule;
using ShardingConnector.Kernels.Parse;
using ShardingConnector.Kernels.Route;

namespace ShardingConnector.Pluggble.Prepare
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 13 April 2021 21:36:17
* @Email: 326308290@qq.com
*/
    public sealed class PreparedQueryPrepareEngine:BasePrepareEngine
    {
        public PreparedQueryPrepareEngine(ICollection<IBaseRule> rules, ConfigurationProperties properties, ShardingConnectorMetaData metaData, SqlParserEngine sqlParserEngine) : base(rules, properties, metaData, sqlParserEngine)
        {
        }

        protected override List<object> CloneParameters(List<object> parameters)
        {
            return new List<object>(parameters);
        }

        protected override RouteContext Route(DataNodeRouter router, string sql, List<object> parameters)
        {
            return router.Route(sql, parameters);
        }
    }
}