namespace NCDC.CommandParser.Segment.DDL.Constraint.Alter;

public sealed class ModifyConstraintDefinitionSegment:IAlterDefinitionSegment
{
    public int StartIndex { get; }
    public int StopIndex { get; }
    public ConstraintSegment ConstraintName { get; }
    public ModifyConstraintDefinitionSegment(int startIndex, int stopIndex,ConstraintSegment constraintName)
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