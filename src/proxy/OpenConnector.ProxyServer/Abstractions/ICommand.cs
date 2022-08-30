namespace OpenConnector.ProxyServer.Abstractions;

public interface ICommand
{
    ValueTask ExecuteAsync();
}