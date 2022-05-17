using System.Collections.Generic;
using ShardingConnector.Api.Database.MetaData.Dialect;
using ShardingConnector.Spi.DataBase.DataBaseType;
using ShardingConnector.Spi.DataBase.MetaData;

namespace ShardingConnector.Api.Database.DatabaseType.Dialect
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 12:14:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class SqlServerDatabaseType:IDatabaseType
    {
        public string GetName()
        {
            return "SqlServer";
        }
        public IDataSourceMetaData GetDataSourceMetaData(string url)
        {
           return new SqlServerDataSourceMetaData(url);
        }
    }
}