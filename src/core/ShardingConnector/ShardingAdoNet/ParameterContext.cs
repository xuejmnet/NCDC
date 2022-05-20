using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

/*
* @Author: xjm
* @Description:
* @Date: DATE
* @Email: 326308290@qq.com
*/
namespace ShardingConnector.ShardingAdoNet
{
    public sealed class ParameterContext
    {
        private readonly IDictionary<string, DbParameter> _parameters;
        private readonly int _parameterCount;
        public ParameterContext(DbParameter[] dbParameters)
        {
            _parameters = dbParameters.ToDictionary(o => o.ParameterName, o => o);
            _parameterCount = dbParameters.Length;
        }

        /// <summary>
        /// 直接获取值
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public object GetParameterValue(string parameterName)
        {
            return _parameters[parameterName];
        }
        
        /// 尝试获取参数值
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetParameterValue(string parameterName,out object value)
        {
            var has= _parameters.TryGetValue(parameterName, out var dbParameter);
            value = has ? dbParameter.Value : null;
            return has;
        }

        public IDictionary<string, DbParameter> GetParameters()
        {
            return _parameters;
        }
        public IDictionary<string, DbParameter> GetCloneParameters()
        {
            return _parameters.ToDictionary(o => o.Key, o => o.Value);
        }

        public int GetParameterCount()
        {
            return _parameterCount;
        }

        public ParameterContext CloneParameterContext()
        {
            return new ParameterContext(_parameters.Values.ToArray());
        }
    }
}