namespace OpenConnector.ProxyServer.Commons
{
    public enum ConnectionModeEnum
    {
        //内存限制使用流式聚合
        MEMORY_STRICTLY,
        //链接限制使用内存聚合
        CONNECTION_STRICTLY
    }
}