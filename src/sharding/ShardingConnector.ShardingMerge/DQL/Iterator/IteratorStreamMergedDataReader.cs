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
    public sealed class IteratorStreamMergedDataReader:StreamMergedDataReader
    {
        private readonly IEnumerator<IQueryDataReader> _queryEnumerator;

        public IteratorStreamMergedDataReader( List<IQueryDataReader> queryResults)
        {
            this._queryEnumerator = queryResults.GetEnumerator();
            SetCurrentQueryEnumerator(this._queryEnumerator.Next());
        }
        public override bool Read()
        {
            if (GetCurrentQueryDataReader().Read())
            {
                return true;
            }
            if (!_queryEnumerator.MoveNext())
            {
                return false;
            }
            SetCurrentQueryEnumerator(_queryEnumerator.Current);
            var hasNext = GetCurrentQueryDataReader().Read();
            if (hasNext)
            {
                return true;
            }
            while (!hasNext && _queryEnumerator.MoveNext())
            {
                SetCurrentQueryEnumerator(_queryEnumerator.Current);
                hasNext = GetCurrentQueryDataReader().Read();
            }
            return hasNext;
        }
    }
}
