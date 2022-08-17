using DotNetty.Buffers;

namespace ShardingConnector.ProtocolCore.Payloads;

/// <summary>
/// 数据库命令包内容
/// </summary>
public interface IPacketPayload:IDisposable
{
    IByteBuffer GetByteBuffer();
}