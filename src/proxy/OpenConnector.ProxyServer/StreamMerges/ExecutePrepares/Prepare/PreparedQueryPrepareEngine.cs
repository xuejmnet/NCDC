using OpenConnector.CommandParser.SqlParseEngines;
using OpenConnector.Common.Config.Properties;
using OpenConnector.Common.MetaData;
using OpenConnector.Common.Rule;
using OpenConnector.Route;
using OpenConnector.Route.Context;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.ProxyServer.StreamMerges.ExecutePrepares.Prepare
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 13 April 2021 21:36:17
* @Email: 326308290@qq.com
*/
    public sealed class PreparedQueryPrepareEngine:ProxyServer.StreamMerges.ExecutePrepares.Prepare.BasePrepareEngine
    {
        public PreparedQueryPrepareEngine(ICollection<IBaseRule> rules, ConfigurationProperties properties, OpenConnectorMetaData metaData, SqlParserEngine sqlParserEngine) : base(rules, properties, metaData, sqlParserEngine)
        {
        }

        protected override ParameterContext CloneParameters(ParameterContext parameterContext)
        {
            return parameterContext.CloneParameterContext();
        }

        protected override RouteContext Route(DataNodeRouter router, string sql, ParameterContext parameterContext)
        {
            return router.Route(sql, parameterContext,true);
        }
    }
}