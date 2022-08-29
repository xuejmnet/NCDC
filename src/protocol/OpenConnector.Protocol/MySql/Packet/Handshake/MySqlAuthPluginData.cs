using OpenConnector.Protocol.Helper;

namespace OpenConnector.Protocol.MySql.Packet.Handshake;

public class MySqlAuthPluginData
{
    public byte[] Part1 { get; }
    public byte[] Part2{ get; }

    public MySqlAuthPluginData():this(MySqlRandomGenerator.Instance.GenerateRandomBytes(8),MySqlRandomGenerator.Instance.GenerateRandomBytes(12))
    {
        
    }

    public MySqlAuthPluginData(byte[] part1,byte[] part2)
    {
        Part1 = part1;
        Part2 = part2;
    }

    public byte[] GetAuthPluginData()
    {
        return BytesHelper.CombineBytes(Part1, Part2);
    }
    
}