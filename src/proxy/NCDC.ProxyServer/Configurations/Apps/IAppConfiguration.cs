using NCDC.Enums;
using NCDC.ProxyServer.Commons;

namespace NCDC.ProxyServer.Configurations.Apps;

public interface IAppConfiguration
{
    DatabaseTypeEnum GetDatabaseType();
    ConfigurationStorageTypeEnum GetConfigurationStorageType();
    int GetPort();
    string GetRulePluginPath();
}