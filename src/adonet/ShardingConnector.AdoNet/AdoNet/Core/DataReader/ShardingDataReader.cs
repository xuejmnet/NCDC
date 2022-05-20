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

        private readonly IStreamDataReader _streamDataReader;
        //private readonly IDictionary<string, int> _columnLabelAndIndexMap;

        public ShardingDataReader(List<DbDataReader> dataReaders, IStreamDataReader streamDataReader, DbCommand command,
            ExecutionContext executionContext) : base(dataReaders, command, executionContext)
        {
            _streamDataReader = streamDataReader;
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
            return _streamDataReader.IsDBNull(ordinal);
        }
        /// <summary>
        /// 读取下个结果集比如批处理返回多个结果集
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool NextResult()
        {
            return _streamDataReader.NextResult();
        }

        public override bool Read()
        {
            Console.WriteLine("read");
            return _streamDataReader.Read();
        }


        public override bool GetBoolean(int ordinal)
        {
            return _streamDataReader.GetBoolean(ordinal);
        }

        public override byte GetByte(int ordinal)
        {
            return _streamDataReader.GetByte(ordinal);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return _streamDataReader.GetBytes(ordinal, dataOffset,buffer,bufferOffset,length);
        }

        public override char GetChar(int ordinal)
        {
            return _streamDataReader.GetChar(ordinal);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return _streamDataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public override string GetDataTypeName(int ordinal)
        {
            return _streamDataReader.GetDataTypeName(ordinal);
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return _streamDataReader.GetDateTime(ordinal);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return _streamDataReader.GetDecimal(ordinal);
        }

        public override double GetDouble(int ordinal)
        {
            return _streamDataReader.GetDouble(ordinal);
        }

        public override IEnumerator GetEnumerator()
        {
            //TODO ShardingEnumerator
            throw new NotImplementedException();
        }

        public override Type GetFieldType(int ordinal)
        {
            return _streamDataReader.GetFieldType(ordinal);
        }

        public override float GetFloat(int ordinal)
        {
            return _streamDataReader.GetFloat(ordinal);
        }

        public override Guid GetGuid(int ordinal)
        {
            return _streamDataReader.GetGuid(ordinal);
        }

        public override short GetInt16(int ordinal)
        {
            return _streamDataReader.GetInt16(ordinal);
        }

        public override int GetInt32(int ordinal)
        {
            return _streamDataReader.GetInt32(ordinal);
        }

        public override long GetInt64(int ordinal)
        {
            return _streamDataReader.GetInt64(ordinal);
        }

        public override string GetName(int ordinal)
        {
            return _streamDataReader.GetName(ordinal);
        }

        public override string GetString(int ordinal)
        {
            return _streamDataReader.GetString(ordinal);
        }

        public override object GetValue(int ordinal)
        {
            return _streamDataReader.GetValue(ordinal);
        }

        public override int GetValues(object[] values)
        {
            return _streamDataReader.GetValues(values);
        }
    }
}