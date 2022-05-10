using System;
using ShardingConnector.AbstractParser.SqlLexer;
using ShardingConnector.AbstractParser.SqlParser;
using ShardingConnector.AbstractParser.Visitor;

namespace ShardingConnector.AbstractParser
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 8:34:23
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface ISqlParserConfiguration
    {
        /// <summary>
        /// 获取数据源名称
        /// </summary>
        /// <returns></returns>
        string GetDataSourceName();

        //Type GetLexerType();
        //Type GetParserType();
        //Type GetVisitorFacadeType();
        ///// <summary>
        ///// 词法分析
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        //ISqlLexer CreateSqlLexer(string sql);
        /// <summary>
        /// 通过sql创建sql解析器
        /// </summary>
        /// <param name="sqlLexer"></param>
        /// <returns></returns>
        ISqlParser CreateSqlParser(string sql);
        /// <summary>
        /// 创建一个访问器
        /// </summary>
        /// <returns></returns>
        ISqlVisitorFacade CreateVisitorFacade();

    }
}
