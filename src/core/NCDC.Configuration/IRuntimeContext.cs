using NCDC.Configuration.Metadatas;

namespace NCDC.Configuration;

public interface IRuntimeContext
{
    ILogicDatabase? GetDatabase(string database);
    ILogicDbServer GetLogicDbServer();
    IMergeComparer GetMergeComparer();
}