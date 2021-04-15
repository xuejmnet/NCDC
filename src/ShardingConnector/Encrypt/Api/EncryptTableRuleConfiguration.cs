using System.Collections.Generic;
using ShardingConnector.Extensions;

namespace ShardingConnector.Encrypt.Api
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 10:25:46
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class EncryptTableRuleConfiguration
    {
        public readonly IDictionary<string, EncryptColumnRuleConfiguration> Columns = new Dictionary<string, EncryptColumnRuleConfiguration>();
    
        public EncryptTableRuleConfiguration(IDictionary<string, EncryptColumnRuleConfiguration> columns) {
            this.Columns.AddAll(columns);
        }
    }
}