namespace NCDC.Basic.Connection.Abstractions;

public interface IServerDbTransaction:IDisposable
{
    void CommitTransaction();
    void RollbackTransaction();
    IServerDbConnection GetServerDbConnection();
}