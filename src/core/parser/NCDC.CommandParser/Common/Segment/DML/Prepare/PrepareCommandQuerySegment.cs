using NCDC.CommandParser.Common.Command.DML;

namespace NCDC.CommandParser.Common.Segment.DML.Prepare;

public sealed class PrepareCommandQuerySegment:ISqlSegment
{
    public PrepareCommandQuerySegment(int startIndex, int stopIndex)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public SelectCommand? Select { get; set; }
    public InsertCommand? Insert { get; set; }
    public UpdateCommand? Update { get; set; }
    public DeleteCommand? Delete { get; set; }
}