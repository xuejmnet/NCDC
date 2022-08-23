using ShardingConnector.ProxyServer.Commons;

namespace ShardingConnector.ProxyClient.Abstractions;

public interface IClientQueryDataReader:IClientDataReader
{
    ResultTypeEnum ResultType { get; }
    
}