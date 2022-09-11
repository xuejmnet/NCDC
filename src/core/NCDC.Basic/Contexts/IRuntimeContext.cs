using NCDC.Basic.Metadatas;

namespace NCDC.Basic.Contexts;

public interface IRuntimeContext
{
    ILogicDatabase? GetDatabase(string database);
    ILogicDbServer GetLogicDbServer();
}