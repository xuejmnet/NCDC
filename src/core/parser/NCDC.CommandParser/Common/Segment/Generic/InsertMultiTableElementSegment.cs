using NCDC.CommandParser.Common.Command.DML;

namespace NCDC.CommandParser.Common.Segment.Generic;

public sealed class InsertMultiTableElementSegment:ISqlSegment
{
    public InsertMultiTableElementSegment(int startIndex, int stopIndex)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public ICollection<InsertCommand> InsertCommands = new LinkedList<InsertCommand>();
}