using System;
using System.Collections.Generic;
using System.Text;

namespace NCDC.CommandParser.Common.Value.Literal.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 15:41:50
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class NumberLiteralValue:ILiteralValue<decimal>
    {

        public NumberLiteralValue(string value)
        {
            Value = GetNumber(value);
        }
        public decimal Value { get; }

        private decimal GetNumber(string value)
        {
            return decimal.Parse(value);
        }
    }
}
