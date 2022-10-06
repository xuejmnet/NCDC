using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.AppServices;

public interface IAppUserLoader
{
    bool AddAppUser(AuthUser authUser);
    bool RemoveAppUser(string username);
}