using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenConnector.Common.Config.Properties;
using OpenConnector.Common.Rule;
using OpenConnector.Extensions;
using OpenConnector.RewriteEngine.Context;
using OpenConnector.RewriteEngine.Sql.Token.Generator.Aware;
using OpenConnector.Route.Context;
using OpenConnector.ShardingCommon.Core.Rule;
using OpenConnector.ShardingRewrite.Parameter;
using OpenConnector.ShardingRewrite.Token.SimpleObject;

namespace OpenConnector.ShardingRewrite.Context
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
            //
            // var tableNames = sqlRewriteContext.GetSqlCommandContext().GetTablesContext().GetTableNames();
            // if (!rule.TableRules.Any(o => tableNames.Any(t => o.LogicTable.EqualsIgnoreCase(t))))
            // {
            //     return;
            // }
            var parameterRewriters = new ShardingParameterRewriterBuilder(rule, routeContext).GetParameterRewriters(sqlRewriteContext.GetSchemaMetaData());
            foreach (var parameterRewriter in parameterRewriters)
            {
                if (!sqlRewriteContext.GetParameterContext().IsEmpty() && parameterRewriter.IsNeedRewrite(sqlRewriteContext.GetSqlCommandContext()))
                {
                    parameterRewriter.Rewrite(sqlRewriteContext.GetParameterBuilder(), sqlRewriteContext.GetSqlCommandContext(), sqlRewriteContext.GetParameterContext());
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
