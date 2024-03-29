using System.Data.Common;
using NCDC.StreamDataReaders;

namespace NCDC.ProxyServer.StreamMerges.Results;

public sealed class QueryExecuteResult:IExecuteResult
{
    public List<DbColumn> DbColumns { get; }
    public IStreamDataReader StreamDataReader { get; }

    public QueryExecuteResult(List<DbColumn> dbColumns,IStreamDataReader streamDataReader)
    {
        DbColumns = dbColumns;
        StreamDataReader = streamDataReader;
    }

    public void Dispose()
    {
        StreamDataReader.Dispose();
    }
}