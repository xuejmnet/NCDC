using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace OpenConnector.Spi.DataBase.DataBaseType.DataBaseDiscover
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 17:15:57
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public  class SqlServerDataBaseTypeDiscover:IDataBaseTypeDiscover
    {

        private readonly ISet<string> dbConnectionFullNames = new HashSet<string>(){"System.Data.SqlClient.SqlConnection", "Microsoft.Data.SqlClient.SqlConnection"};
        public bool Match(DbConnection connection)
        {
            var type = connection.GetType();
            return dbConnectionFullNames.Contains(type.FullName);
        }

        public string DataBaseTypeName => "SqlServer";
    }
}
