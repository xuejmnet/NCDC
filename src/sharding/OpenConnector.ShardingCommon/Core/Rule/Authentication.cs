using System.Collections.Generic;

namespace OpenConnector.ShardingCommon.Core.Rule
{
    public class Authentication
    {
        public readonly IDictionary<string, ProxyUser> Users = new Dictionary<string, ProxyUser>();
    }
}