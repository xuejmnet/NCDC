using OpenConnector.CommandParserBinder.MetaData;

namespace OpenConnector.Sharding.Abstractions;

public interface ITableMetadataAutoBindInitializer
{
    void Initialize(TableMetadata tableMetadata);
}