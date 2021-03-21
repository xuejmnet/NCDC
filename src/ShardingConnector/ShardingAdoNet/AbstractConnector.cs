using System.Data.Common;

namespace ShardingConnector.ShardingAdoNet
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 21 March 2021 10:54:51
* @Email: 326308290@qq.com
*/
    public class AbstractConnector
    {
        public AbstractConnector(string dataSourceName, string connectionString)
        {
            DataSourceName = dataSourceName;
            ConnectionString = connectionString;
        }

        public string DataSourceName { get; }
        public string ConnectionString { get; }
        public DbConnection Create()
        {
            return null;
        }
    }
}