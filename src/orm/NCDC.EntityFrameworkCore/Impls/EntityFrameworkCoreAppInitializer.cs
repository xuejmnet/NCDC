using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCDC.Basic.User;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.Enums;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Bootstrappers;
using NCDC.ProxyServer.Runtimes.Builder;

namespace NCDC.EntityFrameworkCore.Impls;

public class EntityFrameworkCoreAppInitializer : AbstractAppInitializer
{
    private readonly IServiceProvider _appServiceProvider;

    public EntityFrameworkCoreAppInitializer(IServiceProvider appServiceProvider, IAppRuntimeLoader appRuntimeLoader,
        IAppUserLoader appUserLoader, IUserDatabaseMappingLoader userDatabaseMappingLoader,
        IAppRuntimeBuilder appRuntimeBuilder) : base(appRuntimeLoader, appUserLoader, userDatabaseMappingLoader,
        appRuntimeBuilder)
    {
        _appServiceProvider = appServiceProvider;
    }

    protected override async Task PreInitializeAsync()
    {
        using (var scope = _appServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            if (!await dbContext.Set<LogicDatabaseEntity>().AnyAsync())
            {
                var logicDatabase = new LogicDatabaseEntity();
                logicDatabase.Id = Guid.NewGuid().ToString("n");
                logicDatabase.CreateTime=DateTime.Now;
                logicDatabase.UpdateTime=DateTime.Now;
                logicDatabase.Version= Guid.NewGuid().ToString("n");
                logicDatabase.Name = "xxa";
                logicDatabase.AutoUseWriteConnectionStringAfterWriteDb = true;
                logicDatabase.ThrowIfQueryRouteNotMatch = false;
                logicDatabase.MaxQueryConnectionsLimit = Environment.ProcessorCount;
                logicDatabase.ConnectionMode = ConnectionModeEnum.SYSTEM_AUTO;
                await dbContext.AddAsync(logicDatabase);
                var dataSource = new DataSourceEntity();
                dataSource.Id = Guid.NewGuid().ToString("n");
                dataSource.CreateTime=DateTime.Now;
                dataSource.UpdateTime=DateTime.Now;
                dataSource.Version= Guid.NewGuid().ToString("n");
                dataSource.Database = "xxa";
                dataSource.Name = "ds0";
                dataSource.IsDefault = true;
                dataSource.ConnectionString = "server=127.0.0.1;port=3306;database=test;userid=root;password=root;";
                await dbContext.AddAsync(dataSource);
                var logicTable = new LogicTableEntity();
                logicTable.Id = Guid.NewGuid().ToString("n");
                logicTable.CreateTime=DateTime.Now;
                logicTable.UpdateTime=DateTime.Now;
                logicTable.Version= Guid.NewGuid().ToString("n");
                logicTable.TableName = "sysusermod";
                logicTable.Database = "xxa";
                logicTable.ShardingTableRule = "ShardingRoutePluginTest.TestModTableRouteRule";
                await dbContext.AddAsync(logicTable);
                var actualTable0 = new ActualTableEntity();
                actualTable0.Id = Guid.NewGuid().ToString("n");
                actualTable0.CreateTime=DateTime.Now;
                actualTable0.UpdateTime=DateTime.Now;
                actualTable0.Version= Guid.NewGuid().ToString("n");
                actualTable0.Database = "xxa";
                actualTable0.LogicTableName = "sysusermod";
                actualTable0.DataSource = "ds0";
                actualTable0.TableName = "sysusermod_00";
                await dbContext.AddAsync(actualTable0);
                var actualTable1 = new ActualTableEntity();
                actualTable1.Id = Guid.NewGuid().ToString("n");
                actualTable1.CreateTime=DateTime.Now;
                actualTable1.UpdateTime=DateTime.Now;
                actualTable1.Version= Guid.NewGuid().ToString("n");
                actualTable1.Database = "xxa";
                actualTable1.LogicTableName = "sysusermod";
                actualTable1.DataSource = "ds0";
                actualTable1.TableName = "sysusermod_01";
                await dbContext.AddAsync(actualTable1);
                var actualTable2 = new ActualTableEntity();
                actualTable2.Id = Guid.NewGuid().ToString("n");
                actualTable2.CreateTime=DateTime.Now;
                actualTable2.UpdateTime=DateTime.Now;
                actualTable2.Version= Guid.NewGuid().ToString("n");
                actualTable2.Database = "xxa";
                actualTable2.LogicTableName = "sysusermod";
                actualTable2.DataSource = "ds0";
                actualTable2.TableName = "sysusermod_02";
                await dbContext.AddAsync(actualTable2);
                var appAuthUser = new AppAuthUserEntity();
                appAuthUser.Id = Guid.NewGuid().ToString("n");
                appAuthUser.CreateTime=DateTime.Now;
                appAuthUser.UpdateTime=DateTime.Now;
                appAuthUser.Version= Guid.NewGuid().ToString("n");
                appAuthUser.UserName = "xjm";
                appAuthUser.Password = "abc";
                appAuthUser.HostName = "%";
                appAuthUser.IsEnable = true;
                await dbContext.AddAsync(appAuthUser);
                var logicDatabaseUser = new LogicDatabaseUserEntity();
                logicDatabaseUser.Id = Guid.NewGuid().ToString("n");
                logicDatabaseUser.CreateTime=DateTime.Now;
                logicDatabaseUser.UpdateTime=DateTime.Now;
                logicDatabaseUser.Version= Guid.NewGuid().ToString("n");
                logicDatabaseUser.Database = "xxa";
                logicDatabaseUser.UserName = "xjm";
                await dbContext.AddAsync(logicDatabaseUser);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    protected override async Task<IReadOnlyCollection<string>> GetRuntimesAsync()
    {
        using (var scope = _appServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var logicDatabases = await dbContext.Set<LogicDatabaseEntity>().Select(o => o.Name).ToListAsync();
            return logicDatabases.AsReadOnly();
        }
    }

    protected override async Task<IReadOnlyCollection<AuthUser>> GetAuthUsersAsync()
    {
        using (var scope = _appServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var appAuthUsers = await dbContext.Set<AppAuthUserEntity>().Where(o => o.IsEnable)
                .Select(o => new AuthUser(o.UserName, o.Password, o.HostName)).ToListAsync();
            return appAuthUsers.AsReadOnly();
        }
    }

    protected override async Task<IReadOnlyCollection<UserDatabaseEntry>> GetUserDatabasesAsync()
    {
        using (var scope = _appServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var appAuthUsers = await dbContext.Set<LogicDatabaseUserEntity>()
                .Select(o => new UserDatabaseEntry(o.UserName, o.Database)).ToListAsync();
            return appAuthUsers.AsReadOnly();
        }
    }
}