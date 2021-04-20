using Microsoft.Data.SqlClient;
using ShardingConnector.Api.Database.DatabaseType.Dialect;
using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.NewConnector.DataSource.Dialect;
using ShardingConnector.Transaction;
using System;
using System.Collections.Generic;
using ShardingConnector.AdoNet.AdoNet.Core.Connection;
using ShardingConnector.AdoNet.AdoNet.Core.Context;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.AppConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var dbProviderFactory = ShardingCreateDbProviderFactory.CreateDataSource(dataSourceMap, new ShardingRuleConfiguration(),
            //    new Dictionary<string, object>());
            var dataSourceMap = new Dictionary<string, IDataSource>()
            {
                {
                    "SqlServer",
                    new SqlServerDataSource(SqlClientFactory.Instance,
                        "Data Source=localhost;Initial Catalog=ShardingCoreDB123;Integrated Security=True")
                }
            };
            var dbConnection = new ShardingConnection(dataSourceMap,
                new ShardingRuntimeContext(dataSourceMap, new ShardingRule(), new Dictionary<string, object>(),
                    new SqlServerDatabaseType()), TransactionTypeEnum.LOCAL);


            var dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "select * from SysUserMod";
            var dbDataReader = dbCommand.ExecuteReader();
            Console.WriteLine("Hello World!");
        }
    }
}