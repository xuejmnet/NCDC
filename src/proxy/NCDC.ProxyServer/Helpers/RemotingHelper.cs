using System.Net;
using DotNetty.Transport.Channels;

namespace NCDC.ProxyServer.Helpers;

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
    public static string GetHostAddress(IChannelHandlerContext context) {
        //获取Ip
        IPEndPoint iPEndPoint = (IPEndPoint)context.Channel.RemoteAddress;
            
        string addr = iPEndPoint.Address.ToString();

        if (!string.IsNullOrWhiteSpace(addr)) {
            int index = addr.LastIndexOf("/", StringComparison.Ordinal);
            if (index >= 0) {
                return addr.Substring(index + 1);
            }
            return addr;
        }

        return string.Empty;
    }
}