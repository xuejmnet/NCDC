using NCDC.CommandParser.Common.Value.Identifier;

namespace NCDC.CommandParser.Common.Segment.DDL.Constraint;

public sealed class ConstraintSegment:ISqlSegment
{
    public int StartIndex { get; }
    public int StopIndex { get; }
    public IdentifierValue IdentifierValue { get; }

    public ConstraintSegment(int startIndex, int stopIndex,IdentifierValue identifierValue)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        IdentifierValue = identifierValue;
    }

}