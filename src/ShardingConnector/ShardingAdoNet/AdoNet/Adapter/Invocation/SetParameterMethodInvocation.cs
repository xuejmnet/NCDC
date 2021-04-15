using System.Reflection;

namespace ShardingConnector.ShardingAdoNet.AdoNet.Adapter.Invocation
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 10:59:46
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class SetParameterMethodInvocation : AdoNetMethodInvocation
    {
        public int Index { get; }

        public object Value { get; }

        public SetParameterMethodInvocation(MethodInfo method, object[] arguments, object value) : base(method,
            arguments)
        {
            this.Index = (int) arguments[0];
            this.Value = value;
        }


        /**
     * Set argument.
     * 
     * @param value argument value
     */
        public void ChangeValueArgument(object value)
        {
            Arguments[1] = value;
        }
    }
}