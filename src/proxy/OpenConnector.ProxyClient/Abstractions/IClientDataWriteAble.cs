using DotNetty.Transport.Channels;
using OpenConnector.Configuration.Session;
using OpenConnector.Protocol.Packets;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClient.Abstractions;

public interface IClientDataWriteAble
{
    ValueTask WriteQueryDataAsync(IChannelHandlerContext context, ConnectionSession connectionSession,
        IClientQueryDataReader clientQueryDataReader, int headerPackagesCount);
}

public interface IClientDataWriteAble<T> : IClientDataWriteAble where T : IPacketPayload
{
    ValueTask WriteQueryDataAsync(IChannelHandlerContext context, ConnectionSession connectionSession,
        IClientQueryDataReader<T> clientQueryDataReader, int headerPackagesCount);

    ValueTask IClientDataWriteAble.WriteQueryDataAsync(IChannelHandlerContext context, ConnectionSession connectionSession,
        IClientQueryDataReader clientQueryDataReader, int headerPackagesCount)
    {
        return WriteQueryDataAsync(context, connectionSession, (IClientQueryDataReader<T>)clientQueryDataReader,
            headerPackagesCount);
    }
}