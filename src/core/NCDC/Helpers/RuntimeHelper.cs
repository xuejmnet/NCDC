using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using NCDC.Extensions;

namespace NCDC.Helpers
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/5 14:13:34
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class RuntimeHelper
    {
        /// <summary>
        /// 获取项目程序集，排除所有的系统程序集(Microsoft.***、System.***等)、Nuget下载包
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAllAssemblies()
        {
            return DependencyContext.Default.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package")
                .Select(x => Assembly.Load(new AssemblyName(x.Name)));
        }
        public static IEnumerable<Type> GetAllTypes()
        {
            return GetAllAssemblies().SelectMany(assembly => assembly.GetTypes());
        }

        public static IEnumerable<Type> GetImplementTypes(Type baseInterfaceType)
        {
            return GetAllTypes().Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.IsClass && !type.IsAbstract && (baseInterfaceType.IsAssignableFrom(type)|| type.HasImplementedRawGeneric(baseInterfaceType))
                );
        }
    }



}
