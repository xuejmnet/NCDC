namespace NCDC.CommandParser.Segment.DDL.Routine;

public sealed class RoutineBodySegment:ISqlSegment
{
    public RoutineBodySegment(int startIndex, int stopIndex)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        ValidCommands= new LinkedList<ValidCommandSegment>();
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public ICollection<ValidCommandSegment> ValidCommands { get; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(ValidCommands)}: {ValidCommands}";
    }
}