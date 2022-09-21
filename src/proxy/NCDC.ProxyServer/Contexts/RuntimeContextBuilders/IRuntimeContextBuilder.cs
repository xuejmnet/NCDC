namespace NCDC.ProxyServer.Contexts.RuntimeContextBuilders;

public interface IRuntimeContextBuilder
{
    IRuntimeContext BuildRuntimeContext(IDatabaseDiscover databaseDiscover);
}