using NCDC.Basic.Parsers;
using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.ShardingRewrite.Abstractions;
using NCDC.ShardingRewrite.ParameterRewriters.Parameters;
using NCDC.ShardingRoute;

namespace NCDC.ShardingRewrite.ParameterRewriters;

public sealed class ShardingParameterRewriterBuilder:IParameterRewriterBuilder
{
    public ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> GetParameterRewriters(RouteContext routeContext)
    {
        ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> result = new LinkedList<IParameterRewriter<ISqlCommandContext<ISqlCommand>>>();
        result.Add(new ShardingGeneratedKeyInsertValueParameterRewriter());
        result.Add(new ShardingPaginationParameterRewriter(routeContext));
        return result;
    }
}