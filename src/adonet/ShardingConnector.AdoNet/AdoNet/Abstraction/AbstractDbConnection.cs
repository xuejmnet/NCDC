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
    public abstract class AbstractDbConnection:DbConnection
    {
        public readonly MultiValueDictionary<string,DbConnection> cachedConnections = new MultiValueDictionary<string, DbConnection>();
        public AbstractDbConnection()
        {

        }
        protected abstract IDictionary<string, IDataSource> GetDataSourceMap();
        public  List<DbConnection> GetConnections(ConnectionModeEnum connectionMode, string dataSourceName,int connectionSize)
        {
            var dataSourceMap = GetDataSourceMap();
            if (!dataSourceMap.ContainsKey(dataSourceName))
                throw new ShardingException($"missing the data source name: '{dataSourceName}'");
            IDataSource dataSource = dataSourceMap[dataSourceName];
        ICollection<DbConnection> connections;
            lock (cachedConnections)
            {
                connections = cachedConnections.GetValues(dataSourceName,true);
            }
        List<DbConnection> result;
            if (connections.Count >= connectionSize) {
            result = new List<DbConnection>(connections).GetRange(0, connectionSize);
        } else if (connections.Any()) {
    result = new List<DbConnection>(connections);
    List<DbConnection> newConnections = createConnections(dataSourceName, connectionMode, dataSource, connectionSize - connections.size());
    result.addAll(newConnections);
    synchronized(cachedConnections)
    {
        cachedConnections.putAll(dataSourceName, newConnections);
    }
    } else
    {
    result = new ArrayList<>(createConnections(dataSourceName, connectionMode, dataSource, connectionSize));
    synchronized(cachedConnections) {
        cachedConnections.putAll(dataSourceName, result);
    }
    }
    return result;
}


        private List<Connection> createConnections(final String dataSourceName, final ConnectionMode connectionMode, final DataSource dataSource, final int connectionSize) throws SQLException
        {
            if (1 == connectionSize) {
            Connection connection = createConnection(dataSourceName, dataSource);
            replayMethodsInvocation(connection);
            return Collections.singletonList(connection);
        }
    if (ConnectionMode.CONNECTION_STRICTLY == connectionMode) {
    return createConnections(dataSourceName, dataSource, connectionSize);
}
synchronized(dataSource) {
    return createConnections(dataSourceName, dataSource, connectionSize);
}


}

    }
}
