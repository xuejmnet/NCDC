using NCDC.Enums;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Commons;

namespace NCDC.ProxyServer.AppServices;

public class AppConfiguration : IAppConfiguration
{
    public DatabaseTypeEnum DatabaseType { get; set; } = DatabaseTypeEnum.MySql;
    public DbStorageTypeEnum StorageType { get; set; } = DbStorageTypeEnum.MySql;
    public string ConnectionsString { get; set; } = null!;
    public int Port { get; set; } = 3307;
    public string RulePluginPath { get; set; } = null!;
    public bool LogEncode { get; set; }
    public bool LogDecode { get; set; }
}