using System;
using System.Collections.Generic;
using System.Text;

namespace OpenConnector.CommandParser.Value.ParameterMaker
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 15:06:45
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ParameterMarkerValue:IValueASTNode<int>
    {
        private readonly int _value;

        public ParameterMarkerValue(int value)
        {
            _value = value;
        }

        public int GetValue()
        {
            return _value;
        }
    }
}
