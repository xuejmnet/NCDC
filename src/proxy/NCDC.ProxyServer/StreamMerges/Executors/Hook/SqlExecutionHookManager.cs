using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using OpenConnector.ShardingAdoNet;
using OpenConnector.Spi.DataBase.MetaData;

namespace OpenConnector.Executor.Hook
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 8:32:45
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlExecutionHookManager
    {
        private static readonly SqlExecutionHookManager Instance;
        private readonly ICollection<ISqlExecutionHook> _sqlExecutionHooks = NewInstanceServiceLoader.NewServiceInstances<ISqlExecutionHook>();

        private SqlExecutionHookManager()
        {

        }
        static SqlExecutionHookManager()
        {
            NewInstanceServiceLoader.Register<ISqlExecutionHook>();
            Instance = new SqlExecutionHookManager();
        }

        public static SqlExecutionHookManager GetInstance()
        {
            return Instance;
        }
        public void Start(string dataSourceName, string sql, ParameterContext parameterContext, IDataSourceMetaData dataSourceMetaData, bool isTrunkThread, IDictionary<string, object> shardingExecuteDataMap)
        {
            foreach (var sqlExecutionHook in _sqlExecutionHooks)
            {
                sqlExecutionHook.Start(dataSourceName, sql, parameterContext, dataSourceMetaData, isTrunkThread, shardingExecuteDataMap);
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
