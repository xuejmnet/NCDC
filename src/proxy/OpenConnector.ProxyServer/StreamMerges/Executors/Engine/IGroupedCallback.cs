using System.Collections.Generic;

namespace OpenConnector.Executor.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 13:13:52
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public interface IGroupedCallback<T,R>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="isTrunkThread">是否是主线程</param>
        /// <param name="dataMap"></param>
        /// <returns></returns>
        ICollection<R> Execute(ICollection<T> inputs, bool isTrunkThread, IDictionary<string, object> dataMap);
    }
}