using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ShardingConnector
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/5 14:13:34
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class AssemblyHelper
    {
        public static AssemblyHelper CurrentDomain { get; private set; }

        static AssemblyHelper()
        {
            CurrentDomain = new AssemblyHelper();
        }

        public Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
