// using System;
// using System.Collections.Generic;
// using System.Reflection;
// using System.Text;
//
// namespace ShardingConnector.AdoNet.AdoNet.Core
// {
//     /*
//     * @Author: xjm
//     * @Description:
//     * @Date: 2021/7/23 15:45:23
//     * @Ver: 1.0
//     * @Email: 326308290@qq.com
//     */
//     public class ShardingMethodInvocation
//     {
//         private readonly MethodInfo _method;
//         private readonly object[] _arguments;
//
//         public ShardingMethodInvocation(MethodInfo method,object[] arguments)
//         {
//             _method = method;
//             _arguments = arguments;
//         }
//
//         public void Inovke(object target)
//         {
//             _method.Invoke(target, _arguments);
//         }
//     }
// }
