using System.Collections.Generic;
using ShardingConnector.Common.Rule;
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
    public class SimpleQueryPrepareEngine: BasePrepareEngine
    {
        public SimpleQueryPrepareEngine(DataNodeRouter router, ICollection<IBaseRule> rules) : base(router, rules)
        {
        }

        protected override IList<object> CloneParameters(IList<object> parameters)
        {
            return new List<object>();
        }

        protected override RouteContext Route(DataNodeRouter router, string sql, IList<object> parameters)
        {
            return router.
        }
    }
}
