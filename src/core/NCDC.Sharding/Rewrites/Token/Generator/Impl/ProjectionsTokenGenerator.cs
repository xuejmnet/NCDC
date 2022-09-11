﻿using OpenConnector.Base;
using NCDC.CommandParser.Abstractions;
using NCDC.Basic.Parser.Command;
using NCDC.Basic.Parser.Command.DML;
using NCDC.Basic.Parser.Segment.Select.Projection;
using NCDC.Basic.Parser.Segment.Select.Projection.Impl;
using OpenConnector.Extensions;
using NCDC.Sharding.Rewrites.Sql.Token.Generator;
using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;
using NCDC.Sharding.Rewrites.Token.SimpleObject;

namespace NCDC.Sharding.Rewrites.Token.Generator.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 8:17:52
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
   public sealed class ProjectionsTokenGenerator: IOptionalSqlTokenGenerator<SelectCommandContext>, IIgnoreForSingleRoute
    {
        public SqlToken GenerateSqlToken(SelectCommandContext sqlCommandContext)
        {
            ICollection<string> derivedProjectionTexts = GetDerivedProjectionTexts(sqlCommandContext);
            return new ProjectionsToken(sqlCommandContext.GetProjectionsContext().GetStopIndex() + 1 + " ".Length, derivedProjectionTexts);
        }

        public SqlToken GenerateSqlToken(ISqlCommandContext<ISqlCommand>  sqlCommandContext)
        {
            return GenerateSqlToken((SelectCommandContext)sqlCommandContext);

        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is SelectCommandContext selectCommandContext && GetDerivedProjectionTexts(selectCommandContext).Any();

        }
        private ICollection<string> GetDerivedProjectionTexts(SelectCommandContext selectCommandContext)
        {
            ICollection<string> result = new LinkedList<string>();
            foreach (var projection in selectCommandContext.GetProjectionsContext().GetProjections())
            {
                if (projection is AggregationProjection aggregationProjection && aggregationProjection.GetDerivedAggregationProjections().Any()) {
                    result.AddAll(aggregationProjection.GetDerivedAggregationProjections().Select(GetDerivedProjectionText).ToList());
                } else if (projection is DerivedProjection derivedProjection) {
                    result.Add(GetDerivedProjectionText(derivedProjection));
                }
            }
            return result;
        }

        private string GetDerivedProjectionText( IProjection projection)
        {
            ShardingAssert.ShouldBeNotNull(projection.GetAlias(),"alias is required");
            if (projection is AggregationDistinctProjection aggregationDistinctProjection) {
                return aggregationDistinctProjection.GetDistinctInnerExpression() + " AS " + projection.GetAlias() + " ";
            }
            return projection.GetExpression() + " AS " + projection.GetAlias() + " ";
        }
}
}
