using ShardingConnector.Api.Spi.KeyGen;

namespace ShardingConnector.Spi.Algorithm.KeyGen
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 13:17:43
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class ShardingKeyGeneratorServiceLoader:TypeBasedSPIServiceLoader<IShardingKeyGenerator> {
    
        static ShardingKeyGeneratorServiceLoader(){
            NewInstanceServiceLoader.Register<IShardingKeyGenerator>();
        }
    }
}