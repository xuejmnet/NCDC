namespace NCDC.ProxyServer.AppServices.Abstractions;

public interface IUserDatabaseMappingLoader
{
    bool AddUserDatabaseMapping(UserDatabaseEntry entry);
    bool RemoveUserDatabaseMapping(UserDatabaseEntry entry);
    bool ContainsUserDatabaseMapping(UserDatabaseEntry entry);
}