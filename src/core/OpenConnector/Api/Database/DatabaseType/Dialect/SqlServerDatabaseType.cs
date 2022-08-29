using System.Collections.Generic;
using OpenConnector.Api.Database.MetaData.Dialect;
using OpenConnector.Spi.DataBase.DataBaseType;
using OpenConnector.Spi.DataBase.MetaData;

namespace OpenConnector.Api.Database.DatabaseType.Dialect
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