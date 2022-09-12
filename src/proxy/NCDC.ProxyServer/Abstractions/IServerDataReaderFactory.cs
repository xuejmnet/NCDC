using NCDC.Basic.Connection.Abstractions;

namespace NCDC.ProxyServer.Abstractions;

public interface IServerDataReaderFactory
{
    IServerDataReader Create(string sql,IConnectionSession connectionSession);
}