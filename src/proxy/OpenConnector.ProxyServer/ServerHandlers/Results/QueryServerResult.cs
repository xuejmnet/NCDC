using System.Data.Common;
using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Commons;

namespace OpenConnector.ProxyServer.ServerHandlers.Results;

public sealed class QueryServerResult:IServerResult
{

    public ResultTypeEnum ResultType => ResultTypeEnum.QUERY;
    public List<DbColumn> DbColumns { get; }

    public QueryServerResult(List<DbColumn> dbColumns)
    {
        DbColumns = dbColumns;
    }
}