namespace NCDC.ProxyServer.Abstractions;

public interface ICommand
{
    ValueTask ExecuteAsync();
}