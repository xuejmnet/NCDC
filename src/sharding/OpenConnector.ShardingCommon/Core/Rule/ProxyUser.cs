using System.Collections.Generic;

namespace OpenConnector.ShardingCommon.Core.Rule
{
    public sealed class ProxyUser
    {
        public ProxyUser(string password, List<string> authorizedSchemas)
        {
            Password = password;
            AuthorizedSchemas = authorizedSchemas;
        }

        public string Password { get; }
        public List<string> AuthorizedSchemas { get; }
    }
}