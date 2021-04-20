namespace ShardingConnector.NewConnector.DataSource.MetaData
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/19 13:19:31
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class SqlServerDataSourceMetaData:IDataSourceMetaData
    {
        private readonly string _connectionString;

        public SqlServerDataSourceMetaData(string connectionString)
        {
            _connectionString = connectionString;
        }
        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
