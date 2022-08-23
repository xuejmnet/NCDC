using ShardingConnector.Protocol.Packets;

namespace ShardingConnector.ProxyClient.Abstractions;

public interface IClientDataReader:IDisposable
{
    /// <summary>
    /// 发送命令到数据库
    /// </summary>
    /// <returns>数据库返回结果</returns>
    List<IPacket> SendCommand();

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