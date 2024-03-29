using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.ShardingParser.Command;

namespace NCDC.ShardingMerge.Abstractions;

public interface IDataReaderMergerFactory
{
    IDataReaderMerger Create(ISqlCommandContext<ISqlCommand> sqlCommandContext);
}