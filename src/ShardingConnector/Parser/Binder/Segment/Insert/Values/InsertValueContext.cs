using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Exceptions;
using ShardingConnector.Extensions;
using ShardingConnector.Parser.Sql.Segment.DML.Expr;
using ShardingConnector.Parser.Sql.Segment.DML.Expr.Simple;

namespace ShardingConnector.Parser.Binder.Segment.Insert.Values
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 8:10:34
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class InsertValueContext
    {
        private readonly int _parametersCount;

        private readonly List<IExpressionSegment> _valueExpressions;

        private readonly List<object> _parameters;

        public InsertValueContext(ICollection<IExpressionSegment> assignments, List<object> parameters, int parametersOffset)
        {
            _parametersCount = CalculateParametersCount(assignments);
            _valueExpressions = GetValueExpressions(assignments);
            this._parameters = GetParameters(parameters, parametersOffset);
        }

        private int CalculateParametersCount(ICollection<IExpressionSegment> assignments)
        {
            int result = 0;
            foreach (var assignment in assignments)
            {
                if (assignment is ParameterMarkerExpressionSegment)
                {
                    result++;
                }
            }
            return result;
        }

        private List<IExpressionSegment> GetValueExpressions(ICollection<IExpressionSegment> assignments)
        {
            List<IExpressionSegment> result = new List<IExpressionSegment>(assignments.Count);
            result.AddAll(assignments);
            return result;
        }

        private List<object> GetParameters(List<object> parameters, int parametersOffset)
        {
            if (0 == _parametersCount)
            {
                return new List<object>(0);
            }
            List<object> result = new List<object>(_parametersCount);

            result.AddRange(parameters.Skip(parametersOffset).Take(_parametersCount));
            return result;
        }

        /**
         * Get value.
         *
         * @param index index
         * @return value
         */
        public object GetValue(int index)
        {
            IExpressionSegment valueExpression = _valueExpressions[index];
            return valueExpression is ParameterMarkerExpressionSegment ? _parameters[GetParameterIndex(valueExpression)] : ((LiteralExpressionSegment)valueExpression).GetLiterals();
        }

        private int GetParameterIndex(IExpressionSegment valueExpression)
        {
            int result = 0;
            foreach (var valueExp in _valueExpressions)
            {

                if (valueExpression == valueExp)
                {
                    return result;
                }
                if (valueExp is ParameterMarkerExpressionSegment)
                {
                    result++;
                }
            }
            throw new ShardingException("Can not get parameter index.");
        }

        public int GetParametersCount()
        {
            return _parametersCount;
        }

        public List<object> GetParameters()
        {
            return _parameters;
        }
    }
}
