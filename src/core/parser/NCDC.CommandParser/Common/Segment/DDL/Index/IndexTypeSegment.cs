using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Value.Identifier;

namespace NCDC.CommandParser.Common.Segment.DDL.Index;

public sealed class IndexTypeSegment:ISqlSegment,IOwnerAvailable
{
    public IndexTypeSegment(int startIndex, int stopIndex,IdentifierValue identifierValue)
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