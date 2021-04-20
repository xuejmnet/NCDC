using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Common.Properties
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 15:41:28
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface ITypedPropertyKey
    {
        /**
         * Get property key.
         * 
         * @return property key
         */
        string GetKey();

        /**
         * Get default property value.
         * 
         * @return default property value
         */
        string GetDefaultValue();

        Type GetValueType();
    }
}
