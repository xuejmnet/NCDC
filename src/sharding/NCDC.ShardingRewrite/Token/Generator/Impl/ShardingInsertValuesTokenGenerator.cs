using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Segment.DML.Assignment;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingRewrite.Sql.Token.Generator;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject.Generic;
using NCDC.ShardingRewrite.Token.SimpleObject;
using NCDC.ShardingRoute;
using NCDC.Extensions;

namespace NCDC.ShardingRewrite.Token.Generator.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 8:27:40
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingInsertValuesTokenGenerator: IOptionalSqlTokenGenerator<InsertCommandContext>
    {
        private readonly RouteContext _routeContext;

        public ShardingInsertValuesTokenGenerator(RouteContext routeContext)
        {
            _routeContext = routeContext;
        }
        public SqlToken GenerateSqlToken(InsertCommandContext sqlCommandContext)
        {
            ICollection<InsertValuesSegment> insertValuesSegments = sqlCommandContext.GetSqlCommand().Values;
            InsertValuesToken result = new ShardingInsertValuesToken(GetStartIndex(insertValuesSegments), GetStopIndex(insertValuesSegments));
            var originalDataNodesIterator = null == _routeContext || _routeContext.GetRouteResult().GetOriginalDataNodes().IsEmpty()
                ? null : _routeContext.GetRouteResult().GetOriginalDataNodes().GetEnumerator();

            foreach (var insertValueContext in sqlCommandContext.GetInsertValueContexts())
            {
                List<IExpressionSegment> expressionSegments = insertValueContext.GetValueExpressions();
                ICollection<DataNode> dataNodes = null == originalDataNodesIterator ? new List<DataNode>(0) : originalDataNodesIterator.Next();
                result.InsertValues.Add(new ShardingInsertValue(expressionSegments, dataNodes));
            }
            return result;
        }

        public SqlToken GenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return GenerateSqlToken((InsertCommandContext)sqlCommandContext);
        }

        private int GetStartIndex(ICollection<InsertValuesSegment> segments)
        {
            int result = segments.First().GetStartIndex();
            foreach (var segment in segments)
            {
                result = result > segment.GetStartIndex() ? segment.GetStartIndex() : result;
            }
            return result;
        }

        private int GetStopIndex(ICollection<InsertValuesSegment> segments)
        {
            int result = segments.First().GetStopIndex();
            foreach (var segment in segments)
            {
                result = result < segment.GetStopIndex() ? segment.GetStopIndex() : result;
            }
            return result;
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is InsertCommandContext insertCommandContext && insertCommandContext.GetSqlCommand().Values.Any();
        }
    }
}
