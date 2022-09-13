using NCDC.Enums;

namespace NCDC.Basic.Metadatas;

public interface IDatabaseSettings
{
    string GetDatabaseName();
    
    DatabaseTypeEnum GetDatabaseType();
}