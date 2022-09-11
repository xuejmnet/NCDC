using OpenConnector.Configuration.Connection.Abstractions;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Routes;

public sealed class RouteContextFactory:IRouteContextFactory
{
    public RouteContextFactory()
    {
        
    }
    public RouteContext Create(IConnectionSession connectionSession, string sql, ParameterContext parameterContext)
    {
        throw new NotImplementedException();
    }
}