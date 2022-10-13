namespace NCDC.CommandParser.Common.Segment.Generic;

public sealed class GrantLevelSegment:ISqlSegment
{
    public GrantLevelSegment(int startIndex, int stopIndex,string dbName,string tableName)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        DbName = dbName;
        TableName = tableName;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public string DbName { get; }
    public string TableName { get; }
}