using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.Sharding.Routes;

namespace NCDC.ShardingRewrite.Abstractions;

public interface IParameterRewriterBuilder
{
    ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> GetParameterRewriters(RouteContext routeContext);
}