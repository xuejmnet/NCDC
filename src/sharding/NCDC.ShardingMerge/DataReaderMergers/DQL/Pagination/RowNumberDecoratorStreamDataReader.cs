using NCDC.ShardingMerge.DataReaders.Decorator;
using NCDC.ShardingParser.Segment.Select.Pagination;
using NCDC.StreamDataReaders;

namespace NCDC.ShardingMerge.DataReaderMergers.DQL.Pagination
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 22:12:36
* @Email: 326308290@qq.com
*/
    public sealed class RowNumberDecoratorStreamDataReader : DecoratorStreamDataReader
    {
        private readonly PaginationContext pagination;

        private readonly bool skipAll;

        private long rowNumber;

        public RowNumberDecoratorStreamDataReader(IStreamDataReader streamDataReader, PaginationContext pagination) : base(streamDataReader)
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
            if (skipAll) {
                return false;
            }
            if (pagination.GetActualRowCount()==null) {
                return StreamDataReader.Read();
            }
            return rowNumber++ < pagination.GetActualRowCount() && StreamDataReader.Read();

        }
    }
}