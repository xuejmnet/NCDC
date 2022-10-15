namespace NCDC.CommandParser.Common.Command.TCL;

public abstract class XACommand: AbstractSqlCommand, ITCLCommand
{
    protected XACommand(string op)
    {
        Op = op;
    }

    public string Op { get; }
    public string? XId { get; set; }
}