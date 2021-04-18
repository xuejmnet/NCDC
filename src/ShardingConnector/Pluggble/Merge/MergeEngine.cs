using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.Rule;
using ShardingConnector.Executor;
using ShardingConnector.Kernels.MetaData.Schema;
using ShardingConnector.Merge;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.Pluggble.Merge
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
    
        public MergeEngine(ICollection<IBaseRule> rules, ConfigurationProperties properties, IDatabaseType databaseType, SchemaMetaData metaData) {
            this.rules = rules;
            merger = new MergeEntry();
            // merger = new MergeEntry(databaseType, metaData, properties);
        }
    
        /**
     * Merge.
     *
     * @param queryResults query results
     * @param sqlStatementContext SQL statement context
     * @return merged result
     * @throws SQLException SQL exception
     */
        public IMergedEnumerator Merge(List<IQueryEnumerator> queryResults, ISqlCommandContext<ISqlCommand> sqlCommandContext) {
            registerMergeDecorator();
        return merger.Process(queryResults, sqlCommandContext);
    }
    
    private void registerMergeDecorator() {
    // for (Class<? extends ResultProcessEngine> each : OrderedRegistry.getRegisteredClasses(ResultProcessEngine.class)) {
    // ResultProcessEngine processEngine = createProcessEngine(each);
    // Class<?> ruleClass = (Class<?>) processEngine.getType();
    // // FIXME rule.getClass().getSuperclass() == ruleClass for orchestration, should decouple extend between orchestration rule and sharding rule
    // rules.stream().filter(rule -> rule.getClass() == ruleClass || rule.getClass().getSuperclass() == ruleClass).collect(Collectors.toList())
    //     .forEach(rule -> merger.registerProcessEngine(rule, processEngine));
    // }
}
    
// private ResultProcessEngine createProcessEngine(final Class<? extends ResultProcessEngine> processEngine) {
//     try {
//         return processEngine.newInstance();
//     } catch (final InstantiationException | IllegalAccessException ex) {
//         throw new ShardingSphereException(String.format("Can not find public default constructor for result process engine `%s`", processEngine), ex);
//     }
// }
    }
}
