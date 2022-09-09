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
namespace OpenConnector.ShardingAdoNet
{
    public sealed class ParameterContext
    {
        public static ParameterContext Empty { get; } = new ParameterContext();
        private readonly IDictionary<string, DbParameter> _parameters;
        public ParameterContext(DbParameter[] dbParameters, int capacity)
        {
            _parameters = new Dictionary<string, DbParameter>(capacity);
            foreach (var dbParameter in dbParameters)
            {
                _parameters.Add(dbParameter.ParameterName, dbParameter);
            }
        }
        //
        // internal static string NormalizeParameterName(string name)
        // {
        //     name = name.Trim();
        //
        //     if ((name.StartsWith("@`", StringComparison.Ordinal) || name.StartsWith("?`", StringComparison.Ordinal)) && name.EndsWith("`", StringComparison.Ordinal))
        //         return name.Substring(2, name.Length - 3).Replace("``", "`");
        //     if ((name.StartsWith("@'", StringComparison.Ordinal) || name.StartsWith("?'", StringComparison.Ordinal)) && name.EndsWith("'", StringComparison.Ordinal))
        //         return name.Substring(2, name.Length - 3).Replace("''", "'");
        //     if ((name.StartsWith("@\"", StringComparison.Ordinal) || name.StartsWith("?\"", StringComparison.Ordinal)) && name.EndsWith("\"", StringComparison.Ordinal))
        //         return name.Substring(2, name.Length - 3).Replace("\"\"", "\"");
        //
        //     return name.StartsWith("@", StringComparison.Ordinal) || name.StartsWith("?", StringComparison.Ordinal) ? name.Substring(1) : name;
        // }

        public ParameterContext(DbParameter[] dbParameters):this(dbParameters, dbParameters.Length)
        {

        }
        public ParameterContext():this(Array.Empty<DbParameter>(), 31)
        {

        }
        public ParameterContext(int capacity):this(Array.Empty<DbParameter>(), capacity)
        {

        }

        /// <summary>
        /// 直接获取值
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public object GetParameterValue(string parameterName)
        {
            return _parameters[parameterName].Value;
        }
        /// <summary>
        /// 直接获取值DbParameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public DbParameter GetDbParameter(string parameterName)
        {
            return _parameters[parameterName];
        }

        /// <summary>
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
        /// <summary>
        /// 尝试获取参数值
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbParameter"></param>
        /// <returns></returns>
        public bool TryGetDbParameter(string parameterName,out DbParameter dbParameter)
        {
            return _parameters.TryGetValue(parameterName, out dbParameter);
        }

        public IDictionary<string, DbParameter> GetParametersWithName()
        {
            return _parameters;
        }
        public ICollection<DbParameter> GetDbParameters()
        {
            return _parameters.Values;
        }
        public IDictionary<string, DbParameter> GetCloneParameters()
        {
            return _parameters.ToDictionary(o => o.Key, o => o.Value);
        }

        public bool ReplaceParameterValue(string parameterName, object value)
        {
            if (TryGetDbParameter(parameterName, out var dbParameter))
            {
                dbParameter.Value = value;
                return true;
            }

            return false;
        }

        public bool RemoveDbParameter(string parameterName)
        {
            return _parameters.Remove(parameterName);
        }
        public int GetParameterCount()
        {
            return _parameters.Count;
        }

        public ParameterContext CloneParameterContext()
        {
            return new ParameterContext(_parameters.Values.ToArray());
        }

        public void AddParameters(ICollection<DbParameter> dbParameters)
        {
            foreach (var dbParameter in dbParameters)
            {
                _parameters.Add(dbParameter.ParameterName, dbParameter);
            }
        }
        public void AddParameter(DbParameter dbParameter)
        {
            _parameters.Add(dbParameter.ParameterName, dbParameter);
        }

        public bool IsEmpty()
        {
            return GetParameterCount() == 0;
        }
    }
}