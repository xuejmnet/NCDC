using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCDC.Basic.TableMetadataManagers;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.Exceptions;
using NCDC.Extensions;
using NCDC.ProxyServer.Configurations;

namespace NCDC.EntityFrameworkCore.Configurations;

public sealed class DbTableMetadataBuilder:ITableMetadataBuilder
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITableSchemaBuilder _tableSchemaBuilder;

    public DbTableMetadataBuilder(IServiceProvider serviceProvider,ITableSchemaBuilder tableSchemaBuilder)
    {
        _serviceProvider = serviceProvider;
        _tableSchemaBuilder = tableSchemaBuilder;
    }
    public  async Task<IReadOnlyList<TableMetadata>> BuildAsync(string databaseName)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var logicTables = await dbContext.Set<LogicTableEntity>().Where(o => o.Database == databaseName).ToListAsync();
            var actualTables = await dbContext.Set<ActualTableEntity>().Where(o => o.Database == databaseName).ToListAsync();
            var tableSchemaArgs = actualTables.GroupBy(o=>o.LogicTableName).Select(o=>new {LogicTableName=o.Key,o.First().TableName})
                .ToDictionary(o=>o.LogicTableName,o=>o.TableName);
            var tableSchemaMap =await _tableSchemaBuilder.BuildAsync(tableSchemaArgs);
            return GetTableMetadatas(logicTables, actualTables, tableSchemaMap).ToImmutableList();
        }
    }

    private IEnumerable<TableMetadata> GetTableMetadatas(List<LogicTableEntity> logicTables,
        List<ActualTableEntity> actualTables,IDictionary<string /*logic table name*/, List<ColumnMetadata>> tableSchemaMap)
    {
        foreach (var logicTable in logicTables)
        {
            var logicTableName = logicTable.LogicName;
            if (!tableSchemaMap.TryGetValue(logicTableName, out var columnMetadatas))
            {
                throw new ShardingInvalidOperationException($"cant found table schema:[{logicTableName}]");
            }

            var columnMaps = columnMetadatas.ToDictionary(o=>o.Name,o=>o);
            var tableMetadata = new TableMetadata(logicTableName,columnMaps);
            if (logicTable.ShardingDataSourceColumns.NotNullOrWhiteSpace())
            {
                var shardingColumns = logicTable.ShardingDataSourceColumns!.Split(",");
                var unknownColumns = shardingColumns.Where(o => !columnMaps.ContainsKey(o)).ToList();
                if (unknownColumns.IsNotEmpty())
                {
                    throw new ShardingInvalidOperationException(
                        $"unknown sharding datasource columns:[{string.Join(",", unknownColumns)}]");
                }
                tableMetadata.SetShardingDataSourceColumn(shardingColumns[0]);
                for (int i = 1; i < shardingColumns.Length; i++)
                {
                    tableMetadata.AddExtraSharingDataSourceColumn(shardingColumns[i]);
                }
            }
            if (logicTable.ShardingTableColumns.NotNullOrWhiteSpace())
            {
                var shardingColumns = logicTable.ShardingTableColumns!.Split(",");
                var unknownColumns = shardingColumns.Where(o => !columnMaps.ContainsKey(o)).ToList();
                if (unknownColumns.IsNotEmpty())
                {
                    throw new ShardingInvalidOperationException(
                        $"unknown sharding table columns:[{string.Join(",", unknownColumns)}]");
                }
                tableMetadata.SetShardingTableColumn(shardingColumns[0]);
                for (int i = 1; i < shardingColumns.Length; i++)
                {
                    tableMetadata.AddExtraSharingTableColumn(shardingColumns[i]);
                }
            }
            foreach (var actualTable in actualTables.Where(o=>o.TableName==logicTableName))
            {
                tableMetadata.AddActualTableWithDataSource(actualTable.DataSource,actualTable.TableName);
            }
            tableMetadata.CheckMetadata();
            yield return tableMetadata;
        }
    }
}