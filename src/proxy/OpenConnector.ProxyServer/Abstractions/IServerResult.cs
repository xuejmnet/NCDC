using OpenConnector.ProxyServer.Commons;

namespace OpenConnector.ProxyServer.Abstractions;

public interface IServerResult
{
    ResultTypeEnum ResultType { get; }
}