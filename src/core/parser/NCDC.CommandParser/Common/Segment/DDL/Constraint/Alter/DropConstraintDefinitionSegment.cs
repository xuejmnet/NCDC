namespace NCDC.CommandParser.Common.Segment.DDL.Constraint.Alter;

public sealed class DropConstraintDefinitionSegment:IAlterDefinitionSegment
{
    public int StartIndex { get; }
    public int StopIndex { get; }
    public ConstraintSegment ConstraintName { get; }
    public DropConstraintDefinitionSegment(int startIndex, int stopIndex,ConstraintSegment constraintName)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        ConstraintName = constraintName;
    }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(ConstraintName)}: {ConstraintName}";
    }
}