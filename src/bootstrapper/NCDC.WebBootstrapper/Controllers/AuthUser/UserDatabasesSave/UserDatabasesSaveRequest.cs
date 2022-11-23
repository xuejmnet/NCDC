namespace NCDC.WebBootstrapper.Controllers.AuthUser.UserDatabasesSave;

public class UserDatabasesSaveRequest
{
    public string Id { get; set; }
    public List<string>  CheckedDatabases{ get; set; } = new List<string>();
}