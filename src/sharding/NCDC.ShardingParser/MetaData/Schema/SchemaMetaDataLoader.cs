using System.Collections.Concurrent;
using System.Data.Common;
using NCDC.ShardingParser.MetaData.Column;
using NCDC.ShardingParser.MetaData.Index;
using NCDC.ShardingParser.MetaData.Table;
using OpenConnector.DataSource;
using OpenConnector.Exceptions;

namespace NCDC.ShardingParser.MetaData.Schema
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/23 13:05:54
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SchemaMetaDataLoader
    {
        private const string TABLE_TYPE = "TABLE";

        private const string TABLE_NAME = "TABLE_NAME";

        /**
         * Load schema meta data.
         *
         * @param dataSource data source
         * @param maxConnectionCount count of max connections permitted to use for this query
         * @param databaseType database type
         * @return schema meta data
         * @throws SQLException SQL exception
         */
        public static SchemaMetaData Load(IDataSource dataSource, int maxConnectionCount, string databaseType)
        {
            List<string> tableNames;
            using (var connection = dataSource.CreateConnection())
            {
                 connection.Open();
                tableNames = LoadAllTableNames(connection,databaseType);
            }

            Console.WriteLine($"Loading {tableNames.Count} tables' meta data.");
            if (tableNames.Count == 0)
                return new SchemaMetaData(new Dictionary<string, TableMetaData>(0));
            var groupInt = Math.Max(tableNames.Count / maxConnectionCount, 1);

            var tableGroups = tableNames.Select((o, i) => new
            {
                Index = i % groupInt,
                TableName = o
            }).GroupBy(o => o.Index).Select(o => o.Select(g=>g.TableName).ToList()).ToList();

            IDictionary<string, TableMetaData> tableMetaDataMap = 1 == tableGroups.Count
                    ? Load(dataSource.CreateConnection(), tableGroups[0], databaseType) : AsyncLoad(dataSource, maxConnectionCount, tableNames, tableGroups, databaseType);
            return new SchemaMetaData(tableMetaDataMap);
        }




        private static List<string> LoadAllTableNames(DbConnection connection, string databaseType)
        {
            if ("MySql".Equals(databaseType) || "MariaDB".Equals(databaseType))
            {
                return LoadMySqlAllTableNames(connection);
            }
            if ("SqlServer".Equals(databaseType))
            {
                return LoadSqlServerAllTableNames(connection);
            }

            throw new ShardingException($"not found data base:[{databaseType}]");
        }
        private const string Tables = "Tables";
        private const string MySQL_TABLE_SCHEMA = "TABLE_SCHEMA";
        private const string MySQL_TABLE_NAME = "TABLE_NAME";
        private static List<string> LoadMySqlAllTableNames(DbConnection connection)
        {
            var database = connection.Database;
            using (var dataTable = connection.GetSchema(Tables))
            {
                var result = new List<string>(dataTable.Rows.Count);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var schema = dataTable.Rows[i][MySQL_TABLE_SCHEMA];
                    if (database.Equals($"{schema}", StringComparison.OrdinalIgnoreCase))
                    {
                        var table = dataTable.Rows[i][MySQL_TABLE_NAME].ToString();
                        if (!IsSystemTable(table))
                        {
                            result.Add(table);
                        }
                    }
                }
                return result;
            }
        }
        private static List<string> LoadSqlServerAllTableNames(DbConnection connection)
        {
            ICollection<string> result = new LinkedList<string>();
            using (var dataTable = connection.GetSchema(Tables))
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var table = dataTable.Rows[i][TABLE_NAME].ToString();
                    if (!IsSystemTable(table))
                    {
                        result.Add(table);
                    }
                }
            }
            return result.ToList();
        }


        private static bool IsSystemTable(string table)
        {
            return table.Contains("$") || table.Contains("/");
        }


        private static IDictionary<string, TableMetaData> Load(DbConnection connection, ICollection<string> tables, string databaseType)
        {
            using (connection)
            {
                connection.Open();
                IDictionary<string, TableMetaData> result = new Dictionary<string, TableMetaData>();
                foreach (var table in tables)
                {
                    result.Add(table, new TableMetaData(ColumnMetaDataLoader.Load(connection, table, databaseType), IndexMetaDataLoader.Load(connection, table, databaseType)));


                }

                return result;
            }
        }

        private static IDictionary<string, TableMetaData> AsyncLoad(IDataSource dataSource, int maxConnectionCount, List<string> tableNames,
                                                List<List<string>> tableGroups, string databaseType)
        {
            ConcurrentDictionary<string, TableMetaData> result = new ConcurrentDictionary<string, TableMetaData>();
            var tasks = tableGroups.Select(o => Task.Run(() =>
            {
                var tableMetaData = Load(dataSource.CreateConnection(), o, databaseType);
                foreach (var entry in tableMetaData)
                {
                    result.TryAdd(entry.Key, entry.Value);
                }
            })).ToArray();
             Task.WaitAll(tasks);
            return result;
        }
    }
}
