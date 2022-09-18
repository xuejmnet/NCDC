using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.Contexts;

public interface IContextManager
{
    bool AddRuntimeContext(string databaseName,IRuntimeContext runtimeContext);
    IRuntimeContext GetRuntimeContext(string databaseName);
    bool HasRuntimeContext(string databaseName);
    IReadOnlyCollection<string> GetAllDatabaseNames();
    IReadOnlyCollection<string> GetAllUsers();
    IReadOnlyCollection<string> GetAuthorizedDatabases(string username);
    IReadOnlyCollection<string> GetAuthorizedUsers(string database);
    AuthUser? TryGetUser(string username);
}