using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.Rule;
using ShardingConnector.ParserEngine;
using ShardingConnector.Route;
using ShardingConnector.Route.Context;

namespace ShardingConnector.Pluggable.Prepare
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

        protected override IDictionary<string, DbParameter> CloneParameters(IDictionary<string, DbParameter> parameters)
        {
            return new Dictionary<string, DbParameter>(parameters);
        }

        protected override RouteContext Route(DataNodeRouter router, string sql, IDictionary<string, DbParameter> parameters)
        {
            return router.Route(sql, parameters,true);
        }
    }
}