using System;
using System.Collections.Generic;
using System.Text;
using NCDC.Extensions;
using NCDC.Extensions;

namespace NCDC.CommandParser.Value.Collection
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 16:14:59
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class CollectionValue<T>:IValueASTNode<ICollection<T>>
    {
        private readonly ICollection<T> _value = new LinkedList<T>();
        public ICollection<T> GetValue()
        {
            return _value;
        }

        public void Combine(CollectionValue<T> collectionValue)
        {
            _value.AddAll(collectionValue.GetValue());
        }
    }
}
