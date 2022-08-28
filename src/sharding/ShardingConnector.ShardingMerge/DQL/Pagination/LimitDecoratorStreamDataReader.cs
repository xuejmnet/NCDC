using System;
using ShardingConnector.Merge.Reader;
using ShardingConnector.Merge.Reader.Decorator;
using ShardingConnector.CommandParserBinder.Segment.Select.Pagination;

namespace ShardingConnector.ShardingMerge.DQL.Pagination
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 22:07:38
* @Email: 326308290@qq.com
*/
    public sealed class LimitDecoratorStreamDataReader : DecoratorStreamDataReader
    {
        private readonly PaginationContext pagination;

        private readonly bool skipAll;

        private int rowNumber;

        public LimitDecoratorStreamDataReader(IStreamDataReader streamDataReader, PaginationContext pagination) : base(streamDataReader)
        {
            this.pagination = pagination;
            skipAll = SkipOffset();
        }

        private bool SkipOffset()
        {
            for (int i = 0; i < pagination.GetActualOffset(); i++)
            {
                if (!StreamDataReader.Read())
                {
                    return true;
                }
            }

            rowNumber = 0;
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
            return ++rowNumber <= pagination.GetActualRowCount() && StreamDataReader.Read();

        }
    }
}