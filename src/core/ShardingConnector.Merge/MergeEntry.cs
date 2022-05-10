using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.Rule;
using ShardingConnector.Executor;
using ShardingConnector.Merge.Engine;
using ShardingConnector.Merge.Reader;
using ShardingConnector.Spi.DataBase.DataBaseType;
using System.Collections.Generic;

using ShardingConnector.CommandParser.Command;
using ShardingConnector.Merge.Engine.Decorator;
using ShardingConnector.Merge.Engine.Merger;
using ShardingConnector.Merge.Reader.Transparent;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;

namespace ShardingConnector.Merge
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Sunday, 18 April 2021 19:45:00
    * @Email: 326308290@qq.com
    */
    public sealed class MergeEntry
    {
        private readonly IDatabaseType databaseType;

        private readonly SchemaMetaData schemaMetaData;

        private readonly ConfigurationProperties properties;

        private readonly IDictionary<IBaseRule, IResultProcessEngine> engines = new Dictionary<IBaseRule, IResultProcessEngine>();

        public MergeEntry(IDatabaseType databaseType, SchemaMetaData schemaMetaData, ConfigurationProperties properties)
        {
            this.databaseType = databaseType;
            this.schemaMetaData = schemaMetaData;
            this.properties = properties;
        }

        /**
         * Register result process engine.
         *
         * @param rule rule
         * @param processEngine result process engine
         */
        public void RegisterProcessEngine(IBaseRule rule, IResultProcessEngine processEngine)
        {
            engines.Add(rule, processEngine);
        }

        /**
         * Process query results.
         *
         * @param queryResults query results
         * @param sqlStatementContext SQL statement context
         * @return merged result
         * @throws SQLException SQL exception
         */
        public IMergedEnumerator Process(List<IQueryEnumerator> queryResults, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            var mergedResult = Merge(queryResults, sqlCommandContext);
            IMergedEnumerator result = null;
            if (mergedResult != null)
            {
                 result = Decorate(mergedResult, sqlCommandContext);
            }
            else
            {
                result = Decorate(queryResults[0], sqlCommandContext);
            }
            return result??new TransparentMergedEnumerator(queryResults[0]);
        }

        private IMergedEnumerator Merge(List<IQueryEnumerator> queryResults, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            foreach (var engineEntry in engines)
            {
                
                if (engineEntry.Value is IResultMergerEngine resultMergerEngine)
                {
                    var resultMerger = resultMergerEngine.NewInstance(databaseType, engineEntry.Key, properties, sqlCommandContext);
                    return resultMerger.Merge(queryResults, sqlCommandContext, schemaMetaData);
                }
            }
            return null;
        }

        private IMergedEnumerator Decorate(IMergedEnumerator mergedResult, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            IMergedEnumerator result = null;
            foreach (var engineEntry in engines)
            {
                if (engineEntry.Key is IResultDecoratorEngine<IBaseRule> resultDecoratorEngine)
                {
                    var resultDecorator = resultDecoratorEngine.NewInstance(databaseType, schemaMetaData, engineEntry.Key, properties, sqlCommandContext);
                    result = null == result ? resultDecorator.Decorate(mergedResult, sqlCommandContext, schemaMetaData) : resultDecorator.Decorate(result, sqlCommandContext, schemaMetaData);

                }
            }
            return result ?? mergedResult;
        }

        private IMergedEnumerator Decorate(IQueryEnumerator queryResult, ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            IMergedEnumerator result = null;
            foreach (var engineEntry in engines)
            {
                if (engineEntry.Value is IResultDecoratorEngine<IBaseRule> resultDecoratorEngine)
                {
                    var resultDecorator = resultDecoratorEngine.NewInstance(databaseType, schemaMetaData, engineEntry.Key, properties, sqlCommandContext);
                    result = null == result ? resultDecorator.Decorate(queryResult, sqlCommandContext, schemaMetaData) : resultDecorator.Decorate(result, sqlCommandContext, schemaMetaData);
                }
            }
            return result;
        }

    }
}