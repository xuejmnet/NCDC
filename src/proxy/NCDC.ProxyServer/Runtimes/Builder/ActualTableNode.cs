namespace NCDC.ProxyServer.Runtimes.Builder;

public sealed class ActualTableNode
{
    public string DataSource { get; }
    public string TableName { get; }

    public ActualTableNode(string dataSource,string tableName)
    {
        DataSource = dataSource;
        TableName = tableName;
    }
}