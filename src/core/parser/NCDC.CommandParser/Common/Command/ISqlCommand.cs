using NCDC.CommandParser.Abstractions;

namespace NCDC.CommandParser.Common.Command
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 23 March 2021 21:23:58
* @Email: 326308290@qq.com
*/
    public interface ISqlCommand:IASTNode
    {
        /// <summary>
        /// 获取参数个数
        /// </summary>
        /// <returns></returns>
        int ParameterCount { get; }
    }
}