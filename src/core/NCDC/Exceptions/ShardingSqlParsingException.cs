﻿using System.Runtime.Serialization;

namespace NCDC.Exceptions
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 10:00:59
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ShardingSqlParsingException : ShardingException
    {
        public ShardingSqlParsingException()
        {
        }

        protected ShardingSqlParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ShardingSqlParsingException(string message) : base(message)
        {
        }

        public ShardingSqlParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}