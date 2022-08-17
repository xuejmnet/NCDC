namespace ShardingConnector.ProxyClient.Command;

public interface IClientCommand
{
    ValueTask ExecuteAsync();
}