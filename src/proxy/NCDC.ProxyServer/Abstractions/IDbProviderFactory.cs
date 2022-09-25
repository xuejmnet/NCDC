using System.Data.Common;

namespace NCDC.ProxyServer.Abstractions;

public interface IDbProviderFactory
{
    DbProviderFactory Create();
}