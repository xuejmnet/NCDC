using System.Net;
using DotNetty.Transport.Channels;

namespace NCDC.ProxyClient;

public static class RemotingHelper
{

    public static string ParseChannelRemoteAddress(IChannel? channel)
    {
        if (channel == null)
            return string.Empty;
        string str = ((IPEndPoint) channel.RemoteAddress).Address.ToString();
        if (string.IsNullOrWhiteSpace(str))
            return string.Empty;
        int num = str.LastIndexOf("/", StringComparison.Ordinal);
        return num >= 0 ? str.Substring(num + 1) : str;
    }

    public static string ParseSocketAddressAddress(EndPoint? socketAddress) => socketAddress != null ? socketAddress.ToString() : string.Empty;
}