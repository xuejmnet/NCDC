using System;
using System.Collections.Generic;
using System.Text;

namespace OpenConnector.CommandParser.Value.KeyWord
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 16:10:58
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class KeywordValue:IValueASTNode<string>
    {
        private readonly string _value;

        public KeywordValue(string value)
        {
            _value = value;
        }
        public string GetValue()
        {
            return _value;
        }
    }
}
