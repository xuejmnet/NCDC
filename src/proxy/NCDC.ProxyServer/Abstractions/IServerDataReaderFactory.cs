using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Abstractions;

public interface IServerDataReaderFactory
{
    IServerDataReader Create(string sql,ISqlCommand sqlCommand,IConnectionSession connectionSession);
    IServerDataReader Create(string sql,IConnectionSession connectionSession);
}