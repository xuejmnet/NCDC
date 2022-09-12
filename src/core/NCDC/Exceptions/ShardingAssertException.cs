using System.Runtime.Serialization;

namespace NCDC.Exceptions
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 11:24:54
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ShardingAssertException : ShardingException
    {
        public ShardingAssertException()
        {
        }

        protected ShardingAssertException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ShardingAssertException(string message) : base(message)
        {
        }

        public ShardingAssertException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}