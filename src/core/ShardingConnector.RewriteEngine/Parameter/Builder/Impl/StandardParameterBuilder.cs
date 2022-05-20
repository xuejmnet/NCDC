using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ShardingConnector.Extensions;

namespace ShardingConnector.RewriteEngine.Parameter.Builder.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 12:48:57
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class StandardParameterBuilder:IParameterBuilder
    {
        private readonly IDictionary<string, DbParameter> _originalParameters;

        private readonly IDictionary<int, ICollection<object>> _addedIndexAndParameters = new SortedDictionary<int, ICollection<object>>();

        private readonly IDictionary<string, object> _replacedIndexAndParameters = new Dictionary<string, object>();

        private readonly ISet<string> _removeParameterNames = new HashSet<string>();

        public StandardParameterBuilder(IDictionary<string, DbParameter> originalParameters)
        {
            this._originalParameters = originalParameters;
        }
        /**
         * Add added parameters.
         * 
         * @param index parameters index to be added
         * @param parameters parameters to be added
         */
        public void AddAddedParameters(int index, ICollection<object> parameters)
        {
            _addedIndexAndParameters.Add(index, parameters);
        }

        /**
         * Add replaced parameter.
         * 
         * @param index parameter index to be replaced
         * @param parameter parameter to be replaced
         */
        public void AddReplacedParameters(int index, object parameter)
        {
            throw new NotImplementedException();
            // _replacedIndexAndParameters.Add(index, parameter);
        }

        /**
         * Add removed parameter.
         *
         * @param index parameter index to be removed
         */
        public void AddRemovedParameters(string parameterName)
        {
            _removeParameterNames.Add(parameterName);
        }

        public IDictionary<string,DbParameter> GetParameters()
        {
            var result = new Dictionary<string,DbParameter>(_originalParameters);
            foreach (var replaced in _replacedIndexAndParameters)
            {
                result[replaced.Key].Value = replaced.Value;
            }
            // foreach (var added in _addedIndexAndParameters.Reverse())
            // {
            //     if (added.Key > result.Count)
            //     {
            //         result.AddAll(added.Value);
            //     }
            //     else
            //     {
            //         result.InsertRange(added.Key, added.Value);
            //     }
            // }
            foreach (var removeParameterName in _removeParameterNames)
            {
                result.Remove(removeParameterName);
            }
            return result;
        }

        public IDictionary<int, ICollection<object>> GetAddedIndexAndParameters()
        {
            return _addedIndexAndParameters;
        }

        public IDictionary<string, DbParameter> GetOriginalParameters()
        {
            return _originalParameters;
        }
    }
}
