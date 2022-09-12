using System.Data.Common;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Commons;

namespace NCDC.ProxyServer.ServerHandlers.Results;

public sealed class QueryServerResult:IServerResult
{

    public ResultTypeEnum ResultType => ResultTypeEnum.QUERY;
    public List<DbColumn> DbColumns { get; }

    public QueryServerResult(List<DbColumn> dbColumns)
    {
        DbColumns = dbColumns;
    }
}