using DotNetty.Transport.Channels;
using NCDC.Protocol.Packets;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClient.Abstractions;

public interface IClientDataWriteAble
{
    ValueTask WriteQueryDataAsync(IChannelHandlerContext context, IConnectionSession connectionSession,
        IClientQueryDataReader clientQueryDataReader, int headerPackagesCount);
}

public interface IClientDataWriteAble<T> : IClientDataWriteAble where T : IPacketPayload
{
    ValueTask WriteQueryDataAsync(IChannelHandlerContext context, IConnectionSession connectionSession,
        IClientQueryDataReader<T> clientQueryDataReader, int headerPackagesCount);

    ValueTask IClientDataWriteAble.WriteQueryDataAsync(IChannelHandlerContext context, IConnectionSession connectionSession,
        IClientQueryDataReader clientQueryDataReader, int headerPackagesCount)
    {
        return WriteQueryDataAsync(context, connectionSession, (IClientQueryDataReader<T>)clientQueryDataReader,
            headerPackagesCount);
    }
}