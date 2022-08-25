using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Binaries;

namespace ShardingConnector.ProxyServer.Connectors;

public sealed class AdoNetServerConnector:IServerConnector
{
    public AdoNetServerConnector()
    {
        
    }
    public IServerResult Execute()
    {
        throw new NotImplementedException();
    }

    public bool Read()
    {
        throw new NotImplementedException();
    }

    public BinaryRow GetQueryRow()
    {
        throw new NotImplementedException();
    }
}