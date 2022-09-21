namespace NCDC.ProxyServer.Options;

public class DataSourceOption
{
    public string DataSourceName { get; set; }
    public string ConnectionString { get; set; }
    public bool IsDefault { get; set; }
}