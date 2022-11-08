using NCDC.ShardingParser.MetaData;
using NCDC.Extensions;
using NCDC.ShardingRewrite.Sql.Token.Generator;
using NCDC.ShardingRewrite.Sql.Token.Generator.Builder;
using NCDC.ShardingRewrite.Sql.Token.Generator.Generic;
using NCDC.ShardingRewrite.Token.Generator.Impl;
using NCDC.ShardingRewrite.Token.Generator.Impl.KeyGen;
using NCDC.ShardingRoute;
using NCDC.Extensions;

namespace NCDC.ShardingRewrite.Token.SimpleObject
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

        public ShardingTokenGenerateBuilder(ITableMetadataManager tableMetadataManager)
        {
            _tableMetadataManager = tableMetadataManager;
        }

        public ICollection<ISqlTokenGenerator> GetSqlTokenGenerators(RouteContext routeContext)
        {
            ICollection<ISqlTokenGenerator> result = BuildSqlTokenGenerators(routeContext);
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

        private ICollection<ISqlTokenGenerator> BuildSqlTokenGenerators(RouteContext routeContext)
        {
            var isSingleRouting = routeContext.GetRouteResult().IsSingleRouting();
            ICollection<ISqlTokenGenerator> result = new LinkedList<ISqlTokenGenerator>();
            result.Add(new RemoveTokenGenerator());
            result.Add(new TableTokenGenerator(_tableMetadataManager));
            result.AddIf(!isSingleRouting,new DistinctProjectionPrefixTokenGenerator());
            result.AddIf(!isSingleRouting,new ProjectionsTokenGenerator());
            result.AddIf(!isSingleRouting,new OrderByTokenGenerator());
            result.AddIf(!isSingleRouting,new AggregationDistinctTokenGenerator());
            result.Add(new IndexTokenGenerator(_tableMetadataManager));
            result.AddIf(!isSingleRouting,new OffsetTokenGenerator());
            result.AddIf(!isSingleRouting,new RowCountTokenGenerator());
            result.Add(new GeneratedKeyInsertColumnTokenGenerator());
            result.Add(new GeneratedKeyForUseDefaultInsertColumnsTokenGenerator(_tableMetadataManager));
            result.Add(new GeneratedKeyAssignmentTokenGenerator(routeContext.GetParameterContext()));
            result.Add(new ShardingInsertValuesTokenGenerator(routeContext));
            // result.Add(new GeneratedKeyInsertValuesTokenGenerator());
            return result;
        }
    }
}
