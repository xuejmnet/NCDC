using NCDC.ProxyServer.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.Query.ServerHandlers;

public sealed class MySqlMultiServerHandler:IServerHandler
{
    public Task<IServerResult> ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}