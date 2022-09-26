namespace NCDC.ProxyServer.Abstractions;

public interface IAppDatabaseConfiguration
{
    IReadOnlyCollection<string> GetDatabases();
}