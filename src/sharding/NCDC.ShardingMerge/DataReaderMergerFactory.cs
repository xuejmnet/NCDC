using NCDC.CommandParser.Abstractions;
using NCDC.ShardingMerge.Abstractions;
using NCDC.ShardingParser.Command;

namespace NCDC.ShardingMerge;

public sealed class DataReaderMergerFactory:IDataReaderMergerFactory
{
    public IDataReaderMerger Create(ISqlCommandContext<ISqlCommand> sqlCommandContext)
    {
        throw new NotImplementedException();
    }
}