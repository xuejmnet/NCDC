using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.RewriteEngine.Sql.Token.Generator;
using ShardingConnector.RewriteEngine.Sql.Token.Generator.Aware;
using ShardingConnector.RewriteEngine.Sql.Token.Generator.Builder;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingCommon.Core.Rule.Aware;
using ShardingConnector.ShardingRewrite.Token.Generator;
using ShardingConnector.ShardingRewrite.Token.Generator.Impl;
using ShardingConnector.ShardingRewrite.Token.Generator.Impl.KeyGen;

namespace ShardingConnector.ShardingRewrite.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 8:51:42
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingTokenGenerateBuilder : ISqlTokenGeneratorBuilder
    {
        private readonly ShardingRule _shardingRule;

        private readonly RouteContext _routeContext;

        public ShardingTokenGenerateBuilder(ShardingRule shardingRule, RouteContext routeContext)
        {
            this._shardingRule = shardingRule;
            this._routeContext = routeContext;
        }

        public ICollection<ISqlTokenGenerator> GetSqlTokenGenerators()
        {
            ICollection<ISqlTokenGenerator> result = BuildSqlTokenGenerators();
            foreach (var sqlTokenGenerator in result)
            {
                if (sqlTokenGenerator is IShardingRuleAware shardingRuleAware)
                {
                    shardingRuleAware.SetShardingRule(_shardingRule);
                }
                if (sqlTokenGenerator is IRouteContextAware routeContextAware)
                {
                    routeContextAware.SetRouteContext(_routeContext);
                }
            }
            return result;
        }

        private ICollection<ISqlTokenGenerator> BuildSqlTokenGenerators()
        {
            ICollection<ISqlTokenGenerator> result = new LinkedList<ISqlTokenGenerator>();
            AddSqlTokenGenerator(result, new TableTokenGenerator());
            AddSqlTokenGenerator(result, new DistinctProjectionPrefixTokenGenerator());
            AddSqlTokenGenerator(result, new ProjectionsTokenGenerator());
            AddSqlTokenGenerator(result, new OrderByTokenGenerator());
            AddSqlTokenGenerator(result, new AggregationDistinctTokenGenerator());
            AddSqlTokenGenerator(result, new IndexTokenGenerator());
            AddSqlTokenGenerator(result, new OffsetTokenGenerator());
            AddSqlTokenGenerator(result, new RowCountTokenGenerator());
            AddSqlTokenGenerator(result, new GeneratedKeyInsertColumnTokenGenerator());
            AddSqlTokenGenerator(result, new GeneratedKeyForUseDefaultInsertColumnsTokenGenerator());
            AddSqlTokenGenerator(result, new GeneratedKeyAssignmentTokenGenerator());
            AddSqlTokenGenerator(result, new ShardingInsertValuesTokenGenerator());
            AddSqlTokenGenerator(result, new GeneratedKeyInsertValuesTokenGenerator());
            return result;
        }

        private void AddSqlTokenGenerator(ICollection<ISqlTokenGenerator> sqlTokenGenerators, ISqlTokenGenerator toBeAddedSqlTokenGenerator)
        {
            if (toBeAddedSqlTokenGenerator is IIgnoreForSingleRoute ignoreForSingleRoute && _routeContext.GetRouteResult().IsSingleRouting()) {
                return;
            }
            sqlTokenGenerators.Add(toBeAddedSqlTokenGenerator);
        }
    }
}
