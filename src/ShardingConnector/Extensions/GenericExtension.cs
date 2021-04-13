using System;
using System.Linq;

namespace ShardingConnector.Extensions
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 13 April 2021 21:32:26
* @Email: 326308290@qq.com
*/
    public static class GenericExtension
    {
       public static Type[] GetGenericArguments(this Type type, Type genericType)
        {
            return type.GetInterfaces() //取类型的接口
                .Where(i => IsGenericType(i)) //筛选出相应泛型接口
                .SelectMany(i => i.GetGenericArguments()) //选择所有接口的泛型参数
                .ToArray(); //ToArray

            bool IsGenericType(Type type1)
                => type1.IsGenericType && type1.GetGenericTypeDefinition() == genericType;
        }
    }
}