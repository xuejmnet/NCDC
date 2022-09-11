using System.Data.Common;

namespace NCDC.Basic.Configurations;

public sealed class RuntimeOption
{
    public DbProviderFactory DbProviderFactory { get; set; }
    public List<UserOption> Users { get; set; } = new ();
    public List<DatabaseOption> Databases { get; set; } = new List<DatabaseOption>();
}

public sealed class DatabaseOption
{
    public string Name { get; set; }
    public List<DataSourceOption> DataSources { get; set; } = new ();
}

public sealed class UserOption
{
    public string Username { get; set; }
    public string Password { get; set; }
    public ISet<string> Databases { get; set; } = new HashSet<string>();
}

public sealed class DataSourceOption
{
    public string DataSourceName { get; set; }
    public string ConnectionString { get; set; }
    public bool IsDefault { get; set; }
}