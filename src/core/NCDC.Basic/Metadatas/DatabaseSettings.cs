using NCDC.Enums;

namespace NCDC.Basic.Metadatas;

public class DatabaseSettings:IDatabaseSettings
{
    private readonly string _name;
    private readonly DatabaseTypeEnum _databaseType;

    public DatabaseSettings(string name,DatabaseTypeEnum databaseType)
    {
        _name = name;
        _databaseType = databaseType;
    }
    public string GetDatabaseName()
    {
        return _name;
    }

    public DatabaseTypeEnum GetDatabaseType()
    {
        return _databaseType;
    }
}