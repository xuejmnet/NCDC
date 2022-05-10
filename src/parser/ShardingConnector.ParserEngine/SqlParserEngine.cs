using System;
using Antlr4.Runtime.Tree;

using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserEngine.Cache;
using ShardingConnector.ParserEngine.Core.Parser;
using ShardingConnector.ParserEngine.Core.Visitor;
using ShardingConnector.ParserEngine.Hook;

namespace ShardingConnector.ParserEngine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/7 8:22:10
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class SqlParserEngine
    {
        private readonly string _dataSourceName;
        private readonly SqlParseResultCache _cache = new SqlParseResultCache();
        public SqlParserEngine(string dataSourceName)
        {
            _dataSourceName = dataSourceName;
        }
        public ISqlCommand Parse(string sql,bool useCache)
        {
            var parsingHookManager = ParsingHookManager.GetInstance();
            parsingHookManager.Start(sql);
            try
            {
                ISqlCommand result = Parse0(sql, useCache);
                parsingHookManager.FinishSuccess(result);
                return result;

            }
            catch(Exception e)
            {
                parsingHookManager.FinishFailure(e);
                throw e;
            }
        }

        private ISqlCommand Parse0(string sql, bool useCache)
        {
            if (useCache)
            {
                var sqlCommand = _cache.GetSqlCommand(sql);
                if (sqlCommand != null)
                    return sqlCommand;
            }
            IParseTree parseTree = new SqlParserExecutor(_dataSourceName, sql).Execute().GetRootNode();
            ISqlCommand result = (ISqlCommand)ParseTreeVisitorFactory.NewInstance(_dataSourceName, VisitorRule.ValueOf(parseTree.GetType())).Visit(parseTree);
            if (useCache)
            {
                _cache.Add(sql, result);
            }
            return result;
        }
    }
}
