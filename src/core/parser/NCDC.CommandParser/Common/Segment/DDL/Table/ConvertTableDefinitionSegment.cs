using NCDC.CommandParser.Common.Segment.DDL.Charset;
using NCDC.CommandParser.Common.Segment.DML.Expr.Simple;

namespace NCDC.CommandParser.Common.Segment.DDL.Table;

public sealed class ConvertTableDefinitionSegment:IAlterDefinitionSegment
{
    public ConvertTableDefinitionSegment(int startIndex, int stopIndex,CharsetNameSegment charsetName)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        CharsetName = charsetName;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public CharsetNameSegment CharsetName { get; }
    public ISimpleExpressionSegment? CollateValue { get; set; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(CharsetName)}: {CharsetName}, {nameof(CollateValue)}: {CollateValue}";
    }
}