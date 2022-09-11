using NCDC.CommandParserBinder.MetaData;

namespace NCDC.Sharding.Abstractions;

public interface ITableMetadataAutoBindInitializer
{
    void Initialize(TableMetadata tableMetadata);
}