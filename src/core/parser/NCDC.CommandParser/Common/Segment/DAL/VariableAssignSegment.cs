namespace NCDC.CommandParser.Common.Segment.DAL;

public sealed class VariableAssignSegment:ISqlSegment
{
    public int StartIndex { get; set; }
    public int StopIndex { get; set; }
    public VariableSegment? Variable { get; set; }
    public string? AssignValue { get; set; }
}