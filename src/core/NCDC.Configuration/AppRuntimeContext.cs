using NCDC.Configuration.Metadatas;

namespace NCDC.Configuration;

public class AppRuntimeContext:IRuntimeContext
{
    public static IRuntimeContext Instance { get; } = new AppRuntimeContext();
    public ILogicDatabase? GetDatabase(string database)
    {
        throw new NotImplementedException();
    }

    public ILogicDbServer GetLogicDbServer()
    {
        throw new NotImplementedException();
    }

    public IMergeComparer GetMergeComparer()
    {
        throw new NotImplementedException();
    }
}