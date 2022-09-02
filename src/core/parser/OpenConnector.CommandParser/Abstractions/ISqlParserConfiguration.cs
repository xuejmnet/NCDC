using OpenConnector.CommandParser.Abstractions.SqlParser;
using OpenConnector.CommandParser.Abstractions.Visitor;

namespace OpenConnector.CommandParser.Abstractions
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
        /// 通过sql创建sql解析器
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        ISqlParser CreateSqlParser(string sql);
        /// <summary>
        /// 创建一个访问器
        /// </summary>
        /// <returns></returns>
        ISqlVisitorCreator CreateVisitorCreator();

    }
}
