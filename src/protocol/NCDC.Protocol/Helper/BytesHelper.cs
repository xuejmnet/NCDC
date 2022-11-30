namespace NCDC.Protocol.Helper;

public class BytesHelper
{
    private BytesHelper()
    {
    }

    public static byte[] CombineBytes(byte[] b1, byte[] b2)
    {
        int size = b1.Length + b2.Length;
        byte[] total = new byte[size];
        Array.Copy(b1, 0, total, 0, b1.Length);
        Array.Copy(b2, 0, total, b1.Length, b2.Length);
        return total;
    }
}