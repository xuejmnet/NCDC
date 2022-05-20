using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ShardingConnector.Extensions;

namespace ShardingConnector.RewriteEngine.Parameter.Builder.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 12:46:39
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class GroupedParameterBuilder:IParameterBuilder
    {
        private readonly List<StandardParameterBuilder> _parameterBuilders;

        private readonly IDictionary<string, DbParameter> _onDuplicateKeyUpdateAddedParameters = new Dictionary<string, DbParameter>();

        private string derivedColumnName;

        public GroupedParameterBuilder(List<IDictionary<string, DbParameter>> groupedParameters)
        {
            _parameterBuilders = new List<StandardParameterBuilder>(groupedParameters.Count);
            foreach (var groupedParameter in groupedParameters)
            {
                _parameterBuilders.Add(new StandardParameterBuilder(groupedParameter));
            }
        }

        /**
         * Get parameters.
         * 
         * @param count parameters group count
         * @return parameters
         */
        public IDictionary<string,DbParameter> GetParameters(int count)
        {
            return _parameterBuilders[count].GetParameterContext();
        }

        /**
         * Get derived column name.
         * 
         * @return derived column name
         */
        public string GetDerivedColumnName()
        {
            return derivedColumnName;
        }

        public void SetDerivedColumnName(string derivedColumnName)
        {
            this.derivedColumnName = derivedColumnName;
        }
        public IDictionary<string,DbParameter> GetParameters()
        {
            var result = new Dictionary<string,DbParameter>();
            for (int i = 0; i < _parameterBuilders.Count; i++)
            {
                result.AddAll(GetParameters(i));
            }
            if (_onDuplicateKeyUpdateAddedParameters.Any())
            {
                result.AddAll(_onDuplicateKeyUpdateAddedParameters);
            }
            return result;
        }

        public List<StandardParameterBuilder> GetParameterBuilders()
        {
            return _parameterBuilders;
        }
    }
}
