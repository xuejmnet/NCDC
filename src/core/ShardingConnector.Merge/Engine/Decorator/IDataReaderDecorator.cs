using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParserBinder.Command;
using ShardingConnector.CommandParserBinder.MetaData.Schema;

namespace ShardingConnector.Merge.Engine.Decorator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 10:41:25
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IDataReaderDecorator
    {
        /**
     * Decorate query result.
     *
     * @param queryResult query result
     * @param sqlStatementContext SQL statement context
     * @param schemaMetaData schema meta data
     * @return merged result
     * @throws SQLException SQL exception
     */
        IStreamDataReader Decorate(IStreamDataReader streamDataReader, ISqlCommandContext<ISqlCommand> sqlCommandContext, SchemaMetaData schemaMetaData);
    }
}