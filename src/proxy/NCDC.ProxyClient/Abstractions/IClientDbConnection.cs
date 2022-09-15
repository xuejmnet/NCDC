using DotNetty.Transport.Channels;
using NCDC.Protocol.Packets;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClient.Abstractions;

public interface IClientDbConnection:IClientDataWriteAble
{
    IClientCommand CreateCommand(IPacketPayload payload, IConnectionSession connectionSession);
    IPacket GetErrorPacket(Exception exception);
    IPacket? GetOtherPacket(IConnectionSession connectionSession);
}

public interface IClientDbConnection<T> : IClientDbConnection,IClientDataWriteAble<T> where T : IPacketPayload
{
    IClientCommand<T> CreateCommand(T payload, IConnectionSession connectionSession);
    
    
    IPacket<T> GetErrorPacket(Exception exception);
    IPacket<T>? GetOtherPacket(IConnectionSession connectionSession);

    #region 默认实现

    IClientCommand IClientDbConnection.CreateCommand(IPacketPayload payload, IConnectionSession connectionSession)
    {
        return CreateCommand((T)payload, connectionSession);
    }

    IPacket IClientDbConnection.GetErrorPacket(Exception exception)
    {
        return GetErrorPacket(exception);
    }
    IPacket? IClientDbConnection.GetOtherPacket(IConnectionSession connectionSession)
    {
        return GetOtherPacket(connectionSession);
    }

    #endregion
    
}