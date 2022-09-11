using NCDC.CommandParser.Abstractions;
using NCDC.Basic.Parser.Command;
using NCDC.Sharding.Routes;

namespace NCDC.Sharding.Rewrites.Abstractions;

public interface IParameterRewriterBuilder
{
    ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> GetParameterRewriters(RouteContext routeContext);
}