using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.NewConnector.DataSource;

namespace ShardingConnector.ParserBinder.MetaData.Table
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/22 15:56:19
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TableMetaDataLoader
    {
        private TableMetaDataLoader()
        {
            
        }
        public static TableMetaData Load(IDataSource dataSource, string table, string databaseType)
        {
            using (var connection= dataSource.GetDbConnection())
            {
                return new TableMetaData(ColumnMetaDataLoader.load(connection, table, databaseType), IndexMetaDataLoader.load(connection, table, databaseType));
            }
        }
    }
    }
}
