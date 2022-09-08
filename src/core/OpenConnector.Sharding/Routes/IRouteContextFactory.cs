using OpenConnector.Configuration.Connection.Abstractions;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Routes;

public interface IRouteContextFactory
{
    RouteContext Create(IConnectionSession connectionSession,string sql, ParameterContext parameterContext);
}