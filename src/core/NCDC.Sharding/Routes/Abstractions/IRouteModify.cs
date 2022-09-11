namespace NCDC.Sharding.Routes.Abstractions;

public interface IRouteModify
{
    bool Append(string actualName);
    bool Remove(string actualName);
}