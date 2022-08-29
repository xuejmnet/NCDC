using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using OpenConnector.Extensions;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.RewriteEngine.Parameter.Builder.Impl
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

        private readonly ParameterContext _onDuplicateKeyUpdateAddedParameters = new ParameterContext();

        private string derivedColumnName;

        public GroupedParameterBuilder(List<ParameterContext> groupedParameters)
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
        public ParameterContext GetParameterContext(int count)
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
        public ParameterContext GetParameterContext()
        {
            var result = new ParameterContext();
            for (int i = 0; i < _parameterBuilders.Count; i++)
            {
                var dbParameters = GetParameterContext(i).GetDbParameters();
                result.AddParameters(dbParameters);
            }
            if (_onDuplicateKeyUpdateAddedParameters.GetParameterCount()>0)
            {
                var dbParameters = _onDuplicateKeyUpdateAddedParameters.GetDbParameters();
                result.AddParameters(dbParameters);
            }
            return result;
        }

        public List<StandardParameterBuilder> GetParameterBuilders()
        {
            return _parameterBuilders;
        }
    }
}
