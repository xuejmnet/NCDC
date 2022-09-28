using NCDC.ProxyServer.Connection.User;
using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyServer.Configurations.Initializers;

public class DefaultAppUserInitializer:IAppUserInitializer
{
    private readonly IAppUserManager _appUserManager;
    private readonly IAppAuthUserBuilder _appAuthUserBuilder;

    public DefaultAppUserInitializer(IAppUserManager appUserManager,IAppAuthUserBuilder appAuthUserBuilder)
    {
        _appUserManager = appUserManager;
        _appAuthUserBuilder = appAuthUserBuilder;
    }
    public async Task InitializeAsync()
    {
        var users = await _appAuthUserBuilder.BuildAsync();
        foreach (var authUser in users)
        {
            _appUserManager.AddUser(new AuthUser(authUser.Grantee.Username, authUser.Password,
                authUser.Grantee.Hostname));
        }
    }
}