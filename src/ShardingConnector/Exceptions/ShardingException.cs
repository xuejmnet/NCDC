using System;
using System.Runtime.Serialization;

namespace ShardingConnector.Exceptions
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 21 March 2021 13:01:28
* @Email: 326308290@qq.com
*/
    public class ShardingException:Exception
    {
        public ShardingException()
        {
        }

        protected ShardingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ShardingException(string message) : base(message)
        {
        }

        public ShardingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}