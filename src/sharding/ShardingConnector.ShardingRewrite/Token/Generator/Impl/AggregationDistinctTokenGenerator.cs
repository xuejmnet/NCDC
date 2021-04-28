using System;
using System.Collections.Generic;
using ShardingConnector.Base;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ParserBinder.Segment.Select.Projection;
using ShardingConnector.ParserBinder.Segment.Select.Projection.Impl;
using ShardingConnector.RewriteEngine.Sql.Token.Generator;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;
using ShardingConnector.ShardingRewrite.Token.SimpleObject;

namespace ShardingConnector.ShardingRewrite.Token.Generator.Impl
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
            ShardingAssert.CantBeNull(projection.GetAlias(),"alias is required");
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