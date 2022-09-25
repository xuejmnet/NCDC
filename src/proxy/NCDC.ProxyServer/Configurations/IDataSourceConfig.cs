namespace NCDC.ProxyServer.Configurations;

public interface IDataSourceConfig
{
    string GetDataSourceName();
    string GetConnectionString();
    bool IsDefault();
}