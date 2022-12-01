using DotNetty.Transport.Channels;
using NCDC.Basic.User;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Connection;

public interface IConnectionSessionFactory
{
    IConnectionSession Create(int connectionId,string database,Grantee grantee,IChannel channel);
}