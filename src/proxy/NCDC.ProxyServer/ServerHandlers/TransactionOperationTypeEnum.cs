namespace NCDC.ProxyServer.ServerHandlers;

public enum TransactionOperationTypeEnum
{
    BEGIN,COMMIT,ROLLBACK,SAVEPOINT, ROLLBACK_TO_SAVEPOINT, RELEASE_SAVEPOINT, SET_AUTOCOMMIT
}