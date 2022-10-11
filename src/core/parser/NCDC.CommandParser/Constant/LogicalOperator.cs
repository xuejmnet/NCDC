using System.Collections.Generic;
using NCDC.Extensions;
using NCDC.Extensions;

namespace NCDC.CommandParser.Constant
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 13:58:35
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class LogicalOperator
    {
        private static readonly IDictionary<string, LogicalOperatorEnum> _logicalOperators = new Dictionary<string, LogicalOperatorEnum>();

        static LogicalOperator()
        {        _logicalOperators.Add("and", LogicalOperatorEnum.AND);
            _logicalOperators.Add("And", LogicalOperatorEnum.AND);
            _logicalOperators.Add("aNd", LogicalOperatorEnum.AND);
            _logicalOperators.Add("anD", LogicalOperatorEnum.AND);
            _logicalOperators.Add("ANd", LogicalOperatorEnum.AND);
            _logicalOperators.Add("AnD", LogicalOperatorEnum.AND);
            _logicalOperators.Add("aND", LogicalOperatorEnum.AND);
            _logicalOperators.Add("AND", LogicalOperatorEnum.AND);
            _logicalOperators.Add("&&",  LogicalOperatorEnum.AND);
            _logicalOperators.Add("or",  LogicalOperatorEnum.OR);
            _logicalOperators.Add("Or",  LogicalOperatorEnum.OR);
            _logicalOperators.Add("oR",  LogicalOperatorEnum.OR);
            _logicalOperators.Add("OR",  LogicalOperatorEnum.OR);
            _logicalOperators.Add("||",  LogicalOperatorEnum.OR);
        }

        /**
         * Get logical operator value from text.
         *
         * @param text text
         * @return logical operator value
         */
        public static LogicalOperatorEnum? ValueFrom(string text)
        {
            if (_logicalOperators.TryGetValue(text, out var logicalOperator))
            {
                return logicalOperator;
            }

            return null;
        }
    }

    public enum LogicalOperatorEnum
    {
        AND,
        OR
    }
}
