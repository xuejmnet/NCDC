namespace ShardingConnector.ProxyClient.Command;

public interface IClientCommandExecutor
{
    ValueTask ExecuteAsync();
}