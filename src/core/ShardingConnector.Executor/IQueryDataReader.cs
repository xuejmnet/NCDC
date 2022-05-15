// using System;
// using System.Data;
//
// namespace ShardingConnector.Executor
// {
//     /*
//     * @Author: xjm
//     * @Description:
//     * @Date: 2021/04/16 00:00:00
//     * @Ver: 1.0
//     * @Email: 326308290@qq.com
//     */
//     /// <summary>
//     /// 
//     /// </summary>
//     public interface IQueryDataReader
//     {
//         bool Read();
//         int ColumnCount { get; }
//         string GetColumnName(int columnIndex);
//         string GetColumnLabel(int columnIndex);
//
//         object this[int columnIndex] { get; }
//
//         object this[string name] { get; }
//
//         string GetName(int columnIndex);
//
//         string GetDataTypeName(int columnIndex);
//
//         Type GetFieldType(int columnIndex);
//
//         object GetValue(int columnIndex);
//
//         int GetValues(object[] values);
//
//         int GetOrdinal(string name);
//
//         bool GetBoolean(int columnIndex);
//
//         byte GetByte(int columnIndex);
//
//         long GetBytes(int columnIndex, long fieldOffset, byte[] buffer, int bufferOffset, int length);
//
//         char GetChar(int columnIndex);
//
//         long GetChars(int columnIndex, long fieldOffset, char[] buffer, int bufferOffset, int length);
//
//         Guid GetGuid(int columnIndex);
//
//         short GetInt16(int columnIndex);
//
//         int GetInt32(int columnIndex);
//
//         long GetInt64(int columnIndex);
//
//         float GetFloat(int columnIndex);
//
//         double GetDouble(int columnIndex);
//
//         string GetString(int columnIndex);
//
//         Decimal GetDecimal(int columnIndex);
//
//         DateTime GetDateTime(int columnIndex);
//
//         bool IsDBNull(int columnIndex);
//         bool NextResult();
//     }
// }