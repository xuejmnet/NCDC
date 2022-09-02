﻿using OpenConnector.CommandParserBinder.MetaData.Column;
using OpenConnector.CommandParserBinder.MetaData.Index;
using OpenConnector.NewConnector.DataSource;

namespace OpenConnector.CommandParserBinder.MetaData.Table
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
            using (var connection= dataSource.CreateConnection())
            {
                connection.Open();
                return new TableMetaData(ColumnMetaDataLoader.Load(connection, table, databaseType), IndexMetaDataLoader.Load(connection, table, databaseType));
            }
        }
    }
}