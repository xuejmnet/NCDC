namespace OpenConnector.CommandParser.Command.DML
{
    public sealed class EmptyCommand:AbstractSqlCommand
    {
        public override int GetParameterCount()
        {
            return 0;
        }
    }
}