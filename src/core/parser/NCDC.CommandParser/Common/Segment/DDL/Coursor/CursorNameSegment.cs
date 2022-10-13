using NCDC.CommandParser.Common.Value.Identifier;

namespace NCDC.CommandParser.Common.Segment.DDL.Coursor;

public sealed class CursorNameSegment:ISqlSegment
{
    public int StartIndex { get; }
    public int StopIndex { get; }
    public IdentifierValue IdentifierValue { get; }
    public CursorNameSegment(int startIndex, int stopIndex,IdentifierValue identifierValue)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        IdentifierValue = identifierValue;
    }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(IdentifierValue)}: {IdentifierValue}";
    }
}