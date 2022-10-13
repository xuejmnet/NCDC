namespace NCDC.CommandParser.Common.Command.TCL;

public abstract class XACommand: AbstractSqlCommand, ITCLCommand
{
    public string? Op { get; set; }
    public string? XId { get; set; }
}