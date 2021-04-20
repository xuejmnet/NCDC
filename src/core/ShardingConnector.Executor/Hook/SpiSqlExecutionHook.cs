using System;
using System.Collections.Generic;
using ShardingConnector.Spi.DataBase.MetaData;

namespace ShardingConnector.Executor.Hook
{
/*
* @Author: xjm
* @Description:
* @Date: Thursday, 15 April 2021 20:18:33
* @Email: 326308290@qq.com
*/
    public class SpiSqlExecutionHook:ISqlExecutionHook
    {
        
        private readonly ICollection<ISqlExecutionHook> _sqlExecutionHooks = NewInstanceServiceLoader.NewServiceInstances<ISqlExecutionHook>();
    
        static SpiSqlExecutionHook(){
            NewInstanceServiceLoader.Register<ISqlExecutionHook>();
        }
    
        public void Start(string dataSourceName, string sql, List<object> parameters, IDataSourceMetaData dataSourceMetaData, bool isTrunkThread, IDictionary<string, object> shardingExecuteDataMap)
        {
            foreach (var sqlExecutionHook in _sqlExecutionHooks)
            {
                sqlExecutionHook.Start(dataSourceName,sql,parameters,dataSourceMetaData,isTrunkThread,shardingExecuteDataMap);
            }
        }

        public void FinishSuccess()
        {
            foreach (var sqlExecutionHook in _sqlExecutionHooks)
            {
                sqlExecutionHook.FinishSuccess();
            }
        }

        public void FinishFailure(Exception exception)
        {
            foreach (var sqlExecutionHook in _sqlExecutionHooks)
            {
                sqlExecutionHook.FinishFailure(exception);
            }
        }
    }
}