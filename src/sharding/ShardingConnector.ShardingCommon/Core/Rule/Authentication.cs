using System.Collections.Generic;

namespace ShardingConnector.ShardingCommon.Core.Rule
{
    public class Authentication
    {
        public readonly IDictionary<string, ProxyUser> Users = new Dictionary<string, ProxyUser>();
    }
}