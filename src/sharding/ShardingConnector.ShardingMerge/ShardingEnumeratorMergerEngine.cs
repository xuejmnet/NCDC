using System;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DAL.Dialect;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Merge.Engine.Merger;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingMerge.DAL;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.ShardingMerge
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 05 May 2021 21:16:33
* @Email: 326308290@qq.com
*/
    public class ShardingEnumeratorMergerEngine:IResultMergerEngine<ShardingRule>
    {
        public IResultMerger NewInstance(IDatabaseType databaseType, ShardingRule rule, ConfigurationProperties properties, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {  
            if (sqlCommandContext is SelectCommandContext) {
                return new ShardingDQLResultMerger(databaseType);
            } 
            if (sqlCommandContext.GetSqlCommand() is DALCommand) {
                return new ShardingDALEnumeratorMerger(rule);
            }
            return new TransparentResultMerger();
        }

        public int GetOrder()
        {
            return 0;
        }

        public Type GetGenericType()
        {
            return typeof(ShardingRule);
        }
    }
}