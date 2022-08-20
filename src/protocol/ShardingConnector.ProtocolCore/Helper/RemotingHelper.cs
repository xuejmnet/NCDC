using System.Net;
using DotNetty.Transport.Channels;

namespace ShardingConnector.ProtocolCore.Helper;

public class RemotingHelper
{
    
    private RemotingHelper(){}
    /// <summary>
    /// 通过channel解析Ip地址
    /// </summary>
    /// <param name="channel"></param>
    /// <returns></returns>
    public static string ParseChannelRemoteAddress(IChannel? channel) {
        if (null == channel) {
            return string.Empty;
        }
        //获取Ip
        IPEndPoint iPEndPoint = (IPEndPoint)channel.RemoteAddress;
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
    public static String ParseSocketAddressAddress(EndPoint? socketAddress) {
        if (socketAddress != null) {
            var addr = socketAddress.ToString();
            return addr;
        }
        return string.Empty;
    }
}