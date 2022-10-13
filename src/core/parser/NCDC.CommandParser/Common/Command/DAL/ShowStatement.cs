namespace NCDC.CommandParser.Common.Command.DAL;

public abstract class ShowStatement:AbstractSqlCommand,IDALCommand
{
    public string Name { get; }

    public ShowStatement(string name)
    {
        Name = name;
    }
}