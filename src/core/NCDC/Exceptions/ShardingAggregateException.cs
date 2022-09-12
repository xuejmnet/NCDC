using System.Runtime.Serialization;

namespace NCDC.Exceptions
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public class ShardingAggregateException:ShardingException
    {
        public ICollection<Exception> Exceptions { get; }

        public ShardingAggregateException(string message,ICollection<Exception> exceptions): base(message)
        {
            Exceptions = exceptions;
        }

        protected ShardingAggregateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ShardingAggregateException(string message) : base(message)
        {
        }

        public ShardingAggregateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}