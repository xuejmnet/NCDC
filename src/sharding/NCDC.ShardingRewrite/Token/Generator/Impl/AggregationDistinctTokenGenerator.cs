using NCDC.Base;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingParser.Segment.Select.Projection;
using NCDC.ShardingParser.Segment.Select.Projection.Impl;
using NCDC.ShardingRewrite.Sql.Token.Generator;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;
using NCDC.ShardingRewrite.Token.SimpleObject;

namespace NCDC.ShardingRewrite.Token.Generator.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 21:29:55
* @Email: 326308290@qq.com
*/
    public sealed class AggregationDistinctTokenGenerator:ICollectionSqlTokenGenerator<SelectCommandContext>,IIgnoreForSingleRoute
    {
        public ICollection<SqlToken> GenerateSqlTokens(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return GenerateSqlTokens((SelectCommandContext) sqlCommandContext);
        }
    
        private AggregationDistinctToken GenerateSQLToken(AggregationDistinctProjection projection) {
            ShardingAssert.ShouldBeNotNull(projection.GetAlias(),"alias is required");
            String derivedAlias = DerivedColumn.IsDerivedColumnName(projection.GetAlias()) ? projection.GetAlias() : null;
            return new AggregationDistinctToken(projection.StartIndex, projection.StopIndex, projection.GetDistinctInnerExpression(), derivedAlias);
        }


        public ICollection<SqlToken> GenerateSqlTokens(SelectCommandContext sqlCommandContext)
        {
            ICollection<SqlToken> result = new LinkedList<SqlToken>();
            foreach (var aggregationProjection in sqlCommandContext.GetProjectionsContext().GetAggregationDistinctProjections())
            {
                result.Add(GenerateSQLToken(aggregationProjection));
            }
            return result;
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is SelectCommandContext;
        }
    }
}