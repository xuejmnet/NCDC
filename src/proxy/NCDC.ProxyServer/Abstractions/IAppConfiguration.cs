using NCDC.Enums;
using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.Abstractions;

public interface IAppConfiguration
{
    DatabaseTypeEnum GetStoreDatabaseType();
    DatabaseTypeEnum GetProxyDatabaseType();
    string GetConfigStoreConnectionString();
    string GetRoutePluginPath();
}