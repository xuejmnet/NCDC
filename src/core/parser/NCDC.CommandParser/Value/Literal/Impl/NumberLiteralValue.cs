using System;
using System.Collections.Generic;
using System.Text;

namespace NCDC.CommandParser.Value.Literal.Impl
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
        private readonly decimal _value;

        public NumberLiteralValue(string value)
        {
            _value = GetNumber(value);
        }
        public decimal GetValue()
        {
            return _value;
        }

        private decimal GetNumber(string value)
        {
            return decimal.Parse(value);
        }
    }
}
