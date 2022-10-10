namespace NCDC.ProxyServer.ServerHandlers.ServerTransactions;

public class TransactionHolder
{
    private static AsyncLocal<bool?> _inTransaction = new AsyncLocal<bool?>();
    public static bool? InTransaction 
    {
        get => _inTransaction.Value;
        set => _inTransaction.Value = value;
    }
}