using System;
using System.Collections.Generic;
using System.Text;
using NCDC.Extensions;

namespace NCDC.CommandParser.Common.Value.Literal.Impl
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

        public StringLiteralValue(string value)
        {
            Value = value.SubStringWithEndIndex(1,value.Length-1);
        }
        public string Value { get; }
    }
}
