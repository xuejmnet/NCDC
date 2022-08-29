using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.ShardingApi.Api.Sharding;

namespace OpenConnector.ShardingCommon.Core.Strategy.Route
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 12:45:10
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingAlgorithmFactory
    {
        private ShardingAlgorithmFactory()
        {

        }
        public static  T NewInstance<T,TBase>(string shardingAlgorithmClassName) where T: TBase where TBase : IShardingAlgorithm
        {
            //Class <?> result = Class.forName(shardingAlgorithmClassName);
            //if (!typeof(TBase).isAssignableFrom(result))
            //{
            //    throw new ShardingSphereException("Class %s should be implement %s", shardingAlgorithmClassName, superShardingAlgorithmClass.getName());
            //}
            //return (T)result.newInstance();
            return default;
        }
    }
}
