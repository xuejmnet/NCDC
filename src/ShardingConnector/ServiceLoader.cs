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
            

            var serviceImpls = AssemblyHelper.CurrentDomain.GetAssemblies().SelectMany(o => o.GetTypes())
                .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(type => !type.IsAbstract && type.GetInterfaces()
                                                    .Any(it => it.IsInterface && serviceType == it)
                                                &&type.GetConstructors().Length==1&&type.GetConstructors()[0].GetParameters().Length==0
                );
            return serviceImpls.Select(o => Activator.CreateInstance(o)).ToArray();
        }
    }
}