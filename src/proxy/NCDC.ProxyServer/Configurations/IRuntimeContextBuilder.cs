using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyServer.Configurations;

public interface IRuntimeContextBuilder
{
    IRuntimeContext Build(string databaseName);
}