using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.MetaData;
using NCDC.Sharding.Rewrites.Abstractions;
using NCDC.Sharding.Rewrites.ParameterRewriters.Parameters;
using NCDC.Sharding.Routes;

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