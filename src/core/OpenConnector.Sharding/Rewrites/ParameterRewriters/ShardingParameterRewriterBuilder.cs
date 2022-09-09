using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Sharding.Rewrites.Abstractions;
using OpenConnector.Sharding.Rewrites.ParameterRewriters.Parameters;
using OpenConnector.Sharding.Routes;

namespace OpenConnector.Sharding.Rewrites.ParameterRewriters;

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