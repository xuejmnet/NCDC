using System.Data;
using System.Data.Common;
using NCDC.Basic.Configurations;
using NCDC.Basic.Metadatas;
using NCDC.Enums;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Databases;

namespace NCDC.ProxyServer.Runtimes.Builder;

public abstract class AbstractAppRuntimeBuilder : IAppRuntimeBuilder
{
    private readonly IServiceProvider _appServiceProvider;
    private readonly IAppConfiguration _appConfiguration;

    public AbstractAppRuntimeBuilder(IServiceProvider appServiceProvider, IAppConfiguration appConfiguration)
    {
        _appServiceProvider = appServiceProvider;
        _appConfiguration = appConfiguration;
    }

    protected abstract Task<bool> LoadConfigurationAsync(string database);
    protected abstract void ConfigureOption(ShardingConfiguration shardingConfiguration);
    protected abstract IEnumerable<DataSourceNode> GetDataSources();
    protected abstract IEnumerable<LogicTableNode> GetLogicTables();

    public async Task<IRuntimeContext?> BuildAsync(string database)
    {
        var doBuild = await LoadConfigurationAsync(database);
        if (!doBuild)
        {
            return null;
        }

        var builder = RuntimeApplicationBuilder.CreateBuilder(_appConfiguration.DatabaseType, database);
        ConfigureOption(builder.ConfigOption);
        foreach (var dataSource in GetDataSources())
        {
            if (dataSource.IsDefault)
            {
                builder.ConfigOption.AddDefaultDataSource(dataSource.Name,
                    dataSource.ConnectionString);
            }
            else
            {
                builder.ConfigOption.AddExtraDataSource(dataSource.Name,
                    dataSource.ConnectionString);
            }
        }

        foreach (var logicTable in GetLogicTables())
        {
            if (logicTable.ShardingDataSourceRule != null)
            {
                builder.RouteConfigOption.AddDataSourceRouteRule(logicTable.Name,
                    logicTable.ShardingDataSourceRule);
            }

            if (logicTable.ShardingTableRule != null)
            {
                builder.RouteConfigOption.AddTableRouteRule(logicTable.Name, logicTable.ShardingTableRule);
            }
        }

        var runtimeContext = builder.Build(_appServiceProvider);
        await InitialAsync(runtimeContext);
        return runtimeContext;
        // }
    }

    protected abstract List<ActualTableNode> GetActualTables(string logicTableName);

    private async Task InitialAsync(IRuntimeContext runtimeContext)
    {
        var virtualDataSource = runtimeContext.GetVirtualDataSource();
        var routeInitConfigOption = runtimeContext.GetRouteInitConfigOption();
        var tableMetadataManager = runtimeContext.GetTableMetadataManager();
        var tableMetadataInitializer = runtimeContext.GetTableMetadataInitializer();
        var logicTableNames = routeInitConfigOption.GetTableRouteRules().Keys
            .Union(routeInitConfigOption.GetDataSourceRouteRules().Keys).Distinct().ToList();
        foreach (var logicTableName in logicTableNames)
        {
            var actualTables = GetActualTables(logicTableName);

            var actualTableEntity = actualTables.FirstOrDefault();
            var columnSchema = await GetColumnSchemaAsync(virtualDataSource, actualTableEntity);
            var tableMetadata = new TableMetadata(logicTableName, columnSchema);
            foreach (var actualTable in actualTables)
            {
                tableMetadata.AddActualTableWithDataSource(actualTable.DataSource, actualTable.TableName);
            }

            tableMetadataManager.AddTableMetadata(tableMetadata);

            if (!await tableMetadataInitializer.InitializeAsync(tableMetadata))
            {
                tableMetadataManager.RemoveTableMetadata(tableMetadata.LogicTableName);
            }
        }
    }

    private async Task<Dictionary<string, ColumnMetadata>> GetColumnSchemaAsync(IVirtualDataSource virtualDataSource,
        ActualTableNode? actualTableNode)
    {
        var result = new Dictionary<string, ColumnMetadata>();
        if (actualTableNode == null)
        {
            return result;
        }

        var dataSource = virtualDataSource.GetDataSource(actualTableNode.DataSource);

        var emptyResultSql = GenerateEmptyResultSql(actualTableNode.TableName);
        using (var dbConnection = await dataSource.CreateDbConnectionAsync(true))
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
        var databaseType = _appConfiguration.DatabaseType;
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