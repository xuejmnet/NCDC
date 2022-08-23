using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyServer.Commons;

namespace ShardingConnector.ProxyClient.Abstractions;

public interface IClientQueryDataReader:IClientDataReader
{
    ResultTypeEnum ResultType { get; }
    
}

public interface IClientQueryDataReader<T> : IClientQueryDataReader, IClientDataReader<T> where T : IPacketPayload
{
    
}