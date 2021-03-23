using System;
using System.Data;
using System.Data.Common;
using ShardingConnector.Exceptions;

namespace ShardingConnector.ShardingAdoNet
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 21 March 2021 11:04:39
* @Email: 326308290@qq.com
*/
    public class ShardingCommand:DbCommand
    {
        public ShardingCommand(string commandText,ShardingConnection connection)
        {
            this.CommandText = commandText;
            this.DbConnection = connection;
        }
        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection DbConnection { get; set; }
        protected override DbParameterCollection DbParameterCollection { get; }
        protected override DbTransaction DbTransaction { get; set; }
        public override bool DesignTimeVisible { get; set; }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            if (string.IsNullOrWhiteSpace(this.CommandText))
                throw new ShardingException("sql command text null or empty");
            DbDataReader dataReader= new ShardingDataReader();


            return dataReader;
        }
    }
}