using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Kernels.Parse;
using ShardingConnector.Kernels.Parse.SqlExpression;
using ShardingConnector.Kernels.Route.Rule;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;

namespace ShardingConnector.Kernels.Route
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/3/30 13:00:50
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DataNodeRouter
    {
        private readonly IDictionary<IBaseRule, IRouteDecorator<IBaseRule>> _decorators =
            new Dictionary<IBaseRule, IRouteDecorator<IBaseRule>>();

        private readonly SqlParserEngine _parserEngine;

        public DataNodeRouter(SqlParserEngine parserEngine)
        {
            _parserEngine = parserEngine;
        }
        public void RegisterDecorator(IBaseRule rule,IRouteDecorator<IBaseRule> decorator)
        {
            _decorators.Add(rule,decorator);
        }

        public RouteContext Route(string sql,List<object> parameters)
        {

        }

        private RouteContext ExecuteRoute(string sql, List<object> parameters)
        {

        }

        private RouteContext CreateRouteContext(string sql, List<object> parameters)
        {
            var sqlCommand = _parserEngine.Parse(sql);
            try
            {
                ISqlCommandContext<ISqlCommand>
            }
        }
    }
}
