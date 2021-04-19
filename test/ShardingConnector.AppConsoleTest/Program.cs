using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using ShardingConnector.Api.Config.Sharding;
using ShardingConnector.ShardingAdoNet.Api;

namespace ShardingConnector.AppConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataSourceMap = new Dictionary<string, DbProviderFactory>() {{"sqlserver", SqlClientFactory.Instance}};
            var dbProviderFactory = ShardingCreateDbProviderFactory.CreateDataSource(dataSourceMap, new ShardingRuleConfiguration(),
                new Dictionary<string, object>());

            var dbConnection = dbProviderFactory.CreateConnection();
            dbConnection.ConnectionString =
                "Data Source=localhost;Initial Catalog=ShardingCoreDB123;Integrated Security=True";
            var dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "select * from SysUserMod";
            var dbDataReader = dbCommand.ExecuteReader();
            Console.WriteLine("Hello World!");
        }
    }
}