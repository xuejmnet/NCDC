namespace NCDC.CommandParser.Common.Segment.DAL;

public sealed class VariableAssignSegment:ISqlSegment
{
    public int StartIndex { get; set; }
    public int StopIndex { get; set; }
    public VariableSegment? Variable;
    public string? AssignValue { get; set; }

    public override string ToString()
    {
        return $"{nameof(Variable)}: {Variable}, {nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(AssignValue)}: {AssignValue}";
    }
}