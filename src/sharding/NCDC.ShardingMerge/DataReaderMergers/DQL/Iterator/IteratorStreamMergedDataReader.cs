using NCDC.Extensions;
using NCDC.ShardingMerge.DataReaders.Stream;
using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.DataReaderMergers.DQL.Iterator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/6 8:07:35
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class IteratorStreamMergedDataReader:StreamMergedDataReader
    {
        private readonly List<IStreamDataReader> _streamDataReaders;
        private readonly IEnumerator<IStreamDataReader> _streamDataReaderEnumerator;

        public IteratorStreamMergedDataReader(List<IStreamDataReader> streamDataReaders)
        {
            _streamDataReaders = streamDataReaders;
            _streamDataReaderEnumerator = streamDataReaders.GetEnumerator();
            SetCurrentStreamDataReader( this._streamDataReaderEnumerator.Next());
        }


        public override bool Read()
        {
            if (GetCurrentStreamDataReader().Read())
            {
                return true;
            }
            if (!_streamDataReaderEnumerator.MoveNext())
            {
                return false;
            }
            SetCurrentStreamDataReader(_streamDataReaderEnumerator.Current);
            var hasNext = GetCurrentStreamDataReader().Read();
            if (hasNext)
            {
                return true;
            }
            while (!hasNext && _streamDataReaderEnumerator.MoveNext())
            {
                SetCurrentStreamDataReader(_streamDataReaderEnumerator.Current);
                hasNext = GetCurrentStreamDataReader().Read();
            }
            return hasNext;
        }

        public override void Dispose()
        {
            _streamDataReaders?.ForEach(o=>o.Dispose());
        }
    }
}
