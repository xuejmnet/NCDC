using NCDC.Basic.Executors;
using NCDC.Basic.Metadatas;
using NCDC.Basic.TableMetadataManagers;

namespace NCDC.Basic.Contexts;

public interface IRuntimeContext
{
    string DatabaseName { get; }
    ILogicDatabase GetDatabase();
    ITableMetadataManager GetTableMetadataManager();
    IShardingExecutionContextFactory GetShardingExecutionContextFactory();
}