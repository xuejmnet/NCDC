namespace NCDC.ProxyServer.Options;

public  class UserOption
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public ISet<string> DatabaseNames { get; set; } = new HashSet<string>();
}