﻿using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;

namespace NCDC.ProxyServer.Executors
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/3/22 17:16:21
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingExecutionContext
    {
        private readonly ISqlCommandContext<ISqlCommand> _sqlCommandContext;

        private readonly ICollection<ExecutionUnit> _executionUnits;
        public int MaxQueryConnectionsLimit { get; }
        public bool IsSerialExecute { get; }

        public ShardingExecutionContext(ISqlCommandContext<ISqlCommand> sqlCommandContext,ExecutionUnit executionUnit):this(sqlCommandContext,new List<ExecutionUnit>(){executionUnit})
        {}

        public ShardingExecutionContext(ISqlCommandContext<ISqlCommand> sqlCommandContext,List<ExecutionUnit> executionUnits)
        {
            _sqlCommandContext = sqlCommandContext;
            _executionUnits = executionUnits;
            MaxQueryConnectionsLimit = 10;
            IsSerialExecute = sqlCommandContext is InsertCommandContext||sqlCommandContext is UpdateCommandContext ||sqlCommandContext is DeleteCommandContext;
        }

        public ISqlCommandContext<ISqlCommand> GetSqlCommandContext()
        {
            return _sqlCommandContext;
        }

        public ICollection<ExecutionUnit> GetExecutionUnits()
        {
            return _executionUnits;
        }

        public bool IsSequenceQuery()
        {
            return false;
        }
    }
}
