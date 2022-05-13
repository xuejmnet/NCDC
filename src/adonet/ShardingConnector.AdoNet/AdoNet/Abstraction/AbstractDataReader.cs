using ShardingConnector.Exceptions;
using ShardingConnector.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExecutionContext = ShardingConnector.Executor.Context.ExecutionContext;

namespace ShardingConnector.AdoNet.AdoNet.Abstraction
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 14:24:08
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractDataReader : DbDataReader
    {
        private readonly List<DbDataReader> _dataReaders;

        private readonly DbCommand _command;
        private readonly ExecutionContext executionContext;

        private bool closed;


        private readonly ForceExecuteTemplate<DbDataReader> _forceExecuteTemplate =
            new ForceExecuteTemplate<DbDataReader>();

        public AbstractDataReader(List<DbDataReader> dataReaders, DbCommand command, ExecutionContext executionContext)
        {
            if (dataReaders.IsEmpty())
                throw new ArgumentNullException(nameof(dataReaders));
            this._dataReaders = dataReaders;
            this._command = command;
            this.executionContext = executionContext;
        }

        public abstract override bool GetBoolean(int ordinal);

        public abstract override byte GetByte(int ordinal);

        public abstract override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset,
            int length);

        public abstract override char GetChar(int ordinal);

        public abstract override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset,
            int length);

        public abstract override string GetDataTypeName(int ordinal);

        public abstract override DateTime GetDateTime(int ordinal);

        public abstract override decimal GetDecimal(int ordinal);

        public abstract override double GetDouble(int ordinal);

        public abstract override Type GetFieldType(int ordinal);

        public abstract override float GetFloat(int ordinal);

        public abstract override Guid GetGuid(int ordinal);

        public abstract override short GetInt16(int ordinal);

        public abstract override int GetInt32(int ordinal);

        public abstract override long GetInt64(int ordinal);

        public abstract override string GetName(int ordinal);

        public override int GetOrdinal(string name)
        {
            return _dataReaders[0].GetOrdinal(name);
        }

        public abstract override string GetString(int ordinal);

        public abstract override object GetValue(int ordinal);

        public abstract override int GetValues(object[] values);

        public abstract override bool IsDBNull(int ordinal);

        public  override int FieldCount => _dataReaders[0].FieldCount;

        public abstract override object this[int ordinal]{ get; }

        public abstract override object this[string name] { get; }

        public override int RecordsAffected => _dataReaders.Sum(o=>o.RecordsAffected);

        public override bool HasRows
        {
            get
            {
                if (closed)
                    throw new ShardingException("datareader is closed");
                return _dataReaders.Any(o => o.HasRows);
            }
        }

        public override bool IsClosed => closed;


        public abstract override bool NextResult();

        public abstract override bool Read();

        public override int Depth
        {
            get
            {
                if (closed)
                    throw new ShardingException("datareader is closed");
                return 0;
            }
        }

        public abstract override IEnumerator GetEnumerator();
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _forceExecuteTemplate.Execute(_dataReaders,reader=>reader.Dispose());
        }
    }
}