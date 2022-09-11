using System;
using System.Collections.Generic;
using System.Text;

namespace NCDC.CommandParser.Value.Literal.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 16:04:06
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class BooleanLiteralValue:ILiteralValue<bool>
    {
        private readonly bool _value;

        public BooleanLiteralValue(bool value)
        {
            _value = value;
        }
        public BooleanLiteralValue(string value)
        {
            _value = bool.Parse(value);
        }
        public bool GetValue()
        {
            return _value;
        }
    }
}
