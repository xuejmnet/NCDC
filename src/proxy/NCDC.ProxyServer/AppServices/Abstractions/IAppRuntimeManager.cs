using NCDC.Basic.User;
using NCDC.ProxyServer.Runtimes;

namespace NCDC.ProxyServer.AppServices.Abstractions;

public interface IAppRuntimeManager
{
    bool ContainsRuntimeContext(string databaseName);
    IRuntimeContext GetRuntimeContext(string databaseName);
    ICollection<string> GetAllDatabaseNames();
    ICollection<string> GetAllUsers();
    ICollection<string> GetAuthorizedDatabases(string username);
    ICollection<string> GetAuthorizedUsers(string database);
    AuthUser GetUser(string username);
    bool ContainsAppUser(string username);
}