using System;
using ShardingConnector.Merge.Reader;
using ShardingConnector.Merge.Reader.Decorator;
using ShardingConnector.ParserBinder.Segment.Select.Pagination;

namespace ShardingConnector.ShardingMerge.DQL.Pagination
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 22:07:38
* @Email: 326308290@qq.com
*/
    public sealed class LimitDecoratorMergedEnumerator : DecoratorMergedEnumerator
    {
        private readonly PaginationContext pagination;

        private readonly bool skipAll;

        private int rowNumber;

        public LimitDecoratorMergedEnumerator(IMergedEnumerator mergedEnumerator, PaginationContext pagination) : base(mergedEnumerator)
        {
            this.pagination = pagination;
            skipAll = SkipOffset();
        }

        private bool SkipOffset()
        {
            for (int i = 0; i < pagination.GetActualOffset(); i++)
            {
                if (!MergedEnumerator.MoveNext())
                {
                    return true;
                }
            }

            rowNumber = 0;
            return false;
        }

        public override bool MoveNext()
        { 
            if (skipAll) {
                return false;
            }
            if (pagination.GetActualRowCount()==null) {
                return MergedEnumerator.MoveNext();
            }
            return ++rowNumber <= pagination.GetActualRowCount() && MergedEnumerator.MoveNext();

        }
    }
}