using System.Collections.Generic;
using ShardingConnector.Common.Config;

namespace ShardingConnector.Encrypt.Api
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 10:25:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class EncryptorRuleConfiguration:TypeBasedSPIConfiguration
    {
        public EncryptorRuleConfiguration(string type) : base(type)
        {
        }

        public EncryptorRuleConfiguration(string type, IDictionary<string, object> properties) : base(type, properties)
        {
        }
    }
}