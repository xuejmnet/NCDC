using System;
using System.Collections.Generic;
using ShardingConnector.Exceptions;
using ShardingConnector.ShardingAdoNet.AdoNet.Adapter.Invocation;

namespace ShardingConnector.ShardingAdoNet.AdoNet.Adapter
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 10:42:56
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public abstract class WrapperAdapter:IWrapper
    {
        private readonly List<AdoNetMethodInvocation> _adoNetMethodInvocations = new List<AdoNetMethodInvocation>();
        public T Unwrap<T>()
        { 
            if (this is T t) {
                return t;
            }
            throw new ShardingException($"{this.GetType().Name} cannot be unwrapped as {typeof(T).Name}");
        }
        
        /**
     * record method invocation.
     * 
     * @param targetClass target class
     * @param methodName method name
     * @param argumentTypes argument types
     * @param arguments arguments
     */
        public  void RecordMethodInvocation(Type targetType, string methodName, Type[] argumentTypes, object[] arguments) {
            _adoNetMethodInvocations.Add(new AdoNetMethodInvocation(targetType.GetMethod(methodName, argumentTypes), arguments));
        }
    
        /**
     * Replay methods invocation.
     * 
     * @param target target object
     */
        public void ReplayMethodsInvocation(object target) {
            foreach (var adoNetMethodInvocation in _adoNetMethodInvocations)
            {
                adoNetMethodInvocation.Invoke(target);
            }
        }
    }
}