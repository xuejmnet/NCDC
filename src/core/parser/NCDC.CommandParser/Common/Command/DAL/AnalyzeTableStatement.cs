using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DAL;

public abstract class AnalyzeTableStatement:AbstractSqlCommand,IDALCommand
{
    public ICollection<SimpleTableSegment> Tables = new List<SimpleTableSegment>();
}