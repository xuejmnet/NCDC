using NCDC.CommandParser.Segment.DML.Expr;
using NCDC.CommandParser.Segment.DML.Expr.Simple;
using OpenConnector.Exceptions;
using OpenConnector.Extensions;
using NCDC.ShardingAdoNet;

namespace NCDC.Basic.Parser.Segment.Insert.Values
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

        private readonly ParameterContext _parameterContext;
        /// <summary>
        /// 插入值的上下文,可能存在一次插入有常量也有非常量
        /// 所以先获取参数的表达式片段并且获取其参数名称
        /// 将参数名称通过循环获取parameters的key里面寻找
        /// </summary>
        /// <param name="assignments"></param>
        /// <param name="parameters"></param>
        public InsertValueContext(ICollection<IExpressionSegment> assignments, ParameterContext parameterContext)
        {
            //获取本次执行sql的参数个数
            var parameterNames = GetParameterNames(assignments);
            _parametersCount = parameterNames.Count;
            _valueExpressions = GetValueExpressions(assignments);
            this._parameterContext = GetParameterContext(parameterContext, parameterNames);
        }

        private List<string> GetParameterNames(ICollection<IExpressionSegment> assignments)
        {
            var result = new List<string>(assignments.Count);
            foreach (var assignment in assignments)
            {
                if (assignment is ParameterMarkerExpressionSegment parameterMarkerExpression)
                {
                    result.Add(parameterMarkerExpression.GetParameterName());
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

        private ParameterContext GetParameterContext(ParameterContext parameterContext, List<string> parameterNames)
        {
            if (0 == _parametersCount)
            {
                return new ParameterContext(0);
            }
            var result = new ParameterContext(_parametersCount);
            foreach (var parameterName in parameterNames)
            {
                if (!parameterContext.TryGetDbParameter(parameterName,out var dbParameter))
                {
                    throw new ShardingException($"parameter name:[{parameterName}] not found in parameters");
                }

                result.AddParameter(dbParameter);
            }
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
            return valueExpression is ParameterMarkerExpressionSegment parameterMarkerExpression ? _parameterContext.GetParameterValue(parameterMarkerExpression.GetParameterName()) : ((LiteralExpressionSegment)valueExpression).GetLiterals();
        }

        //private int GetParameterIndex(IExpressionSegment valueExpression)
        //{
        //    int result = 0;
        //    foreach (var valueExp in _valueExpressions)
        //    {

        //        if (valueExpression == valueExp)
        //        {
        //            return result;
        //        }
        //        if (valueExp is ParameterMarkerExpressionSegment)
        //        {
        //            result++;
        //        }
        //    }
        //    throw new ShardingException("Can not get parameter index.");
        //}

        public int GetParametersCount()
        {
            return _parametersCount;
        }

        public ParameterContext GetParameterContext()
        {
            return _parameterContext;
        }

        public List<IExpressionSegment> GetValueExpressions()
        {
            return _valueExpressions;
        }
    }
}
