using System.Data.Common;
using ShardingConnector.NewConnector.DataSource.MetaData;

namespace ShardingConnector.NewConnector.DataSource.Dialect
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/19 13:17:30
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlServerDataSource:IDataSource
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly string _connectionString;

        public SqlServerDataSource(DbProviderFactory dbProviderFactory,string connectionString)
        {
            _dbProviderFactory = dbProviderFactory;
            _connectionString = connectionString;
        }
        public string GetName()
        {
            return "SqlServer";
        }

        public IDataSourceMetaData GetSourceMetaData()
        {
            return new SqlServerDataSourceMetaData(_connectionString);
        }

        public DbConnection GetDbConnection()
        {
            var dbConnection = _dbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            return dbConnection;
        }
    }
}
