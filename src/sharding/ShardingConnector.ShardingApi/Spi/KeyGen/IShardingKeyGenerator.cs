using System;
using ShardingConnector.Spi;

namespace ShardingConnector.ShardingApi.Spi.KeyGen
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public interface IShardingKeyGenerator:ITypeBasedSpi
    {
        /**
     * Generate key.
     * 
     * @return generated key
     */
        IComparable<object> GenerateKey();
    }
}