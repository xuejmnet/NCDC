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
    * @Date: 2021/4/6 14:38:14
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    
    /**
 * Prepare engine for simple query.
 * 
 * <pre>
 *     Simple query:  
 *       for JDBC is Statement; 
 *       for MyQL is COM_QUERY; 
 *       for PostgreSQL is Simple Query;
 * </pre>
 */
    public sealed class SimpleQueryPrepareEngine: BasePrepareEngine
    {

        public SimpleQueryPrepareEngine(ICollection<IBaseRule> rules, ConfigurationProperties properties, ShardingConnectorMetaData metaData, SqlParserEngine sqlParserEngine) : base(rules, properties, metaData, sqlParserEngine)
        {
        }

        protected override List<object> CloneParameters(List<object> parameters)
        {
            return new List<object>();
        }

        protected override RouteContext Route(DataNodeRouter router, string sql, List<object> parameters)
        {
            return  router.Route(sql, new List<object>());
        }
    }
}
