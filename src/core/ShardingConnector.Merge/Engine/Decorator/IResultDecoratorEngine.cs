using System;
using System.Collections.Generic;
using System.Text;

using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParserBinder.Command;
using ShardingConnector.CommandParserBinder.MetaData.Schema;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.Rule;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.Merge.Engine.Decorator
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 10:42:38
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IResultDecoratorEngine<T>:IResultProcessEngine<T> where T:IBaseRule
    {
        IDataReaderDecorator NewInstance(IDatabaseType databaseType, SchemaMetaData schemaMetaData, T rule, ConfigurationProperties properties, ISqlCommandContext<ISqlCommand> sqlCommandContext);

    }
}
