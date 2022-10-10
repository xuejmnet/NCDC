using NCDC.Protocol.Packets;

namespace NCDC.ProxyClient.Abstractions;

public interface IClientDataReader:IDisposable
{
    /// <summary>
    /// 发送命令到数据库
    /// </summary>
    /// <returns>数据库返回结果</returns>
    IAsyncEnumerable<IPacket> SendCommand();

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
    IAsyncEnumerable<IPacket<T>> SendCommand();

    IAsyncEnumerable<IPacket> IClientDataReader.SendCommand()
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