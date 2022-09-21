namespace NCDC.ProxyServer.Options;

public  class DatabaseOption
{
    public string DatabaseName { get; set; }
    public List<DataSourceOption> DataSources { get; set; } = new List<DataSourceOption>();
}