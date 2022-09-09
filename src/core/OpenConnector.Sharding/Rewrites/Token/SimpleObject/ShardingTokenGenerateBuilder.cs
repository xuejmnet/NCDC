using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Sharding.Rewrites.Sql.Token.Generator;
using OpenConnector.Sharding.Rewrites.Sql.Token.Generator.Builder;
using OpenConnector.Sharding.Rewrites.Token.Generator;
using OpenConnector.Sharding.Rewrites.Token.Generator.Impl;
using OpenConnector.Sharding.Rewrites.Token.Generator.Impl.KeyGen;
using OpenConnector.Sharding.Routes;

namespace OpenConnector.Sharding.Rewrites.Token.SimpleObject
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
        private readonly ITableMetadataManager _tableMetadataManager;

        private readonly RouteContext _routeContext;

        public ShardingTokenGenerateBuilder(ITableMetadataManager tableMetadataManager)
        {
            _tableMetadataManager = tableMetadataManager;
        }

        public ICollection<ISqlTokenGenerator> GetSqlTokenGenerators(RouteContext routeContext)
        {
            ICollection<ISqlTokenGenerator> result = BuildSqlTokenGenerators();
            // foreach (var sqlTokenGenerator in result)
            // {
            //     if (sqlTokenGenerator is IShardingRuleAware shardingRuleAware)
            //     {
            //         shardingRuleAware.SetShardingRule(_shardingRule);
            //     }
            //     if (sqlTokenGenerator is IRouteContextAware routeContextAware)
            //     {
            //         routeContextAware.SetRouteContext(_routeContext);
            //     }
            // }
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
            
            if (toBeAddedSqlTokenGenerator is IShardingRuleAware shardingRuleAware)
            {
                shardingRuleAware.SetShardingRule(_shardingRule);
            }
            if (toBeAddedSqlTokenGenerator is IRouteContextAware routeContextAware)
            {
                routeContextAware.SetRouteContext(_routeContext);
            }
            sqlTokenGenerators.Add(toBeAddedSqlTokenGenerator);
        }
    }
}
