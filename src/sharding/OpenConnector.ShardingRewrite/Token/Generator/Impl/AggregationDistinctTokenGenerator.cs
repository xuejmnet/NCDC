using System;
using System.Collections.Generic;
using OpenConnector.Base;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.CommandParserBinder.Command;
using NCDC.CommandParserBinder.Command.DML;
using NCDC.CommandParserBinder.Segment.Select.Projection;
using NCDC.CommandParserBinder.Segment.Select.Projection.Impl;
using OpenConnector.RewriteEngine.Sql.Token.Generator;
using OpenConnector.RewriteEngine.Sql.Token.SimpleObject;
using OpenConnector.ShardingRewrite.Token.SimpleObject;

namespace OpenConnector.ShardingRewrite.Token.Generator.Impl
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