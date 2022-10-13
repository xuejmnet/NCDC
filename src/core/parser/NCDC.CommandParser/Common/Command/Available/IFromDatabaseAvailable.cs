using NCDC.CommandParser.Common.Segment.Generic;

namespace NCDC.CommandParser.Common.Command.Available;

public interface IFromDatabaseAvailable
{
    DatabaseSegment? Database { get; }
}