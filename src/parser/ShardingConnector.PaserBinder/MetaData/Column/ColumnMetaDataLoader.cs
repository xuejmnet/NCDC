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

        private const string TABLE_SCHEMA = "Tables";
        private const string TABLE_NAME = "TABLE_NAME";

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
            if (!IsTableExist(connection, table))
            {
                return new List<ColumnMetaData>(0);
            }

            ICollection<ColumnMetaData> result = new LinkedList<ColumnMetaData>();
            bool isCaseSensitive = false;

            using (var dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = GenerateEmptyResultSQL(table, databaseType);
                DbDataReader dbDataReader = null;
                List<DbColumn> dbColumns = null;
                try
                {
                    dbDataReader = dbCommand.ExecuteReader();
                    using (var schemaTable = dbDataReader.GetSchemaTable())
                    {
                        isCaseSensitive = schemaTable.CaseSensitive;
                        dbColumns = dbDataReader.GetColumnSchema().ToList();
                        foreach (var dbColumn in dbColumns)
                        {
                            if (dbColumn.ColumnOrdinal.HasValue)
                            {
                                result.Add(new ColumnMetaData(dbColumn.ColumnName, dbColumn.ColumnOrdinal.Value, dbColumn.DataTypeName, dbColumn.IsIdentity.GetValueOrDefault(), dbColumn.IsAutoIncrement.GetValueOrDefault(), isCaseSensitive));
                            }
                        }
                    }

                }
                finally
                {
                    dbDataReader?.Close();
                    dbColumns?.Clear();
                }
            }

            return result.OrderBy(o => o.ColumnOrdinal).ToList();
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

        private static bool IsTableExist(DbConnection connection, string table)
        {
            using (var dataTable = connection.GetSchema(TABLE_SCHEMA))
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i][TABLE_NAME].Equals(table))
                        return true;
                }

                return false;
            }
        }

    }
}
