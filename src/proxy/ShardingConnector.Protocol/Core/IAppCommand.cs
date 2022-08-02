namespace ShardingConnector.Protocol.Core;

public interface IAppCommand
{
    ValueTask ExecuteAsync();
}