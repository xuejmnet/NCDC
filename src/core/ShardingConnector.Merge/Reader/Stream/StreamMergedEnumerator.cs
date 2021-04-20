using ShardingConnector.Exceptions;
using ShardingConnector.Executor;

namespace ShardingConnector.Merge.Reader.Stream
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 16:42:45
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public abstract class StreamMergedEnumerator : IMergedEnumerator
    {
        private IQueryEnumerator currentQueryEnumerator;

        public abstract bool MoveNext();

        public object GetValue(int columnIndex)
        {
            var queryEnumerator = GetCurrentQueryEnumerator();
            object result = queryEnumerator.GetValue(columnIndex);
            return result;
        }

        public T GetValue<T>(int columnIndex)
        {
            var queryEnumerator = GetCurrentQueryEnumerator();
            T result = queryEnumerator.GetValue<T>(columnIndex);
            return result;
        }

        public bool IsDBNull(int columnIndex)
        {
            return currentQueryEnumerator.IsDBNull(columnIndex);
        }

        protected IQueryEnumerator GetCurrentQueryEnumerator()
        {
            if (null == currentQueryEnumerator)
            {
                throw new ShardingException("Current DataReader is null, DataReader perhaps end of next.");
            }

            return currentQueryEnumerator;
        }

        public void SetCurrentQueryEnumerator(IQueryEnumerator queryEnumerator)
        {
            currentQueryEnumerator = queryEnumerator;
        }

    }
}