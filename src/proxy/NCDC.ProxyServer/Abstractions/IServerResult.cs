using NCDC.ProxyServer.Commons;

namespace NCDC.ProxyServer.Abstractions;

public interface IServerResult
{
    ResultTypeEnum ResultType { get; }
}