using NCDC.CommandParser.Command.DML;
using NCDC.CommandParser.Constant;

namespace NCDC.CommandParser.Segment.DML.Combine;

public sealed class CombineSegment:ISqlSegment
{
    public CombineSegment(int startIndex, int stopIndex,CombineTypeEnum combineType,SelectCommand selectCommand)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        CombineType = combineType;
        SelectCommand = selectCommand;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public CombineTypeEnum CombineType { get; }
    public SelectCommand SelectCommand { get; }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(CombineType)}: {CombineType}, {nameof(SelectCommand)}: {SelectCommand}";
    }
}