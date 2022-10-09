using NCDC.Enums;
using NCDC.ProxyServer.Commons;

namespace NCDC.ProxyServer.AppServices.Abstractions;

public interface IAppConfiguration
{
    DatabaseTypeEnum DatabaseType { get; set; }
    DbStorageTypeEnum StorageType { get; set; }
    string ConnectionsString { get; set; }
    int Port { get; set; }
    string RulePluginPath{ get; set; }
    bool LogEncode { get; set; }
    bool LogDecode { get; set; }
}