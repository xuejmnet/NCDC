namespace NCDC.CommandParser.Segment.DDL.Constraint.Alter;

public sealed class ValidateConstraintDefinitionSegment:IAlterDefinitionSegment
{
    public int StartIndex { get; }
    public int StopIndex { get; }
    public ConstraintSegment ConstraintName { get; }
    public ValidateConstraintDefinitionSegment(int startIndex, int stopIndex,ConstraintSegment constraintName)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        ConstraintName = constraintName;
    }

}