using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.RewriteEngine.Context;
using ShardingConnector.RewriteEngine.Sql.Token.Generator.Aware;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingRewrite.Context
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 14:36:06
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingSqlRewriteContextDecorator:ISqlRewriteContextDecorator<ShardingRule>,IRouteContextAware
    {
        private RouteContext routeContext;
        public void Decorate(ShardingRule rule, ConfigurationProperties properties, SqlRewriteContext sqlRewriteContext)
        {
            foreach (var VARIABLE in COLLECTION)
            {
                
            }
            for (ParameterRewriter each : new ShardingParameterRewriterBuilder(shardingRule, routeContext).getParameterRewriters(sqlRewriteContext.getSchemaMetaData()))
            {
                if (!sqlRewriteContext.getParameters().isEmpty() && each.isNeedRewrite(sqlRewriteContext.getSqlStatementContext()))
                {
                    each.rewrite(sqlRewriteContext.getParameterBuilder(), sqlRewriteContext.getSqlStatementContext(), sqlRewriteContext.getParameters());
                }
            }
            sqlRewriteContext.addSQLTokenGenerators(new ShardingTokenGenerateBuilder(shardingRule, routeContext).getSQLTokenGenerators());

        }

        public int GetOrder()
        {
            return 0;
        }

        public void SetRouteContext(RouteContext routeContext)
        {
            this.routeContext = routeContext;
        }
    }
}
