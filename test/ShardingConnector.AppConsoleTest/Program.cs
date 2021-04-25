using Microsoft.Data.SqlClient;
using ShardingConnector.Api.Database.DatabaseType.Dialect;
using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.AdoNet.AdoNet.Core.Connection;
using ShardingConnector.AdoNet.AdoNet.Core.Context;
using ShardingConnector.AdoNet.AdoNet.Core.DataSource;
using ShardingConnector.ShardingApi.Api.Config.Sharding;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.SqlServerParser;

namespace ShardingConnector.AppConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var type = typeof(SqlServerParserConfiguration);
            //var dbProviderFactory = ShardingCreateDbProviderFactory.CreateDataSource(dataSourceMap, new ShardingRuleConfiguration(),
            //    new Dictionary<string, object>());
            var dataSourceMap = new Dictionary<string, IDataSource>()
            {
                {
                    "SqlServer",
                    new GenericDataSource(SqlClientFactory.Instance,
                        "Data Source=localhost;Initial Catalog=ShardingCoreDB123;Integrated Security=True")
                }
            };
            var dbConnection = new ShardingConnection(dataSourceMap,
                new ShardingRuntimeContext(dataSourceMap, new ShardingRule(new ShardingRuleConfiguration(), dataSourceMap.Select(o=>o.Key).ToList()), new Dictionary<string, object>(),
                    new SqlServerDatabaseType()), TransactionTypeEnum.LOCAL);


            var dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "select * from SysUserMod_00";
            var dbDataReader = dbCommand.ExecuteReader();
            while (dbDataReader.Read())
            {
                Console.WriteLine($"{dbDataReader[0]}-{dbDataReader[1]}-{dbDataReader[2]}");
            }
            Console.WriteLine("Hello World!");
        }
    }
}