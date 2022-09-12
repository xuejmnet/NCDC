using System;
using System.Collections.Generic;
using System.Text;
using NCDC.Extensions;

namespace NCDC.CommandParser.Value.Literal.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 15:23:22
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class StringLiteralValue:ILiteralValue<string>
    {
        private readonly string _value;

        public StringLiteralValue(string value)
        {
            _value = value.SubStringWithEndIndex(1,value.Length-1);
        }
        public string GetValue()
        {
            return _value;
        }
    }
}
