using ShardingConnector.Executor;

namespace ShardingConnector.Merge.Reader.Transparent
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 17:13:38
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class TransparentMergedEnumerator:IMergedEnumerator
    {
        private readonly IQueryEnumerator queryEnumerator;

        public TransparentMergedEnumerator(IQueryEnumerator queryEnumerator)
        {
            this.queryEnumerator = queryEnumerator;
        }

        public bool MoveNext()
        {
            return queryEnumerator.MoveNext();
        }

        public object GetValue(int columnIndex)
        {
            return queryEnumerator.GetValue(columnIndex);
        }

        public T GetValue<T>(int columnIndex)
        {
            return queryEnumerator.GetValue<T>(columnIndex);
        }

        public bool IsDBNull(int columnIndex)
        {
            return queryEnumerator.IsDBNull(columnIndex);
        }
    }
}