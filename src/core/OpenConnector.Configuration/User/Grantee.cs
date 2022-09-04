using OpenConnector.Extensions;

namespace OpenConnector.Configuration.User
{
    public sealed class Grantee
    {
        public string Username { get; }
        public string Hostname { get; }

        private readonly bool _isUnLimitedHost;
        private readonly string _toString;
        
        public Grantee(string username,string hostname)
        {
            Username = username;
            Hostname = string.IsNullOrEmpty(hostname)?"%":hostname;
            _isUnLimitedHost = "%".Equals(Hostname);
            _toString = $"{Username}@{Hostname}";
        }

        public bool IsPermittedHost(Grantee grantee)
        {
            return _isUnLimitedHost || grantee.Hostname.EqualsIgnoreCase(Hostname);
        }

        private bool Equals(Grantee other)
        {
            return Username == other.Username && Hostname == other.Hostname;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Grantee other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Username != null ? Username.GetHashCode() : 0) * 397) ^ (Hostname != null ? Hostname.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return _toString;
        }
    }
}