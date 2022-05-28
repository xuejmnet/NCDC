using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using ShardingConnector.Base;
using ShardingConnector.Exceptions;

namespace ShardingConnector.AdoNet.AdoNet.Core.Command
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/7/23 15:41:38
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingParameterCollection:DbParameterCollection
    {
        private readonly DbParameterCollection _dbParameterCollection;

        private readonly List<ShardingParameter> _parameters;
        public ShardingParameterCollection(DbParameterCollection dbParameterCollection)
        {
            _dbParameterCollection = dbParameterCollection;
            _parameters = new List<ShardingParameter>(31);
        }

        public List<ShardingParameter> GetParams()
        {
            return _parameters;
        }
        private ShardingParameter CheckValue(object value)
        {
            if (value is ShardingParameter shardingParameter)
            {
                return shardingParameter;
            }
            else
            {
                throw new ShardingInvalidOperationException(
                    $"{nameof(ShardingParameterCollection)} value type:[${value.GetType()}] not {nameof(ShardingParameter)}");
            }
        }
        public override int Add(object value)
        {
            ShardingAssert.ShouldBeNotNull(value,nameof(value));
           
            var parameter = CheckValue(value);
            var i = _dbParameterCollection.Add(parameter.GetDbParameter());
            _parameters.Add(parameter);
            return i;
        }

        public override void Clear()
        {
            _dbParameterCollection.Clear();
            _parameters.Clear();
        }

        public override bool Contains(object value)
        {
            ShardingAssert.ShouldBeNotNull(value,nameof(value));
            var parameter = CheckValue(value);
            return _dbParameterCollection.Contains(parameter.GetDbParameter());
        }

        public override int IndexOf(object value)
        {
            ShardingAssert.ShouldBeNotNull(value,nameof(value));
            var parameter = CheckValue(value);
            return _dbParameterCollection.IndexOf(parameter.GetDbParameter());
        }

        public override void Insert(int index, object value)
        {
            ShardingAssert.ShouldBeNotNull(value,nameof(value));
            var parameter = CheckValue(value);
            _dbParameterCollection.Insert(index,parameter.GetDbParameter());
            _parameters.Insert(index,parameter);
        }

        public override void Remove(object value)
        {
            ShardingAssert.ShouldBeNotNull(value,nameof(value));
            var parameter = CheckValue(value);
            _dbParameterCollection.Remove(parameter.GetDbParameter());
            _parameters.Remove(parameter);
        }

        public override void RemoveAt(int index)
        {
            _dbParameterCollection.RemoveAt(index);
            _parameters.RemoveAt(index);
        }

        public override void RemoveAt(string parameterName)
        {
            ShardingAssert.ShouldBeNotNull(parameterName,nameof(parameterName));
            var dbParameter = _dbParameterCollection[parameterName];
            var index = _dbParameterCollection.IndexOf(dbParameter);
            _dbParameterCollection.RemoveAt(parameterName);
            _parameters.RemoveAt(index);
        }

        public new ShardingParameter this[int index]
        {
            get => _parameters[index];
            set => SetParameter(index, value);
        }
        
        public new ShardingParameter this[string name]
        {
            get => _parameters[_dbParameterCollection.IndexOf(_dbParameterCollection[name])];
            set => SetParameter(name, value);
        }
        protected override void SetParameter(int index, DbParameter value)
        {
            var parameter = CheckValue(value);
            _dbParameterCollection[index] = parameter.GetDbParameter();
            _parameters[index] = parameter;
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            var parameter = CheckValue(value);
            _dbParameterCollection[IndexOf(parameterName)] = parameter.GetDbParameter();
            _parameters[IndexOf(parameterName)] = parameter;
        }

        public override int Count => _dbParameterCollection.Count;
        public override object SyncRoot => _dbParameterCollection.SyncRoot;

        public override int IndexOf(string parameterName)
        {
            return _dbParameterCollection.IndexOf(parameterName);
        }

        public override bool Contains(string value)
        {
            return _dbParameterCollection.Contains(value);
        }

        public override void CopyTo(Array array, int index)
        {
            var arrayList = new DbParameter[array.Length];
            int i = 0;
            foreach (var o in array)
            {
                var parameter = CheckValue(o);
                arrayList[i]=parameter.GetDbParameter();
                i++;
            }
            _dbParameterCollection.CopyTo(arrayList,index);
            ((ICollection)_parameters).CopyTo(array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        protected override DbParameter GetParameter(int index)
        {
            return _parameters[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return GetParameter(IndexOf(parameterName));
        }

        public override void AddRange(Array values)
        {
            foreach (var value in values)
            {
                Add(value);
            }
        }
    }
}
