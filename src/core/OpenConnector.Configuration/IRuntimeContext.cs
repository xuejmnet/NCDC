using OpenConnector.Configuration.Metadatas;

namespace OpenConnector.Configuration;

public interface IRuntimeContext
{
    ILogicDatabase? GetDatabase(string database);
    ILogicDbServer GetLogicDbServer();
    IMergeComparer GetMergeComparer();
}