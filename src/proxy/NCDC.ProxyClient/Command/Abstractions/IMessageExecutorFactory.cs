namespace NCDC.ProxyClient.Command.Abstractions;

public interface IMessageExecutorFactory
{
    IMessageExecutor Create();
}