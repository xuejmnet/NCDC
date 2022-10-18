using NCDC.ShardingAdoNet;
using NCDC.ShardingParser.Command.DML;

namespace NCDC.ShardingParser.Segment.Insert.Values;

public sealed class InsertSelectContext
{
    public  int ParameterCount { get; }
    public  ParameterContext ParameterContext{ get; }
    public  SelectCommandContext SelectCommandContext{ get; }

    public InsertSelectContext(SelectCommandContext selectCommandContext,ParameterContext parameterContext)
    {
        ParameterCount = selectCommandContext.GetSqlCommand().ParameterCount;
        SelectCommandContext = selectCommandContext;
        ParameterContext = parameterContext;
    }

}