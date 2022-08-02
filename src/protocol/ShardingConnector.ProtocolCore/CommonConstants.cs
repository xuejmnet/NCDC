using System.Text;
using DotNetty.Common.Utilities;

namespace ShardingConnector.ProtocolCore;

public sealed class CommonConstants
{
    public static readonly AttributeKey<Encoding> CHARSET_ATTRIBUTE_KEY=AttributeKey<Encoding>.ValueOf(typeof(Encoding).FullName);
}