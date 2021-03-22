using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Kernels
{
    /*
    * @Author: xjm
    * @Description: 分表查询路由
    * @Date: 2021/3/22 17:33:41
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IShardingQueryDataReader
    {
        /// <summary>
        /// 是否有数据
        /// </summary>
        /// <returns></returns>
        bool Read();
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        object GetValue(int columnIndex);
    }
}
