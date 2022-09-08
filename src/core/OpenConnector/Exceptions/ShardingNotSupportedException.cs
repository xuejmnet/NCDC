using System.Runtime.Serialization;

namespace OpenConnector.Exceptions;

public class ShardingNotSupportedException:Exception
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