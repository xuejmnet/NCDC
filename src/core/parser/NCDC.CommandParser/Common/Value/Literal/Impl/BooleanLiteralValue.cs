using System;
using System.Collections.Generic;
using System.Text;

namespace NCDC.CommandParser.Common.Value.Literal.Impl
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

        public BooleanLiteralValue(bool value)
        {
            Value = value;
        }
        public BooleanLiteralValue(string value)
        {
            Value = bool.Parse(value);
        }
        public bool Value { get; }
    }
}
