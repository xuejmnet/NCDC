using NCDC.ProxyClient.Command.Abstractions;

namespace NCDC.ProxyClient.Command;

public sealed class MessageExecutorFactory:IMessageExecutorFactory
{
    public IMessageExecutor Create()
    {
        return new MessageExecutor();
    }
}