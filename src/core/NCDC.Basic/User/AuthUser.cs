namespace NCDC.Basic.User
{
    public class AuthUser
    {
        public string Password { get; }
        public Grantee Grantee { get; }
        public AuthUser(string username,string password,string hostname="%")
        {
            Grantee = new Grantee(username, hostname);
            Password = password;
        }

        protected bool Equals(AuthUser other)
        {
            return Equals(Grantee, other.Grantee);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AuthUser)obj);
        }

        public override int GetHashCode()
        {
            return (Grantee != null ? Grantee.GetHashCode() : 0);
        }
    }
}