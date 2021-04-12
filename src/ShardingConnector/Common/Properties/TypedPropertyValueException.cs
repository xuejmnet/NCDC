using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Common.Properties
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 15:46:04
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TypedPropertyValueException:Exception
    {
        public TypedPropertyValueException(ITypedPropertyKey key, string value,string error) : base($"Value `{value}` of `{key.GetKey()}` cannot convert to type `{key.GetValueType().Name}`,error:{error}.")
        {
        }
    }
}
