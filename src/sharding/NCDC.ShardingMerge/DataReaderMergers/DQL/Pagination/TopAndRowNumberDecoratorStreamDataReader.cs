using NCDC.ShardingMerge.DataReaders.Decorator;
using NCDC.ShardingParser.Segment.Select.Pagination;
using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.DataReaderMergers.DQL.Pagination
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 22:14:58
* @Email: 326308290@qq.com
*/
    public sealed class TopAndRowNumberDecoratorStreamDataReader : DecoratorStreamDataReader
    {
        private readonly PaginationContext pagination;

        private readonly bool skipAll;

        private long rowNumber;

        public TopAndRowNumberDecoratorStreamDataReader(IStreamDataReader streamDataReader, PaginationContext pagination) : base(streamDataReader)
        {
            this.pagination = pagination;
            skipAll = SkipOffset();
        }

        private bool SkipOffset()
        {
            long end = pagination.GetActualOffset();
            for (int i = 0; i < end; i++)
            {
                if (!StreamDataReader.Read())
                {
                    return true;
                }
            }

            rowNumber = end + 1;
            return false;
        }

        public override bool Read()
        {
            if (skipAll)
            {
                return false;
            }

            if (pagination.GetActualRowCount() == null)
            {
                return StreamDataReader.Read();
            }

            return rowNumber++ <= pagination.GetActualRowCount() && StreamDataReader.Read();
        }
    }
}