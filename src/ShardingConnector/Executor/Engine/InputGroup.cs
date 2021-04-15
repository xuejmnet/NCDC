using System.Collections.Generic;

namespace ShardingConnector.Executor.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 13:13:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class InputGroup<T>
    {
        public InputGroup(List<T> inputs)
        {
            Inputs = inputs;
        }

        public List<T> Inputs { get;  }
    }
}