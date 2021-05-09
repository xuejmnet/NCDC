using System.Collections.Generic;
using ShardingConnector.Api.Database.MetaData.Dialect;
using ShardingConnector.Spi.DataBase.DataBaseType;
using ShardingConnector.Spi.DataBase.MetaData;

namespace ShardingConnector.Api.Database.DatabaseType.Dialect
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/19 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public class Sql92DatabaseType:IDatabaseType
    {
        public string GetName()
        {
            return "Sql92";
        }

        public ICollection<string> GetAdoNetUrlPrefixAlias()
        {
            throw new System.NotImplementedException();
        }

        public IDataSourceMetaData GetDataSourceMetaData(string url, string username)
        {
            return new Sql92DataSourceMetaData();
        }
    }
}