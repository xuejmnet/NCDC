using ShardingConnector.AdoNet.AdoNet.Abstraction;
using ShardingConnector.Merge.Reader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using ExecutionContext = ShardingConnector.Executor.Context.ExecutionContext;

namespace ShardingConnector.AdoNet.AdoNet.Core.DataReader
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public class ShardingDataReader : AbstractDataReader
    {

        private readonly IMergedDataReader _mergedDataReader;
        //private readonly IDictionary<string, int> _columnLabelAndIndexMap;

        public ShardingDataReader(List<DbDataReader> dataReaders, IMergedDataReader mergedDataReader, DbCommand command,
            ExecutionContext executionContext) : base(dataReaders, command, executionContext)
        {
            _mergedDataReader = mergedDataReader;
            //_columnLabelAndIndexMap = CreateColumnLabelAndIndexMap(dataReaders[0]);
        }

        //private IDictionary<string, int> CreateColumnLabelAndIndexMap(DbDataReader dataReader)
        //{
        //    IDictionary<string, int> result = new SortedDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        //    for (int columnIndex = 0; columnIndex < dataReader.FieldCount; columnIndex++)
        //    {
        //        result.Add(dataReader.GetName(columnIndex), columnIndex);
        //    }

        //    return result;
        //}
        

        public override bool IsDBNull(int ordinal)
        {
            return _mergedDataReader.IsDBNull(ordinal);
        }
        /// <summary>
        /// 读取下个结果集比如批处理返回多个结果集
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        public override bool Read()
        {
            return _mergedDataReader.Read();
        }

        public override object this[int ordinal] => _mergedDataReader.GetValue(ordinal);

        public override object this[string name] => GetValueByName(name);

        private object GetValueByName(string name)
        {
            return _mergedDataReader.GetValue(name);
        }

        public override bool GetBoolean(int ordinal)
        {
            return _mergedDataReader.GetValue<bool>(ordinal);
        }

        public override byte GetByte(int ordinal)
        {
            return _mergedDataReader.GetValue<byte>(ordinal);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            return _mergedDataReader.GetValue<char>(ordinal);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return _mergedDataReader.GetValue<DateTime>(ordinal);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return _mergedDataReader.GetValue<decimal>(ordinal);
        }

        public override double GetDouble(int ordinal)
        {
            return _mergedDataReader.GetValue<double>(ordinal);
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override Type GetFieldType(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override float GetFloat(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override Guid GetGuid(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override short GetInt16(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetInt32(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetInt64(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override string GetName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override string GetString(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }
    }
}