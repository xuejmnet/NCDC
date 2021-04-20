using System;

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

        Type GetLexerType();
        Type GetParserType();
        Type GetVisitorFacadeType();

    }
}
