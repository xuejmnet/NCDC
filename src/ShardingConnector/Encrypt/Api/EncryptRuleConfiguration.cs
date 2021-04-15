using System.Collections.Generic;
using ShardingConnector.Common.Config;

namespace ShardingConnector.Encrypt.Api
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 10:24:13
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class EncryptRuleConfiguration : IRuleConfiguration
    {
        public EncryptRuleConfiguration(IDictionary<string, EncryptorRuleConfiguration> encryptors,
            IDictionary<string, EncryptTableRuleConfiguration> tables)
        {
            Encryptors = encryptors;
            Tables = tables;
        }

        public EncryptRuleConfiguration():this(new Dictionary<string, EncryptorRuleConfiguration>(),new Dictionary<string, EncryptTableRuleConfiguration>())
        {
            
        }

        public IDictionary<string, EncryptorRuleConfiguration> Encryptors { get; }

        public IDictionary<string, EncryptTableRuleConfiguration> Tables { get; }
    }
}