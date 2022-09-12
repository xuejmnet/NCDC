using NCDC.Protocol.Helper;

namespace NCDC.Protocol.MySql.Packet.Handshake;

public sealed class MySqlRandomGenerator
{
    private static MySqlRandomGenerator instance = new MySqlRandomGenerator();
    public static MySqlRandomGenerator Instance => instance;
    
    private static readonly byte[] SEED = (new []{
        'a', 'b', 'e', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', }).Select(o=>(byte)o).ToArray();

    public byte[] GenerateRandomBytes(int length)
    {
        var result = new byte[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = SEED[RandomHelper.Next(SEED.Length)];
        }

        return result;
    }
}