namespace NCDC.CommandParser.Common.Segment.DDL.Constraint.Alter;

public sealed class AddConstraintDefinitionSegment:IAlterDefinitionSegment
{
    public int StartIndex { get; }
    public int StopIndex { get; }
    public ConstraintDefinitionSegment ConstraintDefinition { get; }

    public AddConstraintDefinitionSegment(int startIndex, int stopIndex,ConstraintDefinitionSegment constraintDefinition)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        ConstraintDefinition = constraintDefinition;
    }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(ConstraintDefinition)}: {ConstraintDefinition}";
    }
}