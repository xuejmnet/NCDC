using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.ShardingRoute;

namespace NCDC.ShardingRewrite.Abstractions;

public interface IParameterRewriterBuilder
{
    ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> GetParameterRewriters(RouteContext routeContext);
}