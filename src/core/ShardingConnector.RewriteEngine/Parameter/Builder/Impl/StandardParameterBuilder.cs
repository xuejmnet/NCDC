using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ShardingConnector.Extensions;
using ShardingConnector.ParserBinder.Segment.Select.Pagination;
using ShardingConnector.ShardingAdoNet;

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
        private readonly ParameterContext _originalParameterContext;

        private readonly IDictionary<int, ICollection<object>> _addedIndexAndParameters = new SortedDictionary<int, ICollection<object>>();

        private readonly IDictionary<string, object> _replacedIndexAndParameters = new Dictionary<string, object>();

        private readonly ISet<string> _removeParameterNames = new HashSet<string>();

        public StandardParameterBuilder(ParameterContext originalParameterContext)
        {
            this._originalParameterContext = originalParameterContext;
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

        public ParameterContext GetParameterContext()
        {
            var result = _originalParameterContext.CloneParameterContext();
            foreach (var replaced in _replacedIndexAndParameters)
            {
                result.ReplaceParameterValue(replaced.Key, replaced.Value);
            }
            foreach (var added in _addedIndexAndParameters.Reverse())
            {
                throw new NotImplementedException();
                //if (added.Key > result.Count)
                //{
                //    result.AddAll(added.Value);
                //}
                //else
                //{
                //    result.InsertRange(added.Key, added.Value);
                //}
            }
            foreach (var removeParameterName in _removeParameterNames)
            {
                result.RemoveDbParameter(removeParameterName);
            }
            return result;
        }

        public IDictionary<int, ICollection<object>> GetAddedIndexAndParameters()
        {
            return _addedIndexAndParameters;
        }

        public ParameterContext GetOriginalParameterContext()
        {
            return _originalParameterContext;
        }
    }
}
