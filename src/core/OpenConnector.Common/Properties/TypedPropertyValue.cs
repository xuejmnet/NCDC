using System;
using System.Collections.Generic;
using System.Text;

namespace OpenConnector.Common.Properties
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 15:42:28
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TypedPropertyValue
    {
        private object value;

        public TypedPropertyValue(ITypedPropertyKey key, string value)
        {
            this.value = CreateTypedValue(key, value);
        }
        private object CreateTypedValue(ITypedPropertyKey key, string value)
        {
            if (key.GetValueType() == typeof(bool))
            {
                try
                {
                    return bool.Parse(value);
                }
                catch (Exception e)
                {
                    throw new TypedPropertyValueException(key, value, e.Message);
                };
            }

            if (key.GetValueType() == typeof(int))
            {
                try
                {
                    return int.Parse(value);
                }
                catch (Exception e)
                {
                    throw new TypedPropertyValueException(key, value, e.Message);
                }
            }

            if (key.GetValueType() == typeof(long))
            {

                try
                {
                    return long.Parse(value);
                }
                catch (Exception e)
                {
                    throw new TypedPropertyValueException(key, value, e.Message);
                }
            }
            return value;
        }

        public object GetValue()
        {
            return value;
        }
    }
}
