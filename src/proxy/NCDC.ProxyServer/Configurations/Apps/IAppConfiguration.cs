using NCDC.Enums;
using NCDC.ProxyServer.Commons;

namespace NCDC.ProxyServer.Configurations.Apps;

public interface IAppConfiguration
{
    DatabaseTypeEnum GetDatabaseType();
    DbStorageTypeEnum GetStorageType();
    string ConnectionsString();
    int GetPort();
    string GetRulePluginPath();
}