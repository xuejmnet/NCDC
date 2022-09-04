using OpenConnector.Configuration.Session;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyServer.Abstractions;

public interface IServerDataReaderFactory
{
    IServerDataReader Create(string sql,ConnectionSession connectionSession);
}