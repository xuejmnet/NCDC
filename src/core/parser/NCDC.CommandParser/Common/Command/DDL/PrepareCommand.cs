using NCDC.CommandParser.Common.Command.DML;

namespace NCDC.CommandParser.Common.Command.DDL;

public abstract class PrepareCommand:AbstractSqlCommand,IDDLCommand
{
    public SelectCommand? Select { get; set; }
    public InsertCommand? Insert { get; set; }
    public UpdateCommand? Update { get; set; }
    public DeleteCommand? Delete { get; set; }
}