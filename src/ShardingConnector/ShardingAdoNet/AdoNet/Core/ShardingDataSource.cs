namespace ShardingConnector.ShardingAdoNet.AdoNet.Core
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 10:38:48
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public  class ShardingDataSource
    {
        
        private  ShardingRuntimeContext runtimeContext;
    
        static {
            NewInstanceServiceLoader.register(RouteDecorator.class);
            NewInstanceServiceLoader.register(SQLRewriteContextDecorator.class);
            NewInstanceServiceLoader.register(ResultProcessEngine.class);
        }
    
        public ShardingDataSource(final Map<String, DataSource> dataSourceMap, final ShardingRule shardingRule, final Properties props) throws SQLException {
            super(dataSourceMap);
        checkDataSourceType(dataSourceMap);
        runtimeContext = new ShardingRuntimeContext(dataSourceMap, shardingRule, props, getDatabaseType());
    }
    
    private void checkDataSourceType(final Map<String, DataSource> dataSourceMap) {
    for (DataSource each : dataSourceMap.values()) {
    Preconditions.checkArgument(!(each instanceof MasterSlaveDataSource), "Initialized data sources can not be master-slave data sources.");
    }
}
    
public ShardingConnection getConnection() {
    return new ShardingConnection(getDataSourceMap(), runtimeContext, TransactionTypeHolder.get());
}
    }
}