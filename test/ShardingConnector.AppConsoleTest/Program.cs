using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using ShardingConnector.Api.Config.Sharding;
using ShardingConnector.Api.Database.DatabaseType.Dialect;
using ShardingConnector.Api.DataSource;
using ShardingConnector.Api.DataSource.Dialect;
using ShardingConnector.Core.Rule;
using ShardingConnector.ShardingAdoNet;
using ShardingConnector.ShardingAdoNet.AdoNet.Core.Context;
using ShardingConnector.ShardingAdoNet.Api;
using ShardingConnector.Transaction;

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