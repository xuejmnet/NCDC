using NCDC.Enums;
using NCDC.ProxyServer.Commons;
using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.Abstractions;

public interface IAppConfiguration
{
    DatabaseTypeEnum GetDatabaseType();
    ConfigurationStorageTypeEnum GetConfigurationStorageType();
    int GetPort();
    string GetRoutePluginPath();
}