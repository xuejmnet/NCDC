using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.Contexts;

public interface IUserManager
{
    bool AddUser(AuthUser user);
    bool HasUser(string username);
    AuthUser GetUser(string username);
    IReadOnlyList<string> GetUserNames();
    bool Remove(string username);
}