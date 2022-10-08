using System.Diagnostics.CodeAnalysis;
using NCDC.ProxyServer.Runtimes;

namespace NCDC.ProxyServer.AppServices;

public interface IAppRuntimeLoader
{
    bool LoadRuntimeContext(IRuntimeContext runtimeContext);
    bool UnLoadRemoveRuntimeContext(string databaseName,[MaybeNullWhen(false)] out IRuntimeContext runtimeContext);
}