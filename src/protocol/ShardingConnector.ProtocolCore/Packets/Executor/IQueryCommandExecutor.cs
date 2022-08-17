namespace ShardingConnector.ProtocolCore.Packets.Executor;

public interface IQueryCommandExecutor:ICommandExecutor
{
    ResponseTypeEnum GetResponseType();

    bool GetNext()
    {
        return false;
    }
    IDatabasePacket GetQueryRowPacket();
}