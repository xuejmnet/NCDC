using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.RewriteEngine.Sql.Token.Generator;
using ShardingConnector.RewriteEngine.Sql.Token.Generator.Builder;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingRewrite.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 8:51:42
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingTokenGenerateBuilder: ISqlTokenGeneratorBuilder
    {
        private readonly ShardingRule shardingRule;
    
    private readonly RouteContext routeContext;
    
    public ICollection<ISqlTokenGenerator> GetSqlTokenGenerators()
        {
            ICollection<ISqlTokenGenerator> result = BuildSqlTokenGenerators();
            for (SQLTokenGenerator each : result)
            {
                if (each instanceof ShardingRuleAware) {
                ((ShardingRuleAware)each).setShardingRule(shardingRule);
            }
            if (each instanceof RouteContextAware) {
                ((RouteContextAware)each).setRouteContext(routeContext);
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
        if (toBeAddedSQLTokenGenerator instanceof IgnoreForSingleRoute && routeContext.getRouteResult().isSingleRouting()) {
            return;
        }
        sqlTokenGenerators.add(toBeAddedSQLTokenGenerator);
    }
}
}
