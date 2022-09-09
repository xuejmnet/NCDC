using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.Sharding.Routes;

namespace OpenConnector.Sharding.Rewrites.Abstractions;

public interface IParameterRewriterBuilder
{
    ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> GetParameterRewriters(RouteContext routeContext);
}