using System.Collections.Generic;
using OpenConnector.Extensions;

namespace OpenConnector.CommandParser.Constant
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
        private static readonly IDictionary<LogicalOperatorEnum, LogicalOperator> _logicalOperators = new Dictionary<LogicalOperatorEnum, LogicalOperator>();

        static LogicalOperator()
        {
            _logicalOperators.Add(LogicalOperatorEnum.AND, new LogicalOperator("AND", "&&"));
            _logicalOperators.Add(LogicalOperatorEnum.OR, new LogicalOperator("OR", "||"));
        }
        private readonly ICollection<string> _texts = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

        private LogicalOperator(params string[] texts)
        {
            this._texts.AddAll(texts);
        }

        /**
         * Get logical operator value from text.
         *
         * @param text text
         * @return logical operator value
         */
        public static LogicalOperatorEnum? ValueFrom(string text)
        {
            foreach (var logicalOperator in _logicalOperators)
            {
                if (logicalOperator.Value._texts.Contains(text))
                {
                    return logicalOperator.Key;
                }
            }
            return null;
        }
    }

    public enum LogicalOperatorEnum
    {
        AND=1,
        OR=1<<1
    }
}
