using System.Runtime.Serialization;

namespace NCDC.Exceptions;

public class ShardingConfigException:ShardingException
{
    public ShardingConfigException()
    {
    }

    protected ShardingConfigException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ShardingConfigException(string? message) : base(message)
    {
    }

    public ShardingConfigException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}