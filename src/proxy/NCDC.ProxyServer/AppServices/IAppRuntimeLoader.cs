using System.Diagnostics.CodeAnalysis;
using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyServer.AppServices;

public interface IAppRuntimeLoader
{
    bool LoadRuntimeContext(IRuntimeContext runtimeContext);
    bool UnLoadRemoveRuntimeContext(string databaseName,[MaybeNullWhen(false)] out IRuntimeContext runtimeContext);
}