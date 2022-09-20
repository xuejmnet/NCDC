using System.Text;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using NCDC.Protocol.MySql.Constant;

namespace NCDC.ProxyServer.Extensions;

public static class ChannelExtension
{
    private static readonly AttributeKey<MySqlCharacterSet> MYSQL_CHARACTER_SET_ATTRIBUTE_KEY=AttributeKey<MySqlCharacterSet>.ValueOf(typeof(MySqlCharacterSet).FullName);
    private static readonly AttributeKey<Encoding> CHARACTER_SET_ATTRIBUTE_KEY=AttributeKey<Encoding>.ValueOf(typeof(Encoding).FullName);
    public static void SetEncoding(this IChannel channel, Encoding encoding)
    {
        channel.GetAttribute(CHARACTER_SET_ATTRIBUTE_KEY).SetIfAbsent(encoding);
    }
    public static Encoding GetEncoding(this IChannel channel)
    {
        return channel.GetAttribute(CHARACTER_SET_ATTRIBUTE_KEY).Get();
    }
    public static void SetMySqlCharacterSet(this IChannel channel, MySqlCharacterSet mySqlCharacterSet)
    {
        channel.GetAttribute(MYSQL_CHARACTER_SET_ATTRIBUTE_KEY).SetIfAbsent(mySqlCharacterSet);
    }
    public static MySqlCharacterSet GetMySqlCharacterSet(this IChannel channel)
    {
        return channel.GetAttribute(MYSQL_CHARACTER_SET_ATTRIBUTE_KEY).Get();
    }
}