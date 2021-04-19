using System.Collections.Generic;
using ShardingConnector.Spi.DataBase.DataBaseType;
using ShardingConnector.Spi.DataBase.MetaData;

namespace ShardingConnector.Api.Database.DatabaseType.Dialect
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/19 11:06:53
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class MySqlDataSourceMetaData:IDatabaseType
    {
        public string GetName()
        {
            throw new System.NotImplementedException();
        }

        public ICollection<string> GetAdoNetUrlPrefixAlias()
        {
            throw new System.NotImplementedException();
        }

        public IDataSourceMetaData GetDataSourceMetaData(string url, string username)
        {
            throw new System.NotImplementedException();
        }
    }
}