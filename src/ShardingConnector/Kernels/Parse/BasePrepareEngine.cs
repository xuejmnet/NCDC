using System;
using System.Collections.Generic;
using ShardingConnector.Kernels.Parse.SqlExpression;
using ShardingConnector.Kernels.Route;

namespace ShardingConnector.Kernels.Parse
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Tuesday, 23 March 2021 21:14:46
    * @Email: 326308290@qq.com
    */
    public abstract class BasePrepareEngine
    {

        protected abstract IList<object> CloneParameters(IList<object> parameters);
        protected abstract RouteContext Route(string sql, IList<object> parameters);

        public ExecutionContext<ISqlCommand> Prepare(string sql, IList<object> parameters)
        {
            var cloneParameters = CloneParameters(parameters);
        }

        private RouteContext ExecuteRoute(string sql, List<object> clonedParameters)
        {
            return Route(sq)
        }
    }
}