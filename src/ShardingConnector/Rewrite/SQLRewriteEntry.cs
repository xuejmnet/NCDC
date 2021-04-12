using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Kernels.MetaData.Schema;
using ShardingConnector.Kernels.Route.Rule;

namespace ShardingConnector.Rewrite
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 15:02:38
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SQLRewriteEntry
    {
        private readonly SchemaMetaData _schemaMetaData;
    
        private readonly ConfigurationProperties _properties;
    
        private readonly IDictionary<IBaseRule, SQLRewriteContextDecorator> decorators = new LinkedHashMap<>();

        /**
         * Register route decorator.
         *
         * @param rule rule
         * @param decorator SQL rewrite context decorator
         */
        public void registerDecorator(final BaseRule rule, final SQLRewriteContextDecorator decorator)
        {
            decorators.put(rule, decorator);
        }

        /**
         * Create SQL rewrite context.
         * 
         * @param sql SQL
         * @param parameters parameters
         * @param sqlStatementContext SQL statement context
         * @param routeContext route context
         * @return SQL rewrite context
         */
        public SQLRewriteContext createSQLRewriteContext(final String sql, final List<Object> parameters, final SQLStatementContext sqlStatementContext, final RouteContext routeContext)
        {
            SQLRewriteContext result = new SQLRewriteContext(schemaMetaData, sqlStatementContext, sql, parameters);
            decorate(decorators, result, routeContext);
            result.generateSQLTokens();
            return result;
        }

        @SuppressWarnings("unchecked")
        private void decorate(final Map<BaseRule, SQLRewriteContextDecorator> decorators, final SQLRewriteContext sqlRewriteContext, final RouteContext routeContext)
        {
            for (Entry<BaseRule, SQLRewriteContextDecorator> entry : decorators.entrySet())
            {
                BaseRule rule = entry.getKey();
                SQLRewriteContextDecorator decorator = entry.getValue();
                if (decorator instanceof RouteContextAware) {
                    ((RouteContextAware)decorator).setRouteContext(routeContext);
                }
                decorator.decorate(rule, properties, sqlRewriteContext);
            }
        }
}
}
