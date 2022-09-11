namespace NCDC.Sharding.Routes.Abstractions;

public interface IRouteContextFactory
{
    RouteContext Create(SqlParserResult sqlParserResult);
}