using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using ShardingConnector.AdoNet.AdoNet.Abstraction;

namespace ShardingConnector.AdoNet.AdoNet.Core.Command
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/7/23 15:56:13
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ShardingParameter : DbParameter, IAdoMethodRecorder<DbParameter>
    {
        private readonly DbParameter _dbParameter;
        public ShardingParameterCollection ShardingParameters { get; set; }

        public ShardingParameter():this(null,null)
        {
            
        }
        public ShardingParameter(string parameterName,object parameterValue)
        {
            this._parameterName = parameterName ?? string.Empty;
            this._parameterValue = parameterValue;
        }

        public override void ResetDbType()
        {
            _dbParameter.ResetDbType();
            RecordTargetMethodInvoke(dbParameter => dbParameter.ResetDbType());
        }


        public override DbType DbType
        {
            get => _dbParameter.DbType;
            set
            {
                _dbParameter.DbType = value;
                RecordTargetMethodInvoke(dbParameter => dbParameter.DbType = value);
            }
        }

        public override ParameterDirection Direction
        {
            get => _dbParameter.Direction;
            set
            {
                _dbParameter.Direction = value;
                RecordTargetMethodInvoke(dbParameter => dbParameter.Direction = value);
            }
        }

        public override bool IsNullable
        {
            get => _dbParameter.IsNullable;
            set
            {
                _dbParameter.IsNullable = value;
                RecordTargetMethodInvoke(dbParameter => dbParameter.IsNullable = value);
            }
        }

        private string _parameterName;
        public override string ParameterName
        {
            get => _parameterName;
            set
            {
                var oldName = _parameterName;
                _parameterName = value;
                ShardingParameters?.ChangeParameterName(this, oldName, _parameterName);
                RecordTargetMethodInvoke(dbParameter => dbParameter.ParameterName = value);
            }
        }

        public override string SourceColumn
        {
            get => _dbParameter.SourceColumn;
            set
            {
                _dbParameter.SourceColumn = value;
                RecordTargetMethodInvoke(dbParameter => dbParameter.SourceColumn = value);
            }
        }

        private object _parameterValue;
        public override object Value
        {
            get => _parameterValue;
            set
            {
                _parameterValue = value;
                RecordTargetMethodInvoke(dbParameter => dbParameter.Value = value);
            }
        }

        public override bool SourceColumnNullMapping
        {
            get => _dbParameter.SourceColumnNullMapping;
            set
            {
                _dbParameter.SourceColumnNullMapping = value;
                RecordTargetMethodInvoke(dbParameter => dbParameter.SourceColumnNullMapping = value);
            }
        }

        public override int Size
        {
            get => _dbParameter.Size;
            set
            {
                _dbParameter.Size = value;
                RecordTargetMethodInvoke(dbParameter => dbParameter.Size = value);
            }
        }

        public override string ToString()
        {
            return $"{ParameterName}:{Value}";
        }

        public DbParameter GetDbParameter()
        {
            return _dbParameter;
        }

        private event Action<DbParameter> OnRecorder;

        public void ReplyTargetMethodInvoke(DbParameter target)
        {
            OnRecorder?.Invoke(target);
        }

        public void RecordTargetMethodInvoke(Action<DbParameter> targetMethod)
        {
            OnRecorder += targetMethod;
        }
    }
}