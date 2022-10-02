using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.Configurations.Apps;

public interface IAppUserBuilder
{
    Task<IReadOnlyCollection<AuthUser>> BuildAsync();
}