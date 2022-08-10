namespace ShardingConnector.ProtocolMysql.Helper;

public class BytesHelper
{
    private BytesHelper()
    {
        
    }
    public static byte[] CombineBytes(byte[] b1, byte[] b2)
    {
        int size = (b1?.Length ?? 0) + (b2?.Length ?? 0);
        byte[] total = new byte[size];
        if (null != b1)
            Array.Copy(b1, 0, total, 0, b1.Length);
        if (null != b2)
            Array.Copy(b2, 0, total, b1?.Length ?? 0, b2.Length);
        return total;
    }
}