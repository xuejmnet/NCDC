using System.Threading;

namespace ShardingConnector.Transaction.Core
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 14:13:01
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class TransactionTypeHolder
    {
        private static readonly AsyncLocal<TransactionTypeEnum?> Current = new AsyncLocal<TransactionTypeEnum?>();
        

        public static TransactionTypeEnum? Get()
        {
            return Current.Value;
        }

        public static void Set(TransactionTypeEnum type)
        {
            Current.Value = type;
        }

        public static void Clear()
        {
            Current.Value = null;
        }
    }
}