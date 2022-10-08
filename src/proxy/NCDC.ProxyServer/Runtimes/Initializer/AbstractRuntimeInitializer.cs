using System.Data;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using NCDC.Basic.Configurations;
using NCDC.Basic.TableMetadataManagers;
using NCDC.Enums;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Databases;
using NCDC.ProxyServer.Options;
using NCDC.ProxyServer.ServiceProviders;

namespace NCDC.ProxyServer.Runtimes.Initializer;

public abstract class AbstractRuntimeInitializer : IRuntimeInitializer
{
    private readonly IShardingProvider _shardingProvider;
    private readonly ShardingConfiguration _shardingConfiguration;
    private readonly ITableMetadataManager _tableMetadataManager;
    private readonly IVirtualDataSource _virtualDataSource;
    private readonly IRouteInitConfigOption _routeInitConfigOption;
    private readonly ITableMetadataInitializer _tableMetadataInitializer;

    public AbstractRuntimeInitializer(IShardingProvider shardingProvider, ShardingConfiguration shardingConfiguration,
        ITableMetadataManager tableMetadataManager,
        IVirtualDataSource virtualDataSource, IRouteInitConfigOption routeInitConfigOption,
        ITableMetadataInitializer tableMetadataInitializer)
    {
        _shardingProvider = shardingProvider;
        _shardingConfiguration = shardingConfiguration;
        _tableMetadataManager = tableMetadataManager;
        _virtualDataSource = virtualDataSource;
        _routeInitConfigOption = routeInitConfigOption;
        _tableMetadataInitializer = tableMetadataInitializer;
    }

    protected virtual Task LoadTableSchemaAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logicTableName"></param>
    /// <returns>key:data source,value:actual table names in data source</returns>
    protected abstract List<ActualTableNode> GetActualTables(string logicTableName);

    public async Task InitializeAsync()
    {
        await LoadTableSchemaAsync();
        
        var logicTableNames = _routeInitConfigOption.GetTableRouteRules().Keys
            .Union(_routeInitConfigOption.GetDataSourceRouteRules().Keys).ToList();
        foreach (var logicTableName in logicTableNames)
        {
            var actualTables = GetActualTables(logicTableName);
          
            var actualTableEntity = actualTables.FirstOrDefault();
            var columnMetadatas = await GetColumnSchemaAsync(actualTableEntity);
            var tableMetadata = new TableMetadata(logicTableName, columnMetadatas);
            foreach (var actualTable in actualTables)
            {
                tableMetadata.AddActualTableWithDataSource(actualTable.DataSource, actualTable.TableName);
            }

            _tableMetadataManager.AddTableMetadata(tableMetadata);

            await _tableMetadataInitializer.InitializeAsync(tableMetadata);
        }
    }

    private async Task<Dictionary<string, ColumnMetadata>> GetColumnSchemaAsync(ActualTableNode? actualTableNode)
    {
        var result = new Dictionary<string, ColumnMetadata>();
        if (actualTableNode == null)
        {
            return result;
        }

        var dataSource = _virtualDataSource.GetDataSource(actualTableNode.DataSource);
       
        var emptyResultSql = GenerateEmptyResultSql(actualTableNode.TableName);
        using (var dbConnection = dataSource.CreateDbConnection(true))
        {
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
                                result.Add(dbColumn.ColumnName,
                                    new ColumnMetadata(dbColumn.ColumnName, dbColumn.ColumnOrdinal.Value,
                                        dbColumn.DataTypeName ?? string.Empty, dbColumn.IsKey.GetValueOrDefault(),
                                        dbColumn.IsAutoIncrement.GetValueOrDefault(), isCaseSensitive));
                            }
                        }
                    }
                }
            }
        }

        return result;
    }

    private string GenerateEmptyResultSql(string table)
    {
        var databaseType = _shardingConfiguration.DatabaseType;
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