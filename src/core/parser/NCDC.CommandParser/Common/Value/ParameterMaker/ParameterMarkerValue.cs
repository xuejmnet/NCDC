using System;
using System.Collections.Generic;
using System.Text;

namespace NCDC.CommandParser.Common.Value.ParameterMaker
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

        public ParameterMarkerValue(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}
