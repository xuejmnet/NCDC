using OpenConnector.Protocol.Packets;
using OpenConnector.ProxyServer.Commons;

namespace OpenConnector.ProxyClient.Abstractions;

public interface IClientQueryDataReader:IClientDataReader
{
    ResultTypeEnum ResultType { get; }
    
}

public interface IClientQueryDataReader<T> : IClientQueryDataReader, IClientDataReader<T> where T : IPacketPayload
{
    
}