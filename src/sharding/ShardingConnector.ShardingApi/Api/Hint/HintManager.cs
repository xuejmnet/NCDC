using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using ShardingConnector.Base;
using ShardingConnector.Extensions;

namespace ShardingConnector.ShardingApi.Api.Hint
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/29 10:56:38
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class HintManager : IDisposable
    {
        private static readonly AsyncLocal<HintManager> HINT_MANAGER_HOLDER = new AsyncLocal<HintManager>();

        private readonly MultiValueDictionary<String, IComparable> databaseShardingValues =
            new MultiValueDictionary<string, IComparable>();

        private readonly MultiValueDictionary<String, IComparable> tableShardingValues =
            new MultiValueDictionary<string, IComparable>();

        private bool databaseShardingOnly;

        private bool masterRouteOnly;

        /**
         * Get a new instance for {@code HintManager}.
         *
         * @return  {@code HintManager} instance
         */
        public static HintManager GetInstance()
        {
            ShardingAssert.Else(null == HINT_MANAGER_HOLDER.Value, "Hint has previous value, please clear first.");
            HintManager result = new HintManager();
            HINT_MANAGER_HOLDER.Value = result;
            return result;
        }

        /**
         * Set sharding value for database sharding only.
         *
         * <p>The sharding operator is {@code =}</p>
         *
         * @param value sharding value
         */
        public void SetDatabaseShardingValue(IComparable value)
        {
            databaseShardingValues.Clear();
            tableShardingValues.Clear();
            databaseShardingValues.Add("", value);
            databaseShardingOnly = true;
        }

        /**
         * Add sharding value for database.
         *
         * <p>The sharding operator is {@code =}</p>
         *
         * @param logicTable logic table name
         * @param value sharding value
         */
        public void AddDatabaseShardingValue(String logicTable, IComparable value)
        {
            if (databaseShardingOnly)
            {
                databaseShardingValues.Remove("");
            }
            databaseShardingValues.Add(logicTable, value);
            databaseShardingOnly = false;
        }

        /**
         * Add sharding value for table.
         *
         * <p>The sharding operator is {@code =}</p>
         *
         * @param logicTable logic table name
         * @param value sharding value
         */
        public void AddTableShardingValue(String logicTable, IComparable value)
        {
            if (databaseShardingOnly)
            {
                databaseShardingValues.Remove("");
            }
            tableShardingValues.Add(logicTable, value);
            databaseShardingOnly = false;
        }

        /**
         * Get database sharding values.
         *
         * @return database sharding values
         */
        public static ICollection<IComparable> GetDatabaseShardingValues()
        {
            return GetDatabaseShardingValues("");
        }

        /**
         * Get database sharding values.
         *
         * @param logicTable logic table
         * @return database sharding values
         */
        public static ICollection<IComparable> GetDatabaseShardingValues(String logicTable)
        {
            return null == HINT_MANAGER_HOLDER.Value ? new HashSet<IComparable>() : HINT_MANAGER_HOLDER.Value.databaseShardingValues.GetValues(logicTable, true);
        }

        /**
         * Get table sharding values.
         *
         * @param logicTable logic table name
         * @return table sharding values
         */
        public static ICollection<IComparable> GetTableShardingValues(String logicTable)
        {
            return null == HINT_MANAGER_HOLDER.Value ? new HashSet<IComparable>() : HINT_MANAGER_HOLDER.Value.tableShardingValues.GetValues(logicTable, true);
        }

        /**
         * Judge whether database sharding only.
         *
         * @return database sharding or not
         */
        public static bool IsDatabaseShardingOnly()
        {
            return null != HINT_MANAGER_HOLDER.Value && HINT_MANAGER_HOLDER.Value.databaseShardingOnly;
        }

        /**
         * Set database operation force route to master database only.
         */
        public void SetMasterRouteOnly()
        {
            masterRouteOnly = true;
        }

        /**
         * Judge whether route to master database only or not.
         *
         * @return route to master database only or not
         */
        public static bool IsMasterRouteOnly()
        {
            return null != HINT_MANAGER_HOLDER.Value && HINT_MANAGER_HOLDER.Value.masterRouteOnly;
        }

        /**
         * Clear threadlocal for hint manager.
         */
        public static void Clear()
        {
            HINT_MANAGER_HOLDER.Value = null;
        }


        public void Dispose()
        {
            HintManager.Clear();
        }
    }
}
