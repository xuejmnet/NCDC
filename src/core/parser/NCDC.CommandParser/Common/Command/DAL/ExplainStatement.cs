using NCDC.CommandParser.Abstractions;

namespace NCDC.CommandParser.Common.Command.DAL;

public sealed class ExplainStatement:AbstractSqlCommand,IDALCommand
{
    public ISqlCommand? SqlCommand { get; set; }
}