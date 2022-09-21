namespace NCDC.ProxyServer.Contexts.RuntimeContextBuilders;

public interface IDataSourceDiscover
{
    string GetDataSourceName();
    string GetConnectionString();
    bool IsDefault();
}