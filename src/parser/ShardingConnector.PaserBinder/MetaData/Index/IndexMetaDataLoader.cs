using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ShardingConnector.ParserBinder.MetaData.Index
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/23 12:42:02
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class IndexMetaDataLoader
    {
        private const string INDEX_NAME = "INDEX_NAME";
        private const string TABLE_NAME = "TABLE_NAME";

        /**
     * Load column meta data list.
     * 
     * @param connection connection
     * @param table table name
     * @param databaseType databaseType
     * @return index meta data list
     * @throws SQLException SQL exception
     */
        public static ICollection<IndexMetaData> Load(DbConnection connection, string table, string databaseType)
        {
            ICollection<IndexMetaData> result = new HashSet<IndexMetaData>();
            using (var dataTable = connection.GetSchema("IndexColumns"))
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i][TABLE_NAME].Equals(table))
                    {
                        result.Add(new IndexMetaData(dataTable.Rows[i][INDEX_NAME].ToString()));
                    }
                }
            }

            return result;
        }
    }
}
