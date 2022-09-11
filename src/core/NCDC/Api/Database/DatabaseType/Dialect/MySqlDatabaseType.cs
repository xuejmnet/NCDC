using System.Collections.Generic;
using OpenConnector.Api.Database.MetaData.Dialect;
using OpenConnector.Spi.DataBase.DataBaseType;
using OpenConnector.Spi.DataBase.MetaData;

namespace OpenConnector.Api.Database.DatabaseType.Dialect
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
    public class MySqlDatabaseType:IDatabaseType
    {
        public string GetName()
        {
            return "MySql";
        }

        public IDataSourceMetaData GetDataSourceMetaData(string url)
        {
            return new MySqlDataSourceMetaData(url);
        }
    }
}