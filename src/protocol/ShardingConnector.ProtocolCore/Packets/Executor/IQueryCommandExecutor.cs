namespace ShardingConnector.ProtocolCore.Packets.Executor;

public interface IQueryCommandExecutor:ICommandExecutor
{
    ResponseTypeEnum GetResponseType();

    bool MoveNext()
    {
        return false;
    }
    IDatabasePacket GetQueryRowPacket();
}