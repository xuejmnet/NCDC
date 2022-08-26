using System.Data.Common;
using ShardingConnector.NewConnector.DataSource;

namespace ShardingConnector.AdoNet.AdoNet.Core.DataSource
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/19 13:17:30
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class GenericDataSource : IDataSource
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly string _connectionString;
        private readonly bool _isDefault;

        public GenericDataSource(string dataSourceName,DbProviderFactory dbProviderFactory, string connectionString) : this(dataSourceName,dbProviderFactory, connectionString, false)
        {
        }
        public GenericDataSource(string dataSourceName,DbProviderFactory dbProviderFactory, string connectionString, bool isDefault)
        {
            DataSourceName = dataSourceName;
            _dbProviderFactory = dbProviderFactory;
            _connectionString = connectionString;
            _isDefault = isDefault;
        }

        public string DataSourceName { get; }

        public bool IsDefault()
        {
            return _isDefault;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

        public DbProviderFactory GetDbProviderFactory()
        {
            return _dbProviderFactory;
        }

        public DbConnection CreateConnection()
        {
            var dbConnection = _dbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            return dbConnection;
        }
    }
}
