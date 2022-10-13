namespace NCDC.CommandParser.Common.Command.DML
{
    public sealed class EmptyCommand:AbstractSqlCommand
    {
        public new int ParameterCount
        {
            get => 0;
            set { }
        }
    }
}