using System;
using System.Collections.Generic;

namespace ShardingConnector.Core
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 21 March 2021 21:08:28
* @Email: 326308290@qq.com
*/
    public interface IShardingEnumerator<T>:IEnumerator<T>
    {
        /// <summary>
        /// 跳过第一个
        /// </summary>
        /// <returns></returns>
        bool SkipFirst();
        /// <summary>
        /// 是否有值
        /// </summary>
        /// <returns></returns>
        bool HasElement();
        /// <summary>
        /// 真实的当前值
        /// </summary>
        T ReallyCurrent { get; }
    }
}