using NCDC.Protocol.Packets;

namespace NCDC.ProxyClient.Abstractions;

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