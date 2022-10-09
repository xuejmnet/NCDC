using NCDC.Basic.User;

namespace NCDC.ProxyServer.AppServices.Abstractions;

public interface IAppUserLoader
{
    bool AddAppUser(AuthUser authUser);
    bool RemoveAppUser(string username);
}