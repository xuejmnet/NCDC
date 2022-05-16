using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using ShardingConnector.Base;
using ShardingConnector.Exceptions;
using ShardingConnector.Executor.Constant;
using ShardingConnector.Extensions;
using ShardingConnector.NewConnector.DataSource;

namespace ShardingConnector.AdoNet.AdoNet.Abstraction
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/24 14:39:10
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public abstract class AbstractDbConnection : DbConnection
    {
        public readonly MultiValueDictionary<string, DbConnection> cachedConnections = new MultiValueDictionary<string, DbConnection>();
        public abstract IDictionary<string, IDataSource> GetDataSourceMap();
        public abstract DbConnection CreateConnection(string dataSourceName, IDataSource dataSource);
        private List<DbConnection> _connections;
        public List<DbConnection> GetConnections(ConnectionModeEnum connectionMode, string dataSourceName, int connectionSize)
        {
            var dataSourceMap = GetDataSourceMap();
            if (!dataSourceMap.ContainsKey(dataSourceName))
                throw new ShardingException($"missing the data source name: '{dataSourceName}'");
            IDataSource dataSource = dataSourceMap[dataSourceName];
            //ICollection<DbConnection> connections;
            //lock (cachedConnections)
            //{
            //    connections = cachedConnections.GetValues(dataSourceName, true);
            //}
            //List<DbConnection> result;
            //if (connections.Count >= connectionSize)
            //{
            //    result = new List<DbConnection>(connections).GetRange(0, connectionSize);
            //}
            //else if (connections.Any())
            //{
            //    result = new List<DbConnection>(connections);
            //    List<DbConnection> newConnections = CreateConnections(dataSourceName, connectionMode, dataSource, connectionSize - connections.Count);
            //    result.AddRange(newConnections);
            //    lock (cachedConnections)
            //    {
            //        cachedConnections.AddRange(dataSourceName, newConnections);
            //    }
            //}
            //else
            //{
            //    result = new List<DbConnection>(CreateConnections(dataSourceName, connectionMode, dataSource, connectionSize));
            //    lock (cachedConnections)
            //    {
            //        cachedConnections.AddRange(dataSourceName, result);
            //    }
            //}
            _connections= new List<DbConnection>(CreateConnections(dataSourceName, connectionMode, dataSource, connectionSize));
            return _connections;
        }


        private List<DbConnection> CreateConnections(string dataSourceName, ConnectionModeEnum connectionMode, IDataSource dataSource, int connectionSize)
        {
            if (1 == connectionSize)
            {
                var connection = CreateConnection(dataSourceName, dataSource);
                //replayMethodsInvocation(connection);
                return new List<DbConnection>() { connection };
            }
            if (ConnectionModeEnum.CONNECTION_STRICTLY == connectionMode)
            {
                return CreateConnections(dataSourceName, dataSource, connectionSize);
            }

            lock (dataSource)
            {
                return CreateConnections(dataSourceName, dataSource, connectionSize);
            }


        }
        private List<DbConnection> CreateConnections(string dataSourceName, IDataSource dataSource, int connectionSize)
        {
            List<DbConnection> result = new List<DbConnection>(connectionSize);
            for (int i = 0; i < connectionSize; i++)
            {
                try
                {
                    var connection = CreateConnection(dataSourceName, dataSource);
                    //replayMethodsInvocation(connection);
                    result.Add(connection);
                }
                catch (Exception ex)
                {
                    foreach (var conn in result)
                    {
                        conn.Close();
                    }
                    throw new ShardingException($"Could't get {connectionSize} connections one time, partition succeed connection({result.Count}) have released!", ex);
                }
            }
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            foreach (var dbConnection in _connections)
            {
                dbConnection.Dispose();
            }
        }
    }
}
