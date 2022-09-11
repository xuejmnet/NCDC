using System;
using OpenConnector.Spi.DataBase.DataBaseType;
using OpenConnector.Spi.DataBase.MetaData;

/*
* @Author: xjm
* @Description:
* @Date: DATE
* @Email: 326308290@qq.com
*/
namespace OpenConnector.Api.Database.DatabaseType.Dialect
{
    public class MariaDBDatabaseType:IBranchDatabaseType
    {
        public string GetName()
        {
            return "MariaDB";
        }

        public IDataSourceMetaData GetDataSourceMetaData(string url)
        {
            throw new NotImplementedException();
        }

        public IDatabaseType GetTrunkDatabaseType()
        {
            return DatabaseTypes.GetActualDatabaseType("MySql");
        }
    }
}