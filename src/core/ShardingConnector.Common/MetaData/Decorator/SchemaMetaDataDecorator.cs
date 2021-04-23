using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Common.Rule;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.ParserBinder.MetaData.Table;

namespace ShardingConnector.Common.MetaData.Decorator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/23 17:03:50
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class SchemaMetaDataDecorator
    {
        private SchemaMetaDataDecorator() { }

        public static  SchemaMetaData Decorate<T>(SchemaMetaData schemaMetaData, T rule, ITableMetaDataDecorator<T> tableMetaDataDecorator) where T:IBaseRule
        {
            IDictionary<string, TableMetaData> result = new Dictionary<string, TableMetaData>(schemaMetaData.GetAllTableNames().Count);
            foreach (var tableName in schemaMetaData.GetAllTableNames())
            {
                result.Add(tableName, tableMetaDataDecorator.Decorate(schemaMetaData.Get(tableName), tableName, rule));
            }
            return new SchemaMetaData(result);
        }
    }
}
