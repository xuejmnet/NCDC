using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.Configurations;

public interface IAppAuthUserBuilder
{
    Task<IReadOnlyList<AuthUser>> BuildAsync();
}