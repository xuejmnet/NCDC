using ShardingConnector.Protocol.Packets;

namespace ShardingConnector.ProxyClient.Abstractions;

public interface IClientDataReader:IDisposable
{
    /// <summary>
    /// 发送命令到数据库
    /// </summary>
    /// <returns>数据库返回结果</returns>
    IEnumerable<IPacket> SendCommand();

    bool MoveNext()
    {
        return false;
    }

    IPacket? GetRowPacket()
    {
        return null;
    }

    void IDisposable.Dispose()
    {
        
    }
}

public interface IClientDataReader<T>:IClientDataReader where T : IPacketPayload
{
    IEnumerable<IPacket<T>> SendCommand();

    IEnumerable<IPacket> IClientDataReader.SendCommand()
    {
        return SendCommand();
    }

    IPacket<T>? GetRowPacket()
    {
        return null;
    }

    IPacket? IClientDataReader.GetRowPacket()
    {
        return GetRowPacket();
    }
}