using System.Runtime.Serialization;

namespace NCDC.Exceptions;

public class ShardingInvalidOperationException:ShardingException
{
    public ShardingInvalidOperationException()
    {
    }

    protected ShardingInvalidOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ShardingInvalidOperationException(string message) : base(message)
    {
    }

    public ShardingInvalidOperationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}