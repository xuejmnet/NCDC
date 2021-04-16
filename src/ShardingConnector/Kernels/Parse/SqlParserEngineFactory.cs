using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ShardingConnector.Kernels.Parse
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public sealed class SqlParserEngineFactory
    {
        private SqlParserEngineFactory(){}
        private static readonly ConcurrentDictionary<string, SqlParserEngine> ENGINES = new ConcurrentDictionary<string, SqlParserEngine>();
    
        /**
     * Get SQL parser engine.
     *
     * @param databaseTypeName name of database type
     * @return SQL parser engine
     */
        public static SqlParserEngine GetSQLParserEngine(string databaseTypeName) {
            if (ENGINES.TryGetValue(databaseTypeName, out var sqlParserEngine1))
            {
                return sqlParserEngine1;
            }

            lock (ENGINES)
            {
                if (ENGINES.TryGetValue(databaseTypeName, out var sqlParserEngine2))
                {
                    return sqlParserEngine2;
                }
                var result = new SqlParserEngine(databaseTypeName);
                ENGINES.TryAdd(databaseTypeName, result);
                return result;
            }
        }
    }
}