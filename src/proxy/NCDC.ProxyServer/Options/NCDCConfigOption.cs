using NCDC.Base;
using NCDC.Enums;
using NCDC.Extensions;

namespace NCDC.ProxyServer.Options;

public sealed class NCDCConfigOption
{
    public DatabaseTypeEnum DatabaseType { get; set; } = DatabaseTypeEnum.MySql;
    public List<UserOption> Users { get; set; } = new List<UserOption>();
    public List<DatabaseOption> Databases { get; set; } = new List<DatabaseOption>();

    public void CheckOptionCompleteness()
    {
        ShardingAssert.ShouldBeNotNull(Users,"users config null");
        ShardingAssert.ShouldBeNotNull(Databases,"databases config null");
        foreach (var userOption in Users)
        {
            ShardingAssert.ShouldBeNotNull(userOption.UserName,"user name config null");
            ShardingAssert.ShouldBeNotNull(userOption.Password,$"user's {userOption.UserName} password config null");
            ShardingAssert.ShouldBeNotNull(userOption.DatabaseNames,$"user's {userOption.UserName} databases config null");
        }
        foreach (var databaseOption in Databases)
        {
            ShardingAssert.ShouldBeNotNull(databaseOption.DatabaseName,"database name config null");
            ShardingAssert.ShouldBeNotNull(databaseOption.DataSources,$"database's {databaseOption.DatabaseName} data source config null");
            ShardingAssert.If(databaseOption.DataSources.IsEmpty(),$"database's {databaseOption.DatabaseName} has no data source config");
            var defaultDataSourceCount = databaseOption.DataSources.Count(o=>o.IsDefault);
            ShardingAssert.If(defaultDataSourceCount==0,$"database's {databaseOption.DatabaseName} should contains default data source");
            ShardingAssert.If(defaultDataSourceCount>1,$"database's {databaseOption.DatabaseName} should contains default data source only one");
            foreach (var dataSourceOption in databaseOption.DataSources)
            {
                ShardingAssert.ShouldBeNotNull(dataSourceOption.DataSourceName,$"database's {databaseOption.DatabaseName} data source name is null");
                ShardingAssert.ShouldBeNotNull(dataSourceOption.ConnectionString,$"database's {databaseOption.DatabaseName} data source's {dataSourceOption.DataSourceName} connection string is null");
            }
        }
        
    }
}