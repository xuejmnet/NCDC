using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using OpenConnector.Exceptions;
using OpenConnector.Spi.DataBase.DataBaseType;
using OpenConnector.Spi.DataBase.DataBaseType.DataBaseDiscover;

namespace OpenConnector.Api.Database.DatabaseType
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 11:33:11
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class DatabaseTypes
    {

        private static readonly IDictionary<string, IDatabaseType> DATABASE_TYPES = new Dictionary<string, IDatabaseType>();

        /// <summary>
        /// 默认自动注册
        /// </summary>
        static DatabaseTypes()
        {
            var databaseTypes = ServiceLoader.Load<IDatabaseType>();
            foreach (var databaseType in databaseTypes)
            {
                DATABASE_TYPES.Add(databaseType.GetName(), databaseType);
            }

        }

        
        /// <summary>
        /// 获取数据库类型名称可能存在分支数据库
        /// 比如MariaDB实际使用mysql解析引擎
        /// </summary>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public static string GetTrunkDatabaseTypeName(IDatabaseType databaseType)
        {
            return databaseType is IBranchDatabaseType branchDatabaseType ? branchDatabaseType.GetTrunkDatabaseType().GetName() : databaseType.GetName();
        }

        /**
         * Get trunk database type.
         *
         * @param name database name 
         * @return trunk database type
         */
        public static IDatabaseType GetTrunkDatabaseType(string name)
        {
            return DATABASE_TYPES[name] is IBranchDatabaseType branchDatabaseType ? branchDatabaseType.GetTrunkDatabaseType() : GetActualDatabaseType(name);
        }

        /**
         * Get actual database type.
         *
         * @param name database name 
         * @return actual database type
         */
        public static IDatabaseType GetActualDatabaseType(string name)
        {
            var type = DATABASE_TYPES[name];
            if (type == null)
                throw new ShardingException($"Unsupported database:'{name}'");
            return type;
        }

        public static IDatabaseType GetDataBaseTypeByDbConnection(DbConnection connection)
        {
            var name = DataBaseTypeDiscoverManager.GetInstance().MatchName(connection);
            if (name == null)
                return DATABASE_TYPES["Sql92"];

            return DATABASE_TYPES.Where(o => o.Value.GetName() == name).Select(o => o.Value).FirstOrDefault() ?? DATABASE_TYPES["Sql92"];
        }

    }
}