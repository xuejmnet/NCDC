using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using ShardingConnector.ParserBinder.MetaData.Column;

namespace ShardingConnector.ParserBinder.MetaData.Column
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/22 15:57:26
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ColumnMetaDataLoader
    {
        private ColumnMetaDataLoader()
        {

        }

        private static readonly string COLUMN_NAME = "COLUMN_NAME";

        private static readonly string DATA_TYPE = "DATA_TYPE";

        private static readonly string TYPE_NAME = "TYPE_NAME";

        /**
         * Load column meta data list.
         * 
         * @param connection connection
         * @param table table name
         * @param databaseType database type
         * @return column meta data list
         * @throws SQLException SQL exception
         */
        public static ICollection<ColumnMetaData> Load(DbConnection connection, string table, string databaseType)
        {
            if (!IsTableExist(connection, connection.Database, table, databaseType))
            {
                return new List<ColumnMetaData>(0);
            }
            ICollection<ColumnMetaData> result = new LinkedList<ColumnMetaData>();
            ICollection<string> primaryKeys = new LinkedList<string>();
            List<string> columnNames = new ArrayList<string>();
            List<int> columnTypes = new List<int>();
            List<string> columnTypeNames = new List<string>();
            List<bool> isPrimaryKeys = new List<bool>();
            bool isCaseSensitive = false;
            
            using (var dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = GenerateEmptyResultSQL(table, databaseType);
                DbDataReader dbDataReader = null;
                try
                {
                    dbDataReader = dbCommand.ExecuteReader();
                    var schemaTable = dbDataReader.GetSchemaTable();
                    isCaseSensitive = schemaTable.CaseSensitive;
                    var dbColumns = dbDataReader.GetColumnSchema().ToList();
                    foreach (var dbColumn in dbColumns)
                    {
                        if (dbColumn.IsIdentity.GetValueOrDefault())
                        {
                            primaryKeys.Add(dbColumn.ColumnName);
                        }
                    }

                }
                finally
                {
                    dbDataReader?.Close();
                }
            }

            try (ResultSet resultSet = connection.getMetaData().getColumns(connection.getCatalog(), JdbcUtil.getSchema(connection, databaseType), table, "%")) {
                while (resultSet.next())
                {
                    string columnName = resultSet.getstring(COLUMN_NAME);
                    columnTypes.add(resultSet.getInt(DATA_TYPE));
                    columnTypeNames.add(resultSet.getstring(TYPE_NAME));
                    isPrimaryKeys.add(primaryKeys.contains(columnName));
                    columnNames.add(columnName);
                }
            }
            try (ResultSet resultSet = connection.createStatement().executeQuery(generateEmptyResultSQL(table, databaseType))) {
                for (string each : columnNames)
                {
                    isCaseSensitives.add(resultSet.getMetaData().isCaseSensitive(resultSet.findColumn(each)));
                }
            }
            for (int i = 0; i < columnNames.size(); i++)
            {
                // TODO load auto generated from database meta data
                result.add(new ColumnMetaData(columnNames.get(i), columnTypes.get(i), columnTypeNames.get(i), isPrimaryKeys.get(i), false, isCaseSensitives.get(i)));
            }
            return result;
            }




    private static string GenerateEmptyResultSQL(string table, string databaseType)
            {
                // TODO consider add a getDialectDelimeter() interface in parse module
                string delimiterLeft;
                string delimiterRight;
                if ("MySql".Equals(databaseType) || "MariaDB".Equals(databaseType))
                {
                    delimiterLeft = "`";
                    delimiterRight = "`";
                }
                else if ("SqlServer".Equals(databaseType))
                {
                    delimiterLeft = "[";
                    delimiterRight = "]";
                }
                else
                {
                    delimiterLeft = "";
                    delimiterRight = "";
                }
                return $"SELECT * FROM {delimiterLeft}{table}{delimiterRight} WHERE 1 != 1";
            }

            private static bool IsTableExist(DbConnection connection, string catalog, string table, string databaseType)
            {
                try (ResultSet resultSet = connection.getMetaData().getTables(catalog, JdbcUtil.getSchema(connection, databaseType), table, null)) {
                return resultSet.next();
            }
            }

private static ICollection<string> LoadPrimaryKeys(DbConnection connection, string table, string databaseType)
            {
                ICollection<string> result = new HashSet<string>();
                using (var dbCommand = connection.CreateCommand())
                {
                    dbCommand.CommandText = $"select * from {table}";
                    var dbDataReader = dbCommand.ExecuteReader();
                    var dbColumns = dbDataReader.GetColumnSchema().ToList();
                }
                try (ResultSet resultSet = connection.getMetaData().getPrimaryKeys(connection.getCatalog(), JdbcUtil.getSchema(connection, databaseType), table)) {
                while (resultSet.next())
                {
                    result.add(resultSet.getstring(COLUMN_NAME));
                }
            }
            return result;
        }
    }
}
