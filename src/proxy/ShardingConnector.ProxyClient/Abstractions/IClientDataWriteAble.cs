using DotNetty.Transport.Channels;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClient.Abstractions;

public interface IClientDataWriteAble
{
    void WriteQueryData(IChannelHandlerContext context, ConnectionSession connectionSession,
        IClientQueryDataReader clientQueryDataReader, int headerPackagesCount);
}

public interface IClientDataWriteAble<T> : IClientDataWriteAble where T : IPacketPayload
{
    void WriteQueryData(IChannelHandlerContext context, ConnectionSession connectionSession,
        IClientQueryDataReader<T> clientQueryDataReader, int headerPackagesCount);

    void IClientDataWriteAble.WriteQueryData(IChannelHandlerContext context, ConnectionSession connectionSession,
        IClientQueryDataReader clientQueryDataReader, int headerPackagesCount)
    {
        WriteQueryData(context, connectionSession, (IClientQueryDataReader<T>)clientQueryDataReader,
            headerPackagesCount);
    }
}