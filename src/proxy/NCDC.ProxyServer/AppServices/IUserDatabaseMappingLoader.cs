namespace NCDC.ProxyServer.AppServices;

public interface IUserDatabaseMappingLoader
{
    bool AddUserDatabaseMapping(UserDatabaseEntry entry);
    bool RemoveUserDatabaseMapping(UserDatabaseEntry entry);
    bool ContainsUserDatabaseMapping(UserDatabaseEntry entry);
}