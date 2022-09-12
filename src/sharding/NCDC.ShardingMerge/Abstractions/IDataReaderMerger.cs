using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.Abstractions;

public interface IDataReaderMerger
{
    IStreamDataReader Merge(List<IStreamDataReader> streamDataReaders, ISqlCommandContext<ISqlCommand> sqlCommandContext);
}