namespace ShardingConnector.ShardingCommon.User
{
    public class ShardingConnectorUser
    {
        public string Password { get; }
        private Grantee Grantee { get; }
        public ShardingConnectorUser(string username,string password,string hostname )
        {
            Grantee = new Grantee(username, hostname);
            Password = password;
        }

        protected bool Equals(ShardingConnectorUser other)
        {
            return Equals(Grantee, other.Grantee);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ShardingConnectorUser)obj);
        }

        public override int GetHashCode()
        {
            return (Grantee != null ? Grantee.GetHashCode() : 0);
        }
    }
}