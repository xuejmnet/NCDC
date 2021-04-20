namespace ShardingConnector.Merge.Reader.Decorator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 笛卡尔积的合并
    /// </summary>
    public abstract class DecoratorMergedEnumerator:IMergedEnumerator
    {
        public  IMergedEnumerator MergedEnumerator { get; }

        protected DecoratorMergedEnumerator(IMergedEnumerator mergedEnumerator)
        {
            MergedEnumerator = mergedEnumerator;
        }

        public bool MoveNext()
        {
            return MergedEnumerator.MoveNext();
        }

        public object GetValue(int columnIndex)
        {
            return MergedEnumerator.GetValue(columnIndex);
        }

        public T GetValue<T>(int columnIndex)
        {
            return MergedEnumerator.GetValue<T>(columnIndex);
        }

        public bool IsDBNull(int columnIndex)
        {
            return MergedEnumerator.IsDBNull(columnIndex);
        }
    }
}