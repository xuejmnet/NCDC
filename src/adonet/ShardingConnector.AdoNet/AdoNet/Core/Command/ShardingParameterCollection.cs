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
    public sealed class ShardingParameterCollection : DbParameterCollection
    {

        private readonly List<ShardingParameter> _parameters;
        private readonly Dictionary<string, int> _nameToIndex;

        public ShardingParameterCollection()
        {
            _parameters = new List<ShardingParameter>(31);
            _nameToIndex = new Dictionary<string, int>(31, StringComparer.OrdinalIgnoreCase);
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
            ShardingAssert.ShouldBeNotNull(value, nameof(value));

            var parameter = CheckValue(value);
            parameter.ShardingParameters = this;
            bool hasName = false;
            var nameIsEmpty = string.IsNullOrEmpty(parameter.ParameterName);
            if (!nameIsEmpty)
            {
                hasName = _nameToIndex.TryGetValue(parameter.ParameterName, out var nameIndex);
                if (hasName && nameIndex != -1)
                {
                    throw new ShardingException($"Parameter '{parameter.ParameterName}' has already been defined.");
                }
            }

            if (!hasName)
            {
                if (!nameIsEmpty)
                {
                    _nameToIndex.Add(parameter.ParameterName, _parameters.Count);
                }
            }
            else
            {
                _nameToIndex[parameter.ParameterName] = _parameters.Count;
            }

            _parameters.Add(parameter);

            return _parameters.Count - 1;
        }

        public override void Clear()
        {
            _parameters.Clear();
        }

        public override bool Contains(object value)
        {
            ShardingAssert.ShouldBeNotNull(value, nameof(value));
            var parameter = CheckValue(value);
            return _parameters.Contains(parameter);
        }

        public override int IndexOf(object value)
        {
            ShardingAssert.ShouldBeNotNull(value, nameof(value));
            var parameter = CheckValue(value);
            return _parameters.IndexOf(parameter);
        }

        public override void Insert(int index, object value)
        {
            ShardingAssert.ShouldBeNotNull(value, nameof(value));
            var parameter = CheckValue(value);
            if (!string.IsNullOrEmpty(parameter.ParameterName))
            {
                if (_nameToIndex.TryGetValue(parameter.ParameterName, out var i))
                {
                    throw new ShardingException($"Parameter '{parameter.ParameterName}' has already been defined.");
                }
            }

            if (index < _parameters.Count)
            {
                //将所有nameToIndex在插入index后面的都后移一位
                foreach (var pair in _nameToIndex)
                {
                    if (pair.Value >= index)
                        _nameToIndex[pair.Key] = pair.Value + 1;
                }
            }

            _parameters.Insert(index, parameter);
            _nameToIndex.Add(parameter.ParameterName, index);
        }

        public override void Remove(object value)
        {
            ShardingAssert.ShouldBeNotNull(value, nameof(value));
            var parameter = CheckValue(value);
            RemoveAt(IndexOf(parameter));
        }

        public override void RemoveAt(int index)
        {
            if (index < 0 || index >= _parameters.Count)
                return;
            var parameter = _parameters[index];
            _parameters.RemoveAt(index);
            if (!string.IsNullOrEmpty(parameter.ParameterName))
            {
                _nameToIndex.Remove(parameter.ParameterName);
            }
        }

        public override void RemoveAt(string parameterName)
        {
            ShardingAssert.ShouldBeNotNull(parameterName, nameof(parameterName));
            var indexOf = IndexOf(parameterName);
            RemoveAt(indexOf);
        }

        public new ShardingParameter this[int index]
        {
            get => _parameters[index];
            set => SetParameter(index, value);
        }

        public new ShardingParameter this[string name]
        {
            get => _parameters[IndexOf(name)];
            set => SetParameter(name, value);
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            var oldParameter = _parameters[index];
            var parameter = CheckValue(value);
            _parameters[index] = parameter;
            if (!string.IsNullOrEmpty(oldParameter.ParameterName))
            {
                if (!_nameToIndex.TryGetValue(oldParameter.ParameterName, out _))
                {
                    _nameToIndex.Add(oldParameter.ParameterName,index);
                }
                else
                {
                    _nameToIndex[oldParameter.ParameterName] = index;
                }
            }
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            var indexOf = IndexOf(parameterName);
            SetParameter(indexOf,value);
        }

        public override int Count => _parameters.Count;
        public override object SyncRoot => throw new NotSupportedException();

        public override int IndexOf(string parameterName)
        {
            if (!string.IsNullOrEmpty(parameterName))
            {
                return _nameToIndex.TryGetValue(parameterName, out var index) ? index : -1;
            }

            return -1;
        }

        public override bool Contains(string value)
        {
            return IndexOf(value) != -1;
        }

        public override void CopyTo(Array array, int index)
        {
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

        internal void ChangeParameterName(ShardingParameter parameter, string oldName, string newName)
        {
            if (!string.IsNullOrEmpty(oldName) && _nameToIndex.TryGetValue(oldName, out var index) &&
                _parameters[index] == parameter)
                _nameToIndex.Remove(oldName);
            else
                index = _parameters.IndexOf(parameter);

            if (!string.IsNullOrEmpty(newName))
            {
                if (_nameToIndex.ContainsKey(newName))
                    throw new ShardingException(
                        $"There is already a parameter with the name '{parameter.ParameterName}' in this collection.");
                _nameToIndex[newName] = index;
            }
        }
    }
}