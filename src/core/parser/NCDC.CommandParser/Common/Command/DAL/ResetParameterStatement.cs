namespace NCDC.CommandParser.Common.Command.DAL;

public abstract class ResetParameterStatement:AbstractSqlCommand,IDALCommand
{
    public string ConfigurationParameter { get; }

    public ResetParameterStatement(string configurationParameter)
    {
        ConfigurationParameter = configurationParameter;
    }
}