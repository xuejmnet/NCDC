using NCDC.CommandParser.Segment.Generic;
using NCDC.CommandParser.Value.Identifier;

namespace NCDC.CommandParser.Segment.DDL.Routine;

public sealed class FunctionNameSegment:ISqlSegment,IOwnerAvailable
{
    public FunctionNameSegment(int startIndex, int stopIndex,IdentifierValue identifierValue)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        IdentifierValue = identifierValue;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public IdentifierValue IdentifierValue { get; }
    public OwnerSegment? Owner { get; set; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(IdentifierValue)}: {IdentifierValue}, {nameof(Owner)}: {Owner}";
    }
}