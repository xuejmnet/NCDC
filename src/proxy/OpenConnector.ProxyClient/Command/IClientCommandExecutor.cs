namespace OpenConnector.ProxyClient.Command;

public interface IClientCommandExecutor
{
    ValueTask ExecuteAsync();
}