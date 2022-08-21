using System.Text;
using DotNetty.Buffers;
using ShardingConnector.Protocol.Packets;

namespace ShardingConnector.Protocol.MySql.Payload;

public sealed class MySqlPacketPayload:IPacketPayload
{
    private readonly IByteBuffer _byteBuffer;
    private readonly Encoding _encoding;

    public MySqlPacketPayload(IByteBuffer byteBuffer,Encoding encoding)
    {
        _byteBuffer = byteBuffer;
        _encoding = encoding;
    }

    public int ReadInt1()
    {
        return _byteBuffer.ReadByte() & 0xff;//转成int保证后面8位不变
    }

    public void WriteInt1(int value)
    {
        _byteBuffer.WriteByte(value);
    }

    public int ReadInt2()
    {
        return _byteBuffer.ReadShortLE() & 0xffff;
    }

    public IByteBuffer GetByteBuffer()
    {
        return _byteBuffer;
    }

    public void WriteInt2(int value)
    {
        _byteBuffer.WriteShortLE(value);
    }
    
    public int ReadInt3()
    {
        return _byteBuffer.ReadMediumLE() & 0xffffff;
    } 


    public void WriteInt3(int value)
    {
        _byteBuffer.WriteMediumLE(value);
    }

    public int ReadInt4()
    {
        return _byteBuffer.ReadIntLE();
    }

    public void WriteInt4(int value)
    {
        _byteBuffer.WriteIntLE(value);
    }

    public long ReadInt6()
    {
        long result = 0;
        for (int i = 0; i < 6; i++)
        {
            result|=((long) (0xff & _byteBuffer.ReadByte())) << (8 * i);
        }

        return result;
    }

    public void WriteInt6()
    {
        
    }

    public long ReadInt8()
    {
        return _byteBuffer.ReadLongLE();
    }

    public void WriteInt8(long value)
    {
        _byteBuffer.WriteLongLE(value);
    }

    public long ReadIntLenenc()
    {
        int firstByte = ReadInt1();
        if (firstByte < 0xfb) {
            return firstByte;
        }
        if (0xfb == firstByte) {
            return 0;
        }
        if (0xfc == firstByte) {
            return _byteBuffer.ReadShortLE();
        }
        if (0xfd == firstByte) {
            return _byteBuffer.ReadMediumLE();
        }
        return _byteBuffer.ReadLongLE();
    }
    public void WriteIntLenenc( long value) {
        if (value < 0xfb) {
            _byteBuffer.WriteByte((int) value);
            return;
        }
        if (value < Math.Pow(2, 16)) {
            _byteBuffer.WriteByte(0xfc);
            _byteBuffer.WriteShortLE((int) value);
            return;
        }
        if (value < Math.Pow(2, 24)) {
            _byteBuffer.WriteByte(0xfd);
            _byteBuffer.WriteMediumLE((int) value);
            return;
        }
        _byteBuffer.WriteByte(0xfe);
        _byteBuffer.WriteLongLE(value);
    }
    public string ReadStringLenenc() {
        int length = (int) ReadIntLenenc();
        byte[] result = new byte[length];
        _byteBuffer.ReadBytes(result);
        return _encoding.GetString(result);
    }
    public byte[] ReadStringLenencByBytes() {
        int length = (int) ReadIntLenenc();
        byte[] result = new byte[length];
        _byteBuffer.ReadBytes(result);
        return result;
    }
    public void WriteStringLenenc( String value) {
        if (string.IsNullOrEmpty(value)) {
            _byteBuffer.WriteByte(0);
            return;
        }

        var bytes = _encoding.GetBytes(value);
        WriteIntLenenc(bytes.Length);
        _byteBuffer.WriteBytes(bytes);
    }
    public void WriteBytesLenenc( byte[] value) {
        if (0 == value.Length) {
            _byteBuffer.WriteByte(0);
            return;
        }
        WriteIntLenenc(value.Length);
        _byteBuffer.WriteBytes(value);
    }
    public string ReadStringFix( int length) {
        byte[] result = new byte[length];
        _byteBuffer.ReadBytes(result);
        return _encoding.GetString(result);
    }
    public byte[] ReadStringFixByBytes( int length) {
        byte[] result = new byte[length];
        _byteBuffer.ReadBytes(result);
        return result;
    }
    public void WriteStringFix( string value) {
        _byteBuffer.WriteBytes(_encoding.GetBytes(value));
    }
    public void WriteBytes( byte[] value) {
        _byteBuffer.WriteBytes(value);
    }
    public string ReadStringVar() {
        // TODO
        return "";
    }
    public void WriteStringVar( string value) {
        // TODO
    }
    public string ReadStringNul() {
        byte[] result = new byte[_byteBuffer.BytesBefore((byte) 0)];
        _byteBuffer.ReadBytes(result);
        _byteBuffer.SkipBytes(1);
        return _encoding.GetString(result);
    }
    public byte[] ReadStringNulByBytes() {
        byte[] result = new byte[_byteBuffer.BytesBefore((byte) 0)];
        _byteBuffer.ReadBytes(result);
        _byteBuffer.SkipBytes(1);
        return result;
    }
    public void WriteStringNul( string value) {
        _byteBuffer.WriteBytes(_encoding.GetBytes(value));
        _byteBuffer.WriteByte(0);
    }
    public byte[] ReadStringEOFByBytes() {
        byte[] result = new byte[_byteBuffer.ReadableBytes];
        _byteBuffer.ReadBytes(result);
        return result;
    }
    public string ReadStringEOF() {
        byte[] result = new byte[_byteBuffer.ReadableBytes];
        _byteBuffer.ReadBytes(result);
        return _encoding.GetString(result);
    }
    public void WriteStringEOF( String value) {
        _byteBuffer.WriteBytes(_encoding.GetBytes(value));
    }
    public void SkipReserved( int length) {
        _byteBuffer.SkipBytes(length);
    }
    public void WriteReserved( int length) {
        for (int i = 0; i < length; i++) {
            _byteBuffer.WriteByte(0);
        }
    }
    public void Dispose()
    {
        _byteBuffer.Release();
    }
}