using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.Rule;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.Merge.Engine.Merger
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 10:37:59
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IResultMergerEngine<T>:IResultProcessEngine<T> where  T:IBaseRule
    {
        IResultMerger NewInstance(IDatabaseType databaseType, T rule, ConfigurationProperties properties, ISqlCommandContext<ISqlCommand> sqlCommandContext);

    }
}
