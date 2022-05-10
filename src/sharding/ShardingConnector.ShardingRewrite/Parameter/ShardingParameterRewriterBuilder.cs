using System;
using System.Collections.Generic;
using System.Text;

using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.RewriteEngine.Parameter.Rewrite;
using ShardingConnector.RewriteEngine.Sql.Token.Generator.Aware;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingCommon.Core.Rule.Aware;
using ShardingConnector.ShardingRewrite.Parameter.Impl;

namespace ShardingConnector.ShardingRewrite.Parameter
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 15:02:57
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingParameterRewriterBuilder:IParameterRewriterBuilder
    {
        private readonly ShardingRule _shardingRule;

        private readonly RouteContext _routeContext;

        public ShardingParameterRewriterBuilder(ShardingRule shardingRule, RouteContext routeContext)
        {
            this._shardingRule = shardingRule;
            this._routeContext = routeContext;
        }

        public ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> GetParameterRewriters(SchemaMetaData schemaMetaData)
        {
            ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> result = GetParameterRewriters();
            foreach (var parameterRewriter in result)
            {
                SetUpParameterRewriters(parameterRewriter, schemaMetaData);
            }
            return result;
        }

        private static ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> GetParameterRewriters()
        {
            ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> result = new LinkedList<IParameterRewriter<ISqlCommandContext<ISqlCommand>>>();
            result.Add(new ShardingGeneratedKeyInsertValueParameterRewriter());
            result.Add(new ShardingPaginationParameterRewriter());
            return result;
        }

        private void SetUpParameterRewriters(IParameterRewriter<ISqlCommandContext<ISqlCommand>> parameterRewriter, SchemaMetaData schemaMetaData)
        {
            if (parameterRewriter is ISchemaMetaDataAware schemaMetaDataAware) {
                schemaMetaDataAware.SetSchemaMetaData(schemaMetaData);
            }
            if (parameterRewriter is IShardingRuleAware shardingRuleAware) {
                shardingRuleAware.SetShardingRule(_shardingRule);
            }
            if (parameterRewriter is IRouteContextAware routeContextAware) {
                routeContextAware.SetRouteContext(_routeContext);
            }
        }
    }
}
