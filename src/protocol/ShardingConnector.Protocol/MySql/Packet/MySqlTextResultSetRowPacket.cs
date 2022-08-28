using System.Globalization;
using ShardingConnector.Protocol.MySql.Payload;

namespace ShardingConnector.Protocol.MySql.Packet;

public sealed class MySqlTextResultSetRowPacket:IMysqlPacket
{
    private const int NULL = 0xfb;
    
    // private static final DateTimeFormatter DT_FMT = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");
    
    private readonly  int _sequenceId;
    
    private readonly  List<object?> _data;

    public MySqlTextResultSetRowPacket(int sequenceId,List<object?> data)
    {
        _sequenceId = sequenceId;
        _data = data;
    }
    public MySqlTextResultSetRowPacket( MySqlPacketPayload payload,  int columnCount) {
        _sequenceId = payload.ReadInt1();
        _data = new(columnCount);
        for (int i = 0; i < columnCount; i++) {
            _data.Add(payload.ReadStringLenenc());
        }
    }
    public void WriteTo(MySqlPacketPayload payload)
    {
        foreach (object o in _data)
        {
            if (o is null)
            {
                payload.WriteInt1(NULL);
            }
            else
            {
                if (o is byte[] bb) {
                    payload.WriteBytesLenenc(bb);
                } 
                // else if ((o is Timestamp) && (0 == ((Timestamp) each).getNanos())) {
                //     payload.WriteStringLenenc(each.toString().split("\\.")[0]);
                // } 
                else if (o is decimal d) {
                    payload.WriteStringLenenc(d.ToString(CultureInfo.InvariantCulture));
                } else if (o is bool b) {
                    payload.WriteBytesLenenc(b ? new byte[]{1} : new byte[]{0});
                } else if (o is DateTime dt) {
                    payload.WriteStringLenenc(dt.ToString("yyyy-MM-dd HH:mm:ss"));
                } else {
                    payload.WriteStringLenenc(o.ToString());
                }
            }
        }
    }

    public int SequenceId => _sequenceId;
}