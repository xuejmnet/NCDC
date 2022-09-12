using System.Runtime.Serialization;

namespace NCDC.Exceptions;

public class ShardingNotSupportedException:ShardingException
{
    public ShardingNotSupportedException()
    {
    }

    protected ShardingNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ShardingNotSupportedException(string? message) : base(message)
    {
    }

    public ShardingNotSupportedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}