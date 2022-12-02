using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Abstractions;

public interface IServerDataReaderFactory
{
    IServerDataReader Create(IQueryContext queryContext);
}