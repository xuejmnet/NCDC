using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ShardingConnector.Spi.DataBase.DataBaseType.DataBaseDiscover
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 9:58:11
    /// Email: 326308290@qq.com
    public class MySQLDataBaseTypeDiscover : IDataBaseTypeDiscover
    {
        private readonly ISet<string> dbConnectionFullNames = new HashSet<string>() { "MySqlConnector.MySqlConnection" };

        public MySQLDataBaseTypeDiscover()
        {
            
        }
        public bool Match(DbConnection connection)
        {
            var type = connection.GetType();
            return dbConnectionFullNames.Contains(type.FullName);
        }

        public string DataBaseTypeName => "MySql";
    }
}
