using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.Abstractions;

public interface IAppConfiguration
{
    IReadOnlyCollection<string> GetAllDatabaseNames();
    IReadOnlyCollection<Grantee> GetAllUsers();
}