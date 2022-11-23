namespace NCDC.WebBootstrapper.Controllers.AuthUser.UserDatabases;

public class UserDatabasesResponse
{
    public List<string>  CheckedDatabases{ get; set; } = new List<string>();
    public List<UserDatabaseAllResponse> AllDatabases { get; set; } = new List<UserDatabaseAllResponse>();
}

public class UserDatabaseAllResponse
{
    public string Id { get; set; }
    public string DatabaseName { get; set; }
}