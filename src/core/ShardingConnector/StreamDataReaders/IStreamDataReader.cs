

/*
* @Author: xjm
* @Description:
* @Date: DATE TIME
* @Email: 326308290@qq.com
*/
namespace ShardingConnector.StreamDataReaders
{
    public interface IStreamDataReader:IDisposable
    {
        bool Read();
        int ColumnCount { get; }
        string GetColumnName(int columnIndex);
        string GetColumnLabel(int columnIndex);

        object this[int columnIndex] { get; }

        object this[string name] { get; }

        string GetName(int columnIndex);

        string GetDataTypeName(int columnIndex);

        Type GetFieldType(int columnIndex);

        object GetValue(int columnIndex);

        int GetValues(object[] values);

        int GetOrdinal(string name);

        bool GetBoolean(int columnIndex);

        byte GetByte(int columnIndex);

        long GetBytes(int columnIndex, long fieldOffset, byte[] buffer, int bufferOffset, int length);

        char GetChar(int columnIndex);

        long GetChars(int columnIndex, long fieldOffset, char[] buffer, int bufferOffset, int length);

        Guid GetGuid(int columnIndex);

        short GetInt16(int columnIndex);

        int GetInt32(int columnIndex);

        long GetInt64(int columnIndex);

        float GetFloat(int columnIndex);

        double GetDouble(int columnIndex);

        string GetString(int columnIndex);

        Decimal GetDecimal(int columnIndex);

        DateTime GetDateTime(int columnIndex);

        bool IsDBNull(int columnIndex);
        bool NextResult();
    }
}