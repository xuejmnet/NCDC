using System;

namespace ShardingConnector.SqlParser.SqlExpression
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 23 March 2021 21:23:58
* @Email: 326308290@qq.com
*/
    public interface ISqlCommand:ISqlNode
    {
        /// <summary>
        /// 获取参数个数
        /// </summary>
        /// <returns></returns>
        int GetParameterCount();
    }
}