using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.ShardingMerge.Abstractions;
using NCDC.ShardingMerge.DataReaders.Transparent;
using NCDC.ShardingParser.Command;
using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.DataReaderMergers
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/5 10:15:26
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TransparentDataReaderMerger:IDataReaderMerger
    {
        public IStreamDataReader Merge(List<IStreamDataReader> streamDataReaders, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return new TransparentMergedDataReader(streamDataReaders[0]);
        }
    }
}
