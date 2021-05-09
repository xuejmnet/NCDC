using System.Reflection;

namespace ShardingConnector.ShardingAdoNet.AdoNet.Adapter.Invocation
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 10:57:55
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class AdoNetMethodInvocation
    {
        public AdoNetMethodInvocation(MethodInfo method, object[] arguments)
        {
            Method = method;
            Arguments = arguments;
        }

        public MethodInfo Method { get; }

        public object[] Arguments { get; }

        /**
     * Invoke JDBC method.
     * 
     * @param target target object
     */
        public void Invoke(object target)
        {
            Method.Invoke(target, Arguments);
        }
    }
}