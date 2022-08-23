using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClient.Abstractions;

public interface IClientCommand:IDisposable
{
    IClientDataReader ExecuteReader();
    void IDisposable.Dispose(){}
}

public interface IClientCommand<T>:IClientCommand where T : IPacketPayload
{
    IClientDataReader<T> ExecuteReader();

    IClientDataReader IClientCommand.ExecuteReader()
    {
        return ExecuteReader();
    }
}