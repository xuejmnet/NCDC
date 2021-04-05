using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShardingConnector
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/5 13:08:17
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class NewInstanceServiceLoader
    {
        /// <summary>
        /// 服务集合
        /// </summary>
        private static readonly IDictionary<Type, ICollection<Type>> ServiceMap =
            new Dictionary<Type, ICollection<Type>>();

        /// <summary>
        /// 注册接口对应的多实例实现
        /// </summary>
        /// <typeparam name="TIService"></typeparam>
        public static void Register<TIService>()
        {
            var loadServices = LoadServices<TIService>();
            foreach (var service in loadServices)
            {
                RegisterServiceType(typeof(TIService),service);
            }
        }

        private static ICollection<TIService> LoadServices<TIService>()
        {
            var serviceType = typeof(TIService);

            var serviceImpls = AssemblyHelper.CurrentDomain.GetAssemblies().SelectMany(o => o.GetTypes())
                .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(type => !type.IsAbstract && type.GetInterfaces()
                    .Any(it => it.IsInterface && serviceType == it)
                &&type.GetConstructors().Length==1&&type.GetConstructors()[0].GetParameters().Length==0
                );
            return serviceImpls.Select(o => (TIService)Activator.CreateInstance(o)).ToList();
        }

        /// <summary>
        /// 添加服务类型
        /// </summary>
        /// <param name="serviceType">接口类型</param>
        /// <typeparam name="TInstance"></typeparam>
        /// <param name="instance"></param>
        private static void RegisterServiceType<TInstance>(Type serviceType,TInstance instance)
        {
            if (!ServiceMap.TryGetValue(serviceType, out var serviceTypes))
            {
                serviceTypes = new LinkedList<Type>();
            }
            serviceTypes.Add(instance.GetType());
            ServiceMap.Add(serviceType,serviceTypes);
        }


        /// <summary>
        /// 获取接口对应的所有实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ICollection<T> NewServiceInstances<T>()
        {
            ICollection<T> result = new LinkedList<T>();
            if(!ServiceMap.TryGetValue(typeof(T), out var services))
            {
                return result;
            }

            foreach (var service in services)
            {
                result.Add((T)Activator.CreateInstance(service));
            }
            return result;
        }

    }
}
