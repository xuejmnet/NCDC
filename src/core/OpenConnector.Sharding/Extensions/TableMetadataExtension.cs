using OpenConnector.CommandParserBinder.MetaData;

namespace OpenConnector.Sharding.Extensions;

public static class TableMetadataExtension
{

    public static bool IsMainShardingTableColumn(this TableMetadata metadata, string columnName)
    {
        return metadata.ShardingTableColumn == columnName;
    }
    public static bool IsMainShardingDataSourceColumn(this TableMetadata metadata, string columnName)
    {
        return metadata.ShardingDataSourceColumn == columnName;
    }
    
}