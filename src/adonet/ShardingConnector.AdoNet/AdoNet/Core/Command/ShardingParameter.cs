using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace ShardingConnector.AdoNet.AdoNet.Core.Command
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/7/23 15:56:13
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ShardingParameter : DbParameter
    {
        private readonly DbParameter _dbParameter;

        public ShardingParameter(DbParameter dbParameter)
        {
            _dbParameter = dbParameter;
        }
        public override void ResetDbType()
        {
            _dbParameter.ResetDbType();
        }

        public override DbType DbType
        {
            get => _dbParameter.DbType;
            set => _dbParameter.DbType = value;
        }
        public override ParameterDirection Direction
        {
            get => _dbParameter.Direction;
            set => _dbParameter.Direction = value;
        }
        public override bool IsNullable
        {
            get => _dbParameter.IsNullable;
            set => _dbParameter.IsNullable = value;
        }
        public override string ParameterName
        {
            get => _dbParameter.ParameterName;
            set => _dbParameter.ParameterName = value;
        }
        public override string SourceColumn
        {
            get => _dbParameter.SourceColumn;
            set => _dbParameter.SourceColumn = value;
        }
        public override object Value
        {
            get => _dbParameter.Value;
            set => _dbParameter.Value = value;
        }
        public override bool SourceColumnNullMapping
        {
            get => _dbParameter.SourceColumnNullMapping;
            set => _dbParameter.SourceColumnNullMapping = value;
        }
        public override int Size
        {
            get => _dbParameter.Size;
            set => _dbParameter.Size = value;
        }
        public override string ToString()
        {
            return $"{ParameterName}:{Value}";
        }
    }
}
