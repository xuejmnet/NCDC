using System;
using System.Linq;

namespace ShardingConnector
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 11:39:06
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class ServiceLoader
    {
        private ServiceLoader(){}
        public static T[] Load<T>()
        {
            return Load(typeof(T)).Select(o => (T)o).ToArray();
        }
        public static object[] Load(Type serviceType)
        {
            

            var serviceImpls = RuntimeHelper.GetImplementTypes(serviceType);
            return serviceImpls.Select(o => Activator.CreateInstance(o)).ToArray();
        }
    }
}