using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.Rule;
using ShardingConnector.Exceptions;
using ShardingConnector.Executor;
using ShardingConnector.Extensions;
using ShardingConnector.Merge;
using ShardingConnector.Merge.Engine;
using ShardingConnector.Merge.Reader;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.Spi.DataBase.DataBaseType;
using ShardingConnector.Spi.Order;

namespace ShardingConnector.Pluggable.Merge
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 14:47:21
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class MergeEngine
    {
        private readonly ICollection<IBaseRule> rules;

        private readonly MergeEntry merger;

        public MergeEngine(ICollection<IBaseRule> rules, ConfigurationProperties properties, IDatabaseType databaseType, SchemaMetaData metaData)
        {
            this.rules = rules;
            merger = new MergeEntry(databaseType, metaData, properties);
        }

        /**
     * Merge.
     *
     * @param queryResults query results
     * @param sqlStatementContext SQL statement context
     * @return merged result
     * @throws SQLException SQL exception
     */
        public IMergedEnumerator Merge(List<IQueryEnumerator> queryResults, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            RegisterMergeDecorator();
            return merger.Process(queryResults, sqlCommandContext);
        }

        private void RegisterMergeDecorator()
        {
            foreach (var orderAware in OrderedRegistry.GetRegisteredOrderedAware(typeof(IResultProcessEngine<>)))
            {
                var processEngine = CreateProcessEngine(orderAware.GetType());
                var ruleType = processEngine.GetType().GetGenericArguments(typeof(IBaseRule)).FirstOrDefault();
                rules.Where(o => ruleType.IsInstanceOfType(o)).ToList().ForEach(rule => merger.RegisterProcessEngine(rule, processEngine));
            }
        }

        private IResultProcessEngine<IBaseRule> CreateProcessEngine(Type type)
        {
            try
            {
                return (IResultProcessEngine<IBaseRule>)Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new ShardingException($"Can not find public default constructor for result process engine `{type}`", ex);
            }
        }
    }
}
