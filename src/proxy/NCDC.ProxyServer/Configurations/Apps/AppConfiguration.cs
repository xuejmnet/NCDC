using NCDC.Enums;
using NCDC.ProxyServer.Commons;

namespace NCDC.ProxyServer.Configurations.Apps;

public class AppConfiguration:IAppConfiguration
{
    private readonly DatabaseTypeEnum _databaseType;
    private readonly DbStorageTypeEnum _dbStorageType;
    private readonly string _connectionString;
    private readonly int _port;
    private readonly string _rulePluginPath;
    private readonly bool _logEncode;
    private readonly bool _logDecode;

    public AppConfiguration(DatabaseTypeEnum databaseType,DbStorageTypeEnum dbStorageType,string connectionString,int port,string rulePluginPath,bool logEncode,bool logDecode)
    {
        _databaseType = databaseType;
        _dbStorageType = dbStorageType;
        _connectionString = connectionString;
        _port = port;
        _rulePluginPath = rulePluginPath;
        _logEncode = logEncode;
        _logDecode = logDecode;
    }
    public DatabaseTypeEnum GetDatabaseType()
    {
        return _databaseType;
    }

    public DbStorageTypeEnum GetStorageType()
    {
        return _dbStorageType;
    }

    public string ConnectionsString()
    {
        return _connectionString;
    }

    public int GetPort()
    {
        return _port;
    }

    public string GetRulePluginPath()
    {
        return _rulePluginPath;
    }

    public bool LogEncode()
    {
        return _logEncode;
    }

    public bool LogDecode()
    {
        return _logDecode;
    }
}