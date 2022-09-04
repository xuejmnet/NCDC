using DotNetty.Transport.Channels;
using OpenConnector.Configuration.Session;
using OpenConnector.Protocol.Packets;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClient.Abstractions;

public interface IClientDbConnection:IClientDataWriteAble
{
    IClientCommand CreateCommand(IPacketPayload payload, ConnectionSession connectionSession);
    IPacket GetErrorPacket(Exception exception);
    IPacket? GetOtherPacket(ConnectionSession connectionSession);
}

public interface IClientDbConnection<T> : IClientDbConnection,IClientDataWriteAble<T> where T : IPacketPayload
{
    IClientCommand<T> CreateCommand(T payload, ConnectionSession connectionSession);
    
    
    IPacket<T> GetErrorPacket(Exception exception);
    IPacket<T>? GetOtherPacket(ConnectionSession connectionSession);

    #region 默认实现

    IClientCommand IClientDbConnection.CreateCommand(IPacketPayload payload, ConnectionSession connectionSession)
    {
        return CreateCommand((T)payload, connectionSession);
    }

    IPacket IClientDbConnection.GetErrorPacket(Exception exception)
    {
        return GetErrorPacket(exception);
    }
    IPacket? IClientDbConnection.GetOtherPacket(ConnectionSession connectionSession)
    {
        return GetOtherPacket(connectionSession);
    }

    #endregion
    
}