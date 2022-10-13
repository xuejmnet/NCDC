using NCDC.CommandParser.Common.Segment.DAL;

namespace NCDC.CommandParser.Common.Command.DAL;

public abstract class SetCommand:AbstractSqlCommand,IDALCommand
{
    public ICollection<VariableAssignSegment> VariableAssigns = new LinkedList<VariableAssignSegment>();
}