// using System;
// using System.Collections.Generic;
// using System.Text;
// using Microsoft.Extensions.Caching.Memory;
//
// using ShardingConnector.CommandParser.Command;
//
// namespace ShardingConnector.ParserEngine.Cache
// {
//     /*
//     * @Author: xjm
//     * @Description:
//     * @Date: 2021/4/20 11:37:19
//     * @Ver: 1.0
//     * @Email: 326308290@qq.com
//     */
//     public sealed class SqlParseResultCache
//     {
//         private readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions() {  });
//         private readonly object sLock = new object();
//
//         /**
//          * Put SQL and parse result into cache.
//          * 
//          * @param sql SQL
//          * @param sqlStatement SQL statement
//          */
//         public void Add(string sql, ISqlCommand sqlCommand)
//         {
//             cache.Set(sql, sqlCommand,new MemoryCacheEntryOptions()
//             {
//                 SlidingExpiration = TimeSpan.FromMinutes(30)
//             });
//         }
//
//         /**
//          * Get SQL statement.
//          *
//          * @param sql SQL
//          * @return SQL statement
//          */
//         public ISqlCommand GetSqlCommand(string sql)
//         {
//             return cache.Get<ISqlCommand>(sql);
//         }
//
//     }
// }
