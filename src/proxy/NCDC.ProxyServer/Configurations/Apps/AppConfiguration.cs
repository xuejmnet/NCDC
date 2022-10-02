using NCDC.Enums;
using NCDC.ProxyServer.Commons;

namespace NCDC.ProxyServer.Configurations.Apps;

public class AppConfiguration:IAppConfiguration
{
    private readonly DatabaseTypeEnum _databaseType;
    private readonly ConfigurationStorageTypeEnum _configurationStorageType;
    private readonly int _port;
    private readonly string _rulePluginPath;

    public AppConfiguration(DatabaseTypeEnum databaseType,ConfigurationStorageTypeEnum configurationStorageType,int port,string rulePluginPath)
    {
        _databaseType = databaseType;
        _configurationStorageType = configurationStorageType;
        _port = port;
        _rulePluginPath = rulePluginPath;
    }
    public DatabaseTypeEnum GetDatabaseType()
    {
        return _databaseType;
    }

    public ConfigurationStorageTypeEnum GetConfigurationStorageType()
    {
        return _configurationStorageType;
    }

    public int GetPort()
    {
        return _port;
    }

    public string GetRulePluginPath()
    {
        return _rulePluginPath;
    }
}