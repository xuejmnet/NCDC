using ShardingConnector.CommandParser.SqlParseEngines;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.Rule;
using ShardingConnector.Route;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingAdoNet;

namespace ShardingConnector.ProxyServer.StreamMerges.ExecutePrepares.Prepare
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/6 14:38:14
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    
    /**
 * Prepare engine for simple query.
 * 
 * <pre>
 *     Simple query:  
 *       for ADONET is Command; 
 *       for MyQL is COM_QUERY; 
 *       for PostgreSQL is Simple Query;
 * </pre>
 */
    public sealed class SimpleQueryPrepareEngine: ProxyServer.StreamMerges.ExecutePrepares.Prepare.BasePrepareEngine
    {

        public SimpleQueryPrepareEngine(ICollection<IBaseRule> rules, ConfigurationProperties properties, ShardingConnectorMetaData metaData, SqlParserEngine sqlParserEngine) : base(rules, properties, metaData, sqlParserEngine)
        {
        }

        protected override ParameterContext CloneParameters(ParameterContext parameterContext)
        {
            return parameterContext.CloneParameterContext();
        }

        protected override RouteContext Route(DataNodeRouter router, string sql, ParameterContext parameterContext)
        {
            return  router.Route(sql, parameterContext, false);
        }
    }
}
