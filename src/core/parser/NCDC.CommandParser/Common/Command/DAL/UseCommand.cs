namespace NCDC.CommandParser.Common.Command.DAL;

public abstract class UseCommand:AbstractSqlCommand,IDALCommand
{
    public string Schema { get; }

    public UseCommand(string schema)
    {
        Schema = schema;
    }
}