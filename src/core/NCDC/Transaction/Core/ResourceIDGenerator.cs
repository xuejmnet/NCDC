using System.Threading;

namespace OpenConnector.Transaction.Core
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 13:55:27
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class ResourceIDGenerator
    {
        private static readonly ResourceIDGenerator INSTANCE = new ResourceIDGenerator();

        private int count = 0;

        /**
     * Get instance.
     *
     * @return instance
     */
        public static ResourceIDGenerator GetInstance()
        {
            return INSTANCE;
        }

        /**
     * Next unique resource id.
     *
     * @return next ID
     */
        public string NextId()
        {
            Interlocked.Increment(ref count);
            return $"resource-{count}-";
        }
    }
}