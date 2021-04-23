using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Common.Rule;
using ShardingConnector.ParserBinder.MetaData.Table;

namespace ShardingConnector.Common.MetaData.Decorator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/23 17:02:48
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface ITableMetaDataDecorator<T> where T:IBaseRule
    {

        TableMetaData Decorate(TableMetaData tableMetaData, string tableName, T rule);
    }
}
