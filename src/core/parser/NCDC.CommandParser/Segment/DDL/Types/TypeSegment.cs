using NCDC.CommandParser.Segment.Generic;
using NCDC.CommandParser.Value.Identifier;

namespace NCDC.CommandParser.Segment.DDL.Types;

public sealed class TypeSegment:ISqlSegment,IOwnerAvailable
{
    public TypeSegment(int startIndex, int stopIndex,IdentifierValue identifierValue)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        IdentifierValue = identifierValue;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public IdentifierValue IdentifierValue { get; }
    public OwnerSegment? Owner { get; set; }
}