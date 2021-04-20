using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.Rule;
using ShardingConnector.Executor;
using ShardingConnector.Merge.Engine;
using ShardingConnector.Merge.Reader;
using ShardingConnector.Spi.DataBase.DataBaseType;
using System.Collections.Generic;
using ShardingConnector.CommandParser.Command;
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
    
    private readonly IDictionary<IBaseRule, IResultProcessEngine<IBaseRule>> engines = new Dictionary<IBaseRule, IResultProcessEngine<IBaseRule>>();
    
    // /**
    //  * Register result process engine.
    //  *
    //  * @param rule rule
    //  * @param processEngine result process engine
    //  */
    // public void registerProcessEngine(final BaseRule rule, final ResultProcessEngine processEngine) {
    //     engines.put(rule, processEngine);
    // }
    //
    // /**
    //  * Process query results.
    //  *
    //  * @param queryResults query results
    //  * @param sqlStatementContext SQL statement context
    //  * @return merged result
    //  * @throws SQLException SQL exception
    //  */
    public IMergedEnumerator Process(List<IQueryEnumerator> queryResults, ISqlCommandContext<ISqlCommand> sqlCommandContext) {
        // var mergedResult = Merge(queryResults, sqlCommandContext);
        // var result = mergedResult.isPresent() ? Optional.of(decorate(mergedResult.get(), sqlStatementContext)) : decorate(queryResults.get(0), sqlStatementContext);
        // return result.orElseGet(() -> new TransparentMergedResult(queryResults.get(0)));
        return null;
    }
    //
    // private IMergedEnumerator Merge(List<IQueryEnumerator> queryResults, ISqlCommandContext<ISqlCommand> sqlCommandContext) {
    //     for (Entry<BaseRule, ResultProcessEngine> entry : engines.entrySet()) {
    //         if (entry.getValue() instanceof ResultMergerEngine) {
    //             ResultMerger resultMerger = ((ResultMergerEngine) entry.getValue()).newInstance(databaseType, entry.getKey(), properties, sqlStatementContext);
    //             return Optional.of(resultMerger.merge(queryResults, sqlStatementContext, schemaMetaData));
    //         }
    //     }
    //     foreach (var VARIABLE in COLLECTION)
    //     {
    //         
    //     }
    //     return null;
    // }
    //
    // private MergedResult decorate(final MergedResult mergedResult, final SQLStatementContext sqlStatementContext) throws SQLException {
    //     MergedResult result = null;
    //     for (Entry<BaseRule, ResultProcessEngine> entry : engines.entrySet()) {
    //         if (entry.getValue() instanceof ResultDecoratorEngine) {
    //             ResultDecorator resultDecorator = ((ResultDecoratorEngine) entry.getValue()).newInstance(databaseType, schemaMetaData, entry.getKey(), properties, sqlStatementContext);
    //             result = null == result ? resultDecorator.decorate(mergedResult, sqlStatementContext, schemaMetaData) : resultDecorator.decorate(result, sqlStatementContext, schemaMetaData);
    //         }
    //     }
    //     return null == result ? mergedResult : result;
    // }
    //
    // private Optional<MergedResult> decorate(final QueryResult queryResult, final SQLStatementContext sqlStatementContext) throws SQLException {
    //     MergedResult result = null;
    //     for (Entry<BaseRule, ResultProcessEngine> entry : engines.entrySet()) {
    //         if (entry.getValue() instanceof ResultDecoratorEngine) {
    //             ResultDecorator resultDecorator = ((ResultDecoratorEngine) entry.getValue()).newInstance(databaseType, schemaMetaData, entry.getKey(), properties, sqlStatementContext);
    //             result = null == result ? resultDecorator.decorate(queryResult, sqlStatementContext, schemaMetaData) : resultDecorator.decorate(result, sqlStatementContext, schemaMetaData);
    //         }
    //     }
    //     return Optional.ofNullable(result);
    // }
        
    }
}