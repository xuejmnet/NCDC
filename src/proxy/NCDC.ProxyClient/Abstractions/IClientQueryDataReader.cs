using NCDC.Protocol.Packets;
using NCDC.ProxyServer.Commons;

namespace NCDC.ProxyClient.Abstractions;

public interface IClientQueryDataReader:IClientDataReader
{
    ResultTypeEnum ResultType { get; }
    
}

public interface IClientQueryDataReader<T> : IClientQueryDataReader, IClientDataReader<T> where T : IPacketPayload
{
    
}