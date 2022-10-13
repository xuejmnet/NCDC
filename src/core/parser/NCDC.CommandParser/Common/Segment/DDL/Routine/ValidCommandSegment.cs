using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DDL;
using NCDC.CommandParser.Common.Command.DML;

namespace NCDC.CommandParser.Common.Segment.DDL.Routine;

public sealed class ValidCommandSegment:ISqlSegment
{
    public ValidCommandSegment(int startIndex, int stopIndex)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public ISqlCommand? SqlCommand { get; set; }

    public CreateTableCommand? GetCreateTable()
    {
        return SqlCommand as CreateTableCommand;
    }

    public AlterTableCommand? GetAlterTable()
    {
        return SqlCommand as AlterTableCommand;
    }
    public DropTableCommand? GetDropTable()
    {
        return SqlCommand as DropTableCommand;
    }
    public TruncateCommand? GetTruncate()
    {
        return SqlCommand as TruncateCommand;
    }

    public InsertCommand? GetInsert()
    {
        return SqlCommand as InsertCommand;
    }
    public InsertCommand? GetReplace()
    {
        return SqlCommand as InsertCommand;
    }
    public UpdateCommand? GetUpdate()
    {
        return SqlCommand as UpdateCommand;
    }
    public DeleteCommand? GetDelete()
    {
        return SqlCommand as DeleteCommand;
    }
    public SelectCommand? GetSelect()
    {
        return SqlCommand as SelectCommand;
    }

    public override string ToString()
    {
        return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(SqlCommand)}: {SqlCommand}";
    }
}