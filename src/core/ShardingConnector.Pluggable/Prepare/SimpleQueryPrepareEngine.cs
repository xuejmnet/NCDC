using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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
    public sealed class SimpleQueryPrepareEngine: BasePrepareEngine
    {

        public SimpleQueryPrepareEngine(ICollection<IBaseRule> rules, ConfigurationProperties properties, ShardingConnectorMetaData metaData, SqlParserEngine sqlParserEngine) : base(rules, properties, metaData, sqlParserEngine)
        {
        }

        protected override IDictionary<string, DbParameter> CloneParameters(ParameterContext parameterContext)
        {
            return parameters.ToDictionary(o=>o.Key,o=>o.Value);
        }

        protected override RouteContext Route(DataNodeRouter router, string sql, ParameterContext parameterContext)
        {
            return  router.Route(sql, parameters,false);
        }
    }
}
