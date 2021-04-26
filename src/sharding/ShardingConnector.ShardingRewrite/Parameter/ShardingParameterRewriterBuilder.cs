using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.RewriteEngine.Parameter.Rewrite;

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
        public ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> GetParameterRewriters(SchemaMetaData schemaMetaData)
        {
            ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> result = GetParameterRewriters();
            for (ParameterRewriter each : result)
            {
                setUpParameterRewriters(each, schemaMetaData);
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

        private void setUpParameterRewriters(final ParameterRewriter parameterRewriter, final SchemaMetaData schemaMetaData)
        {
            if (parameterRewriter instanceof SchemaMetaDataAware) {
                ((SchemaMetaDataAware)parameterRewriter).setSchemaMetaData(schemaMetaData);
            }
            if (parameterRewriter instanceof ShardingRuleAware) {
                ((ShardingRuleAware)parameterRewriter).setShardingRule(shardingRule);
            }
            if (parameterRewriter instanceof RouteContextAware) {
                ((RouteContextAware)parameterRewriter).setRouteContext(routeContext);
            }
        }
    }
}
