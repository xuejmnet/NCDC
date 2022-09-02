﻿using OpenConnector.CommandParser.SqlParseEngines;
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

        public SimpleQueryPrepareEngine(ICollection<IBaseRule> rules, ConfigurationProperties properties, OpenConnectorMetaData metaData, SqlCommandParser sqlCommandParser) : base(rules, properties, metaData, sqlCommandParser)
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