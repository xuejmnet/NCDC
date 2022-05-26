using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using ShardingConnector.AdoNet.AdoNet.Core;
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
    public abstract class AbstractDbConnection : DbConnection, IAdoMethodRecorder<DbConnection>
    {
        public event Action<DbConnection> OnRecorder;
        public readonly MultiValueDictionary<string, DbConnection> CachedConnections = new MultiValueDictionary<string, DbConnection>();
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
            _connections = new List<DbConnection>(CreateConnections(dataSourceName, connectionMode, dataSource, connectionSize));

            return _connections;
        }


        private List<DbConnection> CreateConnections(string dataSourceName, ConnectionModeEnum connectionMode, IDataSource dataSource, int connectionSize)
        {
            if (1 == connectionSize)
            {
                if (dataSource.IsDefault())
                {
                    return new List<DbConnection>(){GetDefaultDbConnection()};
                }
                var connection = CreateConnection(dataSourceName, dataSource);
                ReplyTargetMethodInvoke(connection);
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
            List<DbConnection> result = new List<DbConnection>(connectionSize){GetDefaultDbConnection()};
            for (int i = 0; i < connectionSize-1; i++)
            {
                try
                {
                    var connection = CreateConnection(dataSourceName, dataSource);
                    ReplyTargetMethodInvoke(connection);
                    result.Add(connection);
                }
                catch (Exception ex)
                {
                    foreach (var conn in result)
                    {
                        conn.Dispose();
                    }

                    throw new ShardingException($"Could't get {connectionSize} connections one time, partition succeed connection({result.Count}) have released!", ex);
                }
            }

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _connections?.ForEach(dbConnection=>dbConnection.Dispose());
        }

        protected void AssertSingleDataSource()
        {
            ShardingAssert.ShouldBeTrue(GetDataSourceMap().Count == 1, "multi data source not support");
        }

        /// <summary>
        /// 从新播放target的创建后的动作
        /// </summary>
        /// <param name="target"></param>
        public void ReplyTargetMethodInvoke(DbConnection target)
        {
            OnRecorder?.Invoke(target);
        }

        /// <summary>
        /// 记录target的动作
        /// </summary>
        /// <param name="targetMethod"></param>
        public void RecordTargetMethodInvoke(Action<DbConnection> targetMethod)
        {
            OnRecorder += targetMethod;
        }

        public abstract DbConnection GetDefaultDbConnection();
    }
}