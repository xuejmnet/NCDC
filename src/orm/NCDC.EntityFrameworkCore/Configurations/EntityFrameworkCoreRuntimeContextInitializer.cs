
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCDC.Basic.TableMetadataManagers;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.Enums;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Contexts.Initializers;
using NCDC.ProxyServer.Databases;
using NCDC.ProxyServer.Options;
using NCDC.ProxyServer.ServiceProviders;

namespace NCDC.EntityFrameworkCore.Configurations;

public class EntityFrameworkCoreRuntimeContextInitializer:IRuntimeContextInitializer
{
    private readonly IShardingProvider _shardingProvider;
    private readonly IVirtualDataSource _virtualDataSource;
    private readonly IRouteInitConfigOption _routeInitConfigOption;
    private readonly ITableMetadataInitializer _tableMetadataInitializer;
    private readonly IDbProviderFactory _dbProviderFactory;
    private readonly IAppConfiguration _appConfiguration;

    public EntityFrameworkCoreRuntimeContextInitializer(IShardingProvider shardingProvider,IVirtualDataSource virtualDataSource,IRouteInitConfigOption routeInitConfigOption,ITableMetadataInitializer tableMetadataInitializer)
    {
        _shardingProvider = shardingProvider;
        _virtualDataSource = virtualDataSource;
        _routeInitConfigOption = routeInitConfigOption;
        _tableMetadataInitializer = tableMetadataInitializer;
        _dbProviderFactory =_shardingProvider.ApplicationServiceProvider.GetRequiredService<IDbProviderFactory>();
        _appConfiguration =_shardingProvider.ApplicationServiceProvider.GetRequiredService<IAppConfiguration>();
    }
    public async Task InitializeAsync()
    {
        using (var serviceScope = _shardingProvider.ApplicationServiceProvider.CreateScope())
        {
        

            var dbContext = serviceScope.ServiceProvider.GetRequiredService<NCDCDbContext>();
             var dbActualTables = await dbContext.Set<ActualTableEntity>().Where(o=>o.Database==_virtualDataSource.GetDatabaseName()).ToListAsync();
             
           
             var logicTableNames = _routeInitConfigOption.GetTableRouteRules().Keys.Union(_routeInitConfigOption.GetDataSourceRouteRules().Keys).ToList();
             foreach (var logicTableName in logicTableNames)
             {
                 var actualTables = dbActualTables.Where(o=>o.LogicTableName==logicTableName).ToList();
                 var actualTableEntity = actualTables.FirstOrDefault();
                 var columnMetadatas = await GetColumnSchemaAsync(actualTableEntity);
                 var tableMetadata = new TableMetadata(logicTableName,columnMetadatas);
                 foreach (var actualTable in actualTables)
                 {
                     tableMetadata.AddActualTableWithDataSource(actualTable.TableName,actualTable.DataSource);
                 }
                 await _tableMetadataInitializer.InitializeAsync(tableMetadata);
             }
        }
    }

    private async Task<Dictionary<string, ColumnMetadata>> GetColumnSchemaAsync(ActualTableEntity? actualTableEntity)
    {
        var result = new Dictionary<string,ColumnMetadata>();
        if (actualTableEntity == null)
        {
            return result;
        }
        
        var connectionString = _virtualDataSource.GetConnectionString(actualTableEntity.DataSource);
        var emptyResultSql = GenerateEmptyResultSql(actualTableEntity.LogicTableName);
        var providerFactory = _dbProviderFactory.Create();
        using (var dbConnection = providerFactory.CreateConnection()!)
        {
            dbConnection.ConnectionString = connectionString;
            await dbConnection.OpenAsync();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = emptyResultSql;
               
                using (var dbDataReader = await command.ExecuteReaderAsync(behavior: CommandBehavior.KeyInfo))
                {
                    using (var schemaTable = await dbDataReader.GetSchemaTableAsync())
                    {
                        var isCaseSensitive = schemaTable.CaseSensitive;
                        var dbColumns = dbDataReader.GetColumnSchema().ToList();
                        foreach (var dbColumn in dbColumns)
                        {
                            if (dbColumn.ColumnOrdinal.HasValue)
                            {
                                result.Add(dbColumn.ColumnName,new ColumnMetadata(dbColumn.ColumnName, dbColumn.ColumnOrdinal.Value, dbColumn.DataTypeName??string.Empty, dbColumn.IsKey.GetValueOrDefault(), dbColumn.IsAutoIncrement.GetValueOrDefault(), isCaseSensitive));
                            }
                        }
                    }
                }
            }
        }

        return result;
    }
    
    private  string GenerateEmptyResultSql(string table)
    {
        var databaseType = _appConfiguration.GetDatabaseType();
        // TODO consider add a getDialectDelimeter() interface in parse module
        string delimiterLeft;
        string delimiterRight;
        if (DatabaseTypeEnum.MySql.Equals(databaseType) || DatabaseTypeEnum.MariaDB.Equals(databaseType))
        {
            delimiterLeft = "`";
            delimiterRight = "`";
        }
        else if (DatabaseTypeEnum.SqlServer.Equals(databaseType))
        {
            delimiterLeft = "[";
            delimiterRight = "]";
        }
        else
        {
            delimiterLeft = "";
            delimiterRight = "";
        }
        return $"SELECT  * FROM {delimiterLeft}{table}{delimiterRight} WHERE 1!=1";
    }
}