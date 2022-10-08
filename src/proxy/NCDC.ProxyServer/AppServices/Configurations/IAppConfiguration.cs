using NCDC.Enums;
using NCDC.ProxyServer.Commons;

namespace NCDC.ProxyServer.AppServices.Configurations;

public interface IAppConfiguration
{
    DatabaseTypeEnum GetDatabaseType();
    DbStorageTypeEnum GetStorageType();
    string ConnectionsString();
    int GetPort();
    string GetRulePluginPath();
    bool LogEncode();
    bool LogDecode();
}