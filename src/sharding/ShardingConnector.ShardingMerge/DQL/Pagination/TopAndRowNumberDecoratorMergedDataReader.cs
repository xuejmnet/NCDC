using System;
using ShardingConnector.Merge.Reader;
using ShardingConnector.Merge.Reader.Decorator;
using ShardingConnector.ParserBinder.Segment.Select.Pagination;

namespace ShardingConnector.ShardingMerge.DQL.Pagination
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 22:14:58
* @Email: 326308290@qq.com
*/
    public sealed class TopAndRowNumberDecoratorMergedDataReader : DecoratorMergedDataReader
    {
        private readonly PaginationContext pagination;

        private readonly bool skipAll;

        private long rowNumber;

        public TopAndRowNumberDecoratorMergedDataReader(IMergedDataReader mergedDataReader, PaginationContext pagination) : base(mergedDataReader)
        {
            this.pagination = pagination;
            skipAll = SkipOffset();
        }

        private bool SkipOffset()
        {
            long end = pagination.GetActualOffset();
            for (int i = 0; i < end; i++)
            {
                if (!MergedDataReader.Read())
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
                return MergedDataReader.Read();
            }

            return rowNumber++ <= pagination.GetActualRowCount() && MergedDataReader.Read();
        }
    }
}