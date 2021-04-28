using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.Rule;
using ShardingConnector.Extensions;
using ShardingConnector.RewriteEngine.Context;
using ShardingConnector.RewriteEngine.Sql.Token.Generator.Aware;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingRewrite.Parameter;
using ShardingConnector.ShardingRewrite.Token.SimpleObject;

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
            var parameterRewriters = new ShardingParameterRewriterBuilder(rule, routeContext).GetParameterRewriters(sqlRewriteContext.GetSchemaMetaData());
            foreach (var parameterRewriter in parameterRewriters)
            {
                if (!sqlRewriteContext.GetParameters().IsEmpty() && parameterRewriter.IsNeedRewrite(sqlRewriteContext.GetSqlCommandContext()))
                {
                    parameterRewriter.Rewrite(sqlRewriteContext.GetParameterBuilder(), sqlRewriteContext.GetSqlCommandContext(), sqlRewriteContext.GetParameters());
                }
            }
            sqlRewriteContext.AddSqlTokenGenerators(new ShardingTokenGenerateBuilder(rule, routeContext).GetSqlTokenGenerators());
        }

        public void Decorate(IBaseRule rule, ConfigurationProperties properties, SqlRewriteContext sqlRewriteContext)
        {
            Decorate((ShardingRule) rule, properties,sqlRewriteContext);
        }

        public int GetOrder()
        {
            return 0;
        }

        public Type GetGenericType()
        {
            return typeof(ShardingRule);
        }

        public void SetRouteContext(RouteContext routeContext)
        {
            this.routeContext = routeContext;
        }
    }
}
