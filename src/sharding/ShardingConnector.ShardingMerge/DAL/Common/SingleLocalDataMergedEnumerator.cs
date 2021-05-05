using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Merge.Reader;

namespace ShardingConnector.ShardingMerge.DAL.Common
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/5 10:39:24
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SingleLocalDataMergedEnumerator:IMergedEnumerator
    {
        private readonly IEnumerator<object> values;

        private object currentValue;

        public SingleLocalDataMergedEnumerator(ICollection<object> values)
        {
            this.values = values.GetEnumerator();
        }
        public bool MoveNext()
        {
            if (values.MoveNext())
            {
                currentValue = values.Current;
                return true;
            }
            return false;
        }

        public object GetValue(int columnIndex)
        {
            return currentValue;
        }

        public T GetValue<T>(int columnIndex)
        {
            return (T) currentValue;
        }

        public bool IsDBNull(int columnIndex)
        {
            return null == currentValue;
        }
    }
}
