namespace ShardingConnector.ProxyServer.Response.Data;

public class QueryResponseRow
{
    public List<QueryResponseCell> Cells { get; }

    public QueryResponseRow(List<QueryResponseCell> cells)
    {
        Cells = cells;
    }

    public List<object?> GetData()
    {
        var objects = new List<object?>(Cells.Count);
        foreach (var cell in Cells)
        {
            objects.Add(cell.Data);
        }

        return objects;
    }
}