using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Executor;
using ShardingConnector.Extensions;
using ShardingConnector.Merge.Reader.Stream;

namespace ShardingConnector.ShardingMerge.DQL.Iterator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/6 8:07:35
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class IteratorStreamMergedEnumerator:StreamMergedEnumerator
    {
        private readonly IEnumerator<IQueryEnumerator> _queryEnumerator;

        public IteratorStreamMergedEnumerator( List<IQueryEnumerator> queryResults)
        {
            this._queryEnumerator = queryResults.GetEnumerator();
            SetCurrentQueryEnumerator(this._queryEnumerator.Next());
        }
        public override bool MoveNext()
        {
            if (GetCurrentQueryEnumerator().MoveNext())
            {
                return true;
            }
            if (!_queryEnumerator.MoveNext())
            {
                return false;
            }
            SetCurrentQueryEnumerator(_queryEnumerator.Current);
            var hasNext = GetCurrentQueryEnumerator().MoveNext();
            if (hasNext)
            {
                return true;
            }
            while (!hasNext && _queryEnumerator.MoveNext())
            {
                SetCurrentQueryEnumerator(_queryEnumerator.Current);
                hasNext = GetCurrentQueryEnumerator().MoveNext();
            }
            return hasNext;
        }
    }
}
