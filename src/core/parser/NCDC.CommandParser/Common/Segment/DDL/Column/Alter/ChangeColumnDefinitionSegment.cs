using NCDC.CommandParser.Common.Segment.DDL.Column.Position;
using NCDC.CommandParser.Common.Segment.DML.Column;

namespace NCDC.CommandParser.Common.Segment.DDL.Column.Alter;

public sealed class ChangeColumnDefinitionSegment:IAlterDefinitionSegment
{
    public ChangeColumnDefinitionSegment(int startIndex, int stopIndex,ColumnDefinitionSegment columnDefinition)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        ColumnDefinition = columnDefinition;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public ColumnDefinitionSegment ColumnDefinition { get; }
    public ColumnSegment? PreviousColumn { get; set; }
    public ColumnPositionSegment? ColumnPosition { get; set; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(ColumnDefinition)}: {ColumnDefinition}, {nameof(PreviousColumn)}: {PreviousColumn}, {nameof(ColumnPosition)}: {ColumnPosition}";
    }
}