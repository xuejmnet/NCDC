using System;
using System.Collections.Generic;
using OpenConnector.ShardingApi.Spi.KeyGen;

namespace OpenConnector.ShardingCommon.Core.Strategy.KeyGen
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 23 April 2021 21:41:54
* @Email: 326308290@qq.com
*/
    public sealed class UUIDShardingKeyGenerator:IShardingKeyGenerator
    {
        private IDictionary<string, object> _properties = new Dictionary<string, object>();
        public string GetAlgorithmType()
        {
            return "UUID";
        }

        public IDictionary<string, object> GetProperties()
        {
            return _properties;
        }

        public void SetProperties(IDictionary<string, object> properties)
        {
            this._properties = properties;
        }

        public IComparable GenerateKey()
        {
            return Guid.NewGuid().ToString("n");
        }
    }
}